using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public class AuthenticateFilter : IFilter
    {
        public IMessage Run(IMessage message)
        {
            if (message.Headers.TryGetValue("User", out var value))
            {
                ServerEnvironment.SetCurrentUser(int.Parse(value));
            }

        }
    }
}