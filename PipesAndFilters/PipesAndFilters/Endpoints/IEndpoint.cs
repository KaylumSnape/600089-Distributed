using PipesAndFilters.Messages;

namespace PipesAndFilters
{
    public interface IEndpoint
    {
        public IMessage Execute(IMessage message);
    }
}