using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class AuthenticateFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            // Look for "User" header in message.
            // If key found, call SetCurrentUser with the value.
            if (message.Headers.TryGetValue("User", out var value))
            {
                ServerEnvironment.SetCurrentUser(int.Parse(value));
            }

            return message;
        }
    }
}