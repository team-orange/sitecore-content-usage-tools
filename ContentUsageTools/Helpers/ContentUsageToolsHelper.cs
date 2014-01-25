using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace ContentUsageTools.Helpers
{
    public class ContentUsageToolsHelper
    {
        /// <summary>
        /// Return a list of item which is linked to the item in parameters
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<Item> GetLinkedItems(Item item)
        {
            return null;
        }

        /// <summary>
        /// Check if an item has a presentation
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasPresentation(Item item)
        {
            // TODO: Call the pipeline HasPresentationSettings
            return true;
        }

        /// <summary>
        /// Check if an item is UnUsed 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsUnused(Item item)
        {
            // TODO : Call the LinkedItem check if it's empty
            return true;
        }




    }
}