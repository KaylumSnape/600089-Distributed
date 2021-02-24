using System;
using System.Net.Sockets;
using System.Text;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // When a TcpClient is created we must supply the network
            // address and port number of the server to which a connection is to be established.
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 5000); // Connect to a known port on the sever.
            var networkStream = tcpClient.GetStream(); // Get I/O stream associated with client instance.
            Console.WriteLine("Enter a message to be translated...");
            var message = Console.ReadLine();
            var request = Serialise(message);
            networkStream.Write(request, 0, request.Length); // Write data to the network connection.
            // TODO: Read response from stream and display to user
            var response = ReadFromStream(networkStream); // Read the message off the stream.
            Console.WriteLine($"Received: {response}");
            Console.ReadKey(); // Wait for keypress before exit
        }

        static string ReadFromStream(NetworkStream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            var messageLengthBytes = new byte[1]; // Buffer to store data, size is 1 byte.
            // First byte identifies how long the preceding message is.
            // Using the buffer, read in the first byte of the stream to it.
            stream.Read(messageLengthBytes, 0, 1); 
            // Now we know how long the message is, create a byte array of that size to store the message.
            var messageBytes = new byte[messageLengthBytes[0]];
            // messageBytes is the buffer where we store the data. 
            // offset is the location in the buffer to start saving the data.
            // messageLengthBytes[0] has how long the message is, its size, stored from earlier.
            stream.Read(messageBytes, 0, messageLengthBytes[0]); // Read in the message.
            return Encoding.ASCII.GetString(messageBytes); // Return decoded bytes into ASCII.
        }

        static byte[] Serialise(string request)
        {
            var responseBytes = Encoding.ASCII.GetBytes(request);
            var responseLength = (byte)responseBytes.Length;
            var responseLengthBytes = BitConverter.GetBytes(responseLength);
            var rawData = new byte[responseLength + 1];
            responseLengthBytes.CopyTo(rawData, 0);
            responseBytes.CopyTo(rawData, 1);
            return rawData;
        }
    }
}