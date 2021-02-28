using PipesAndFilters.Messages;

namespace PipesAndFilters
{
    public class UpdateUserEndpoint : IEndpoint
    {
        public IMessage Execute(IMessage message)
        {
            Message response = new Message();
            response.Body = $"Updated {ServerEnvironment.CurrentUser.Name} with the message: {message.Body}";
            response.Headers.Add("ResponseFormat", message.Headers["RequestFormat"]);
            return response;
        }
    }
}