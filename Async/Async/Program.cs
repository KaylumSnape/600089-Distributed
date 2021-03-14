using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {

            // Add code here to send requests

            Console.WriteLine("Execution finished");
            Console.ReadLine();
        }

        public static void SendRequest(string message, string endpoint)
        {
            // The use of the using blocks in this method ensure that the resources are disposed when no longer required
            using (TcpClient tcpClient = new TcpClient())
            {
                tcpClient.Connect("127.0.0.1", 5000);

                using (NetworkStream nStream = tcpClient.GetStream())
                {
                    byte[] request = SerializeRequest(message, endpoint);
                    nStream.Write(request, 0, request.Length);

                    RetrieveResponse(nStream);
                }
            }
        }

        public static void RetrieveResponse(NetworkStream nStream)
        {
            // The first part of the response is an integer which identifies how many bytes are in the proceeding message
            byte[] responseLengthBytes = new byte[4];
            nStream.Read(responseLengthBytes, 0, 4);
            int responseLength = BitConverter.ToInt32(responseLengthBytes);

            // The second part of the response is the returned message. We know the length of this message as above
            byte[] responseBytes = new byte[responseLength];
            nStream.Read(responseBytes, 0, responseLength);
            string response = Encoding.ASCII.GetString(responseBytes);

            Console.WriteLine(response);
        }

        public static byte[] SerializeRequest(string Message, string Endpoint)
        {
            int index = 0; // This will keep track of how many bytes are currently in our array, so we can add to the end

            //Convert our strings to byte arrays
            byte[] messageBytes = Encoding.ASCII.GetBytes(Message);
            byte[] endpointBytes = Encoding.ASCII.GetBytes(Endpoint);

            // Calculate the size of the byte array we will need to send this message.
            // +8 gives us space for two integers which will contain the length of the message and endpoint
            int MessageLength = messageBytes.Length + endpointBytes.Length + 8;

            // Create a byte array of the correct size to send over the socket
            byte[] rawData = new byte[MessageLength];

            // Get the number of bytes that the endpoint uses as an integer and turn this into a 4 byte array
            int endpointLength = endpointBytes.Length;
            byte[] endpointLengthBytes = BitConverter.GetBytes(endpointLength);
            // Append this to our rawData array and move the index position to the next free byte
            endpointLengthBytes.CopyTo(rawData, index);
            index += endpointLengthBytes.Length;

            // Append the endpoint string data to our rawData array and move the index position to the next free byte
            endpointBytes.CopyTo(rawData, index);
            index += endpointBytes.Length;

            // Get the number of bytes that the endpoint uses as an integer and turn this into a 4 byte array
            int messageLength = messageBytes.Length;
            byte[] messageLengthBytes = BitConverter.GetBytes(messageLength);
            // Append this to our rawData array and move the index position to the next free byte
            messageLengthBytes.CopyTo(rawData, index);
            index += messageLengthBytes.Length;

            // Append the message string data to our rawData array
            messageBytes.CopyTo(rawData, index);

            return rawData;
        }

    }
}
