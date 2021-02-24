using System;
using System.Collections.Generic;
using System.Linq;
using PipesAndFilters.Filters;
using PipesAndFilters.Messages;

namespace PipesAndFilters.Pipes
{
    public class Pipe : IPipe
    {
        private List<IFilter> Filters;

        public Pipe()
        { }

        public Pipe(List<IFilter> filters)
        {
            Filters = filters;
        }

        public void RegisterFilter(IFilter iFilter)
        {
            Filters.Add(iFilter); // Add the supplied iFilter parameter to the end of the Filters List.
        }

         
        public IMessage ProcessMessage(IMessage iMessage)
        {
            // Sequentially pass the supplied IMessage through each filter in the Filters List.
            foreach (var filter in Filters)
            {
                filter.Run(iMessage);
            }

            // Don't think this is returning the message that's been through the filter.
            // Return the message passed back by the final filter.
            return iMessage;
        }
    }
}