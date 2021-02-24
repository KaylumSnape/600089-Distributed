using System;
using System.Collections.Generic;
using System.Text;

namespace PipesAndFilters.Messages
{
    public interface IMessage
    {
        public Dictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
    }
}
