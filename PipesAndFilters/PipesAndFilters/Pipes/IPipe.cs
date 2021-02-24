using PipesAndFilters.Filters;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Pipes
{
    public interface IPipe
    {
        public void RegisterFilter(IFilter iFilter);
        public IMessage ProcessMessage(IMessage iMessage);

    }
}