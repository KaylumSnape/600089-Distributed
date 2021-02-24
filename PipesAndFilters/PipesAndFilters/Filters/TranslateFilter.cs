using System.Text;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class TranslateFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            // Look for "RequestFormat" header in message.
            // If key found, encode from bytes delimited by dashes to requested format.
            if (message.Headers.TryGetValue("RequestFormat", out var request))
            {
                // Translate the body of the message from the given format (bytes delimited by dashes) to an ASCII string.
                string[] byteStrings = message.Body.Split('-');
                byte[] bytes = new byte[byteStrings.Length];

                for (int i = 0; i < byteStrings.Length; i++)
                {
                    bytes[i] = byte.Parse(byteStrings[i]);
                }

                message.Body = Encoding.ASCII.GetString(bytes);
            }

            // Look for "ResponseFormat" header in message.
            // If key found, decode to string.
            if (message.Headers.TryGetValue("ResponseFormat", out var response))
            {
                // Turn the string of text into bytes.
                byte[] bytes = Encoding.ASCII.GetBytes(message.Body); //
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
            return message;
        }
    }
}