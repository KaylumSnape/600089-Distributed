using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    /*
    A socket is simply the logical endpoint of a communications channel and provides a mechanism
    for creating a virtual connection between processes on the same or different computers. Every
    socket has a unique socket address which consists of a combination of the local computer’s
    network address and a port number.
    A port is a logical communication channel with a unique port number; port numbers are used to
    differentiate between logical channels on the same computer.
    A single computer can have up to 65535 ports (i.e. up to 65535 logical communication channels).
    Any value in the range 0 to 65535 is valid, but ports 0 through to 1024
    are normally reserved by the operating system for specific applications and it is normal practice
    to choose a port number in the range 1025 – 65535 for non-standard applications.
    */
    class Program
    {
        // The TcpListener class implements a server socket which simply listens for incoming
        // connections. When we create a TcpListener we need to specify the IP address and port number on the
        // local machine with which the TcpListener should be associated.
        // ServerSocket.
        static readonly TcpListener TcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);

        static void Main(string[] args)
        {
            TcpListener.Start(); // Start listening.
            // Wait for an incoming connection.
            var tcpClient = TcpListener.AcceptTcpClient(); // Only returns when a new connection is established.
            var networkStream = tcpClient.GetStream(); // Get I/O stream associated with client instance.
            var message = ReadFromStream(networkStream); // Read the message off the stream.
            Console.WriteLine("Received: \"" + message + "\"");
            var translatedMessage = Translate(message); // Translate the message.
            var response = Serialise(translatedMessage); // Serialise the translated message.
            networkStream.Write(response, 0, response.Length); // Write message to the stream.
            Console.ReadKey(); // Wait for keypress before exit.
        }

        static string Translate(string message)
        {
            var translatedMessage = new StringBuilder();
            const string vowels = "aeiou";
            var words = message.Split(' ');
            foreach (var word in words)
            {
                // Perform translation.
                // If first letter of word is a vowel.
                if (vowels.Contains(word[0].ToString().ToLower()))
                {
                    translatedMessage.Append(word + "way ");
                }
                else // Check consecutive letters in word for vowel.
                {
                    var i = 0;
                    while (!vowels.Contains(word[i].ToString().ToLower()) && i < word.Length - 1)
                    {
                        i++;
                    }

                    var consonantsAtFront = word.Substring(0, i);
                    var restOfWord = word[i..]; // Range operator, go from i to end.
                    translatedMessage.Append(restOfWord + consonantsAtFront + "ay "); // Re construct new word.
                }
            }
            return translatedMessage.ToString().Trim(); // Trim whitespace off the end.
        }

        // Read bytes off stream and return in ASCII.
        static string ReadFromStream(NetworkStream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            // First byte identifies how long the preceding message is.
            var messageLengthInBytes = new byte[1]; // Buffer to store data, size is 1 byte as stated.            
            stream.Read(messageLengthInBytes, 0, 1); // Using the buffer, read in the first byte of the stream to it.
            // Now we know how long the message is, create a byte array of that size to store the message.
            var messageBytes = new byte[messageLengthInBytes[0]];
            // messageBytes is the buffer where we store the data. 
            // offset is the location in the buffer to start saving the data.
            // messageLengthBytes[0] has how long the message is, its size, stored from earlier.
            stream.Read(messageBytes, 0, messageLengthInBytes[0]); // Read in the message.
            return Encoding.ASCII.GetString(messageBytes); // Return decoded bytes into ASCII.
        }

        // Serialise the response.
        static byte[] Serialise(string response)
        {
            var responseBytes = Encoding.ASCII.GetBytes(response);
            var responseLength = (byte)responseBytes.Length;
            var responseLengthBytes = BitConverter.GetBytes(responseLength);
            var rawData = new byte[responseLength + 1];
            responseLengthBytes.CopyTo(rawData, 0);
            responseBytes.CopyTo(rawData, 1);
            return rawData;
        }
    }
}