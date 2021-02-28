using System;
using System.Collections.Generic;
using System.Text;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class TranslateFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            // Look for "RequestFormat" header in message.
            if (message.Headers.TryGetValue("RequestFormat", out var requestFormat))
            {
                // Decode from request format to string.
                switch (requestFormat)
                {
                    case "Bytes":
                        BytesToString(message);
                        break;
                    case "Binary":
                        BinaryToString(message);
                        break;
                    case "Hex":
                        HexToString(message);
                        break;
                }
            }

            // Look for "ResponseFormat" header in message.
            if (message.Headers.TryGetValue("ResponseFormat", out var responseFormat))
            {
                // Encode from string to response format.
                switch (responseFormat)
                {
                    case "Bytes":
                        StringToBytes(message);
                        break;
                    case "Binary":
                        StringToBinary(message);
                        break;
                    case "Hex":
                        StringToHex(message);
                        break;
                }
            }

            return message;
        }

        private void StringToHex(IMessage message)
        {
            var sb = new StringBuilder();
            var bytes = Encoding.Unicode.GetBytes(message.Body);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }
            message.Body = sb.ToString();
        }
        
        private void HexToString(IMessage message)
        {
            var bytes = new byte[message.Body.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(message.Body.Substring(i * 2, 2), 16);
            }
            message.Body = Encoding.Unicode.GetString(bytes);
        }

        private void BinaryToString(IMessage message)
        {
            List<Byte> byteList = new List<Byte>();
            for (int i = 0; i < message.Body.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(message.Body.Substring(i, 8), 2));
            }
            message.Body = Encoding.ASCII.GetString(byteList.ToArray());
        }

        // Translate the body of the message from bytes delimited by dashes to string.
        private static void BytesToString(IMessage message)
        {
            string[] byteStrings = message.Body.Split('-');
            byte[] bytes = new byte[byteStrings.Length];

            for (int i = 0; i < byteStrings.Length; i++)
            {
                bytes[i] = byte.Parse(byteStrings[i]);
            }

            message.Body = Encoding.ASCII.GetString(bytes);
        }

        // Translate the body of the message from string to bytes delimited by dashes.
        private static void StringToBytes(IMessage message)
        {
            // Turn the string of text into bytes.
            byte[] bytes = Encoding.ASCII.GetBytes(message.Body);
            string messageBody = "";
            // Add the dashes in-between the bytes.
            for (int i = 0; i < bytes.Length; i++)
            {
                messageBody += bytes[i].ToString();
                if (i + 1 < bytes.Length)
                {
                    messageBody += "-";
                }
            }
            message.Body = messageBody;
        }

        // Translate the body of the message from string to binary.
        private void StringToBinary(IMessage message)
        {
            StringBuilder requestBody = new StringBuilder();
 
            foreach (char c in message.Body.ToCharArray())
            {
                requestBody.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            message.Body = requestBody.ToString();
        }
    }
}