using System.Text;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class TranslateFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            // Look for "RequestFormat" header in message.
            // If key found, encode to bytes delimited by dashes.
            if (message.Headers.TryGetValue("RequestFormat", out var request))
            {
                // translate the body of the message from the given
                // format to an ASCII string.
                byte[] bytes = Encoding.ASCII.GetBytes(request);
                string requestBody = "";
                for (int i = 0; i < bytes.Length; i++)
                {
                    requestBody += bytes[i].ToString();
                    if (i + 1 < bytes.Length)
                    {
                        requestBody += "-";
                    }
                }
            }

            // Look for "ResponseFormat" header in message.
            // If key found, decode to string.
            string responseBody = "";
            if (message.Headers.TryGetValue("RequestFormat", out var response))
            {
                // Turn the delimited string of bytes to a byte array and then to an ASCII string
                string[] byteStrings = response.Split('-');
                byte[] bytes = new byte[byteStrings.Length];
                for (int i = 0; i < byteStrings.Length; i++)
                {
                    bytes[i] = byte.Parse(byteStrings[i]);
                }
                responseBody = Encoding.ASCII.GetString(bytes);
            }
            return message;
        }
    }
}