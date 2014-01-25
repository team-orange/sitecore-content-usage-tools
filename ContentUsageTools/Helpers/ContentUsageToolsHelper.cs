using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Pipelines;
using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace ContentUsageTools.Helpers
{
    public static class ContentUsageToolsHelper
    {
        /// <summary>
        /// Return a list of item which is linked to the item in parameters
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<Item> GetLinkedItems(Item item)
        {
            return null;
        }

        /// <summary>
        /// Check if an item is considered a page.
        /// By default, this checks to see if the page has presentation settings defined.
        /// Please refer to the <contentusagetools.determineifpage /> pipeline to extend this function.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsPage(Item item)
        {
            // Call the pipeline contentusagetools.determineifpage
            DetermineIfPagePipelineArgs args = new DetermineIfPagePipelineArgs(item);
            CorePipeline.Run("contentusagetools.determineifpage", args);
            return args.IsPage;
        }

        /// <summary>
        /// Check if an item is UnUsed 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsUnused(Item item)
        {
            // TODO : Call the LinkedItem check if it's empty
            return true;
        }




    }
}