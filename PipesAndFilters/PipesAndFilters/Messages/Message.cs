using System.Collections.Generic;

namespace PipesAndFilters.Messages
{
    public class Message : IMessage
    {
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }

        public Message()
        { }

        public Message(Dictionary<string, string> headers, string body)
        {
            Headers = new Dictionary<string, string>();
            Body = body;
        }
    }
}