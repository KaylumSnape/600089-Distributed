using System;
using System.Globalization;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class TimestampFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            /* Unlike culture-sensitive data, The CultureInfo.InvariantCulture property is used if
            you are formatting or parsing a string that should be parseable by a piece of software
            independent of the user's local settings.*/
            message.Headers.Add("Timestamp", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            return message;
        }
    }
}