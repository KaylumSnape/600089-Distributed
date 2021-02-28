using System;
using System.Collections.Generic;
using System.Text;
using PipesAndFilters.Messages;

namespace PipesAndFilters
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerEnvironment.Setup();
            Client client = new Client();

            Console.WriteLine("Please enter your message:");
            var requestMessage = Console.ReadLine();

            Console.WriteLine("Please select encoding: \n1 - Byte \n2 - Binary \n3 - Hex");
            string encoding = Console.ReadLine();

            const string valid = "123";

            while (!valid.Contains(encoding) || String.IsNullOrEmpty(encoding))
            {
                Console.WriteLine("Invalid.");
                Console.WriteLine("Please select encoding: \n1 - Byte \n2 - Binary \n3 - Hex");
                encoding = Console.ReadLine();
            }

            client.RequestHello(requestMessage, int.Parse(encoding ?? string.Empty));
        }
    }
    class Client
    {
        int userId = 1;
        public void RequestHello(string requestMessage, int encoding)
        {
            IMessage message = new Message();

            // Add the user ID header
            message.Headers.Add("User", userId.ToString());
            
            switch (encoding)
            {
                case 1:
                    BytesEncoding(message, requestMessage);
                    break;
                case 2:
                    BinaryEncoding(message, requestMessage);
                    break;
                case 3:
                    HexEncoding(message, requestMessage);
                    break;
            }
        }

        internal void BytesEncoding(IMessage message, string requestMessage)
        {
            // Convert the message to a byte array and then turn the byte array into a string of byte values delimited by dashes
            message.Headers.Add("RequestFormat", "Bytes");
            byte[] bytes = Encoding.ASCII.GetBytes(requestMessage);
            string requestBody = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                requestBody += bytes[i].ToString();
                if (i + 1 < bytes.Length)
                {
                    requestBody += "-";
                }
            }
            message.Body = requestBody;

            // Send the message and get the response back
            IMessage response = ServerEnvironment.SendRequest(message);

            // Get the timestamp from the response
            response.Headers.TryGetValue("Timestamp", out string timestamp);

            // Turn the delimited string of bytes to a byte array and then to an ASCII string
            string responseBody = "";
            string[] byteStrings = response.Body.Split('-');
            bytes = new byte[byteStrings.Length];
            for (int i = 0; i < byteStrings.Length; i++)
            {
                bytes[i] = byte.Parse(byteStrings[i]);
            }
            responseBody = Encoding.ASCII.GetString(bytes);

            // Output the response to the Console
            Console.WriteLine($"At {timestamp} Response was: {responseBody}");
        }

        // Encode the request message in binary.
        internal void BinaryEncoding(IMessage message, string requestMessage)
        {
            message.Headers.Add("RequestFormat", "Binary");
            StringBuilder requestBody = new StringBuilder();
 
            foreach (char c in requestMessage.ToCharArray())
            {
                requestBody.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            message.Body = requestBody.ToString();

            // Send the message and get the response back
            IMessage response = ServerEnvironment.SendRequest(message);

            // Get the timestamp from the response
            response.Headers.TryGetValue("Timestamp", out string timestamp);

            // Turn the 
            string responseBody = "";

            List<Byte> byteList = new List<Byte>();
 
            for (int i = 0; i < response.Body.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(response.Body.Substring(i, 8), 2));
            }
            responseBody = Encoding.ASCII.GetString(byteList.ToArray());

            // Output the response to the Console
            Console.WriteLine($"At {timestamp} Response was: {responseBody}");
        }

        internal void HexEncoding(IMessage message, string requestMessage)
        {
            message.Headers.Add("RequestFormat", "Hex");
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(requestMessage);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            message.Body = sb.ToString();

            // Send the message.
            IMessage response = ServerEnvironment.SendRequest(message);

            // Get the timestamp from the response.
            response.Headers.TryGetValue("Timestamp", out string timestamp);

            string responseBody = "";

            bytes = new byte[response.Body.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(response.Body.Substring(i * 2, 2), 16);
            }

            responseBody = Encoding.Unicode.GetString(bytes);

            // Output the response to the Console
            Console.WriteLine($"At {timestamp} Response was: {responseBody}");
        }


    }
}
