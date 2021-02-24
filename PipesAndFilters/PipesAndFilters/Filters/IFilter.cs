using PipesAndFilters.Messages;

namespace PipesAndFilters.Filters
{
    public interface IFilter
    {
        public IMessage Run(IMessage message);
    }
}