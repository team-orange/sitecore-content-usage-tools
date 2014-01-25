using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Pipelines;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines;
using Sitecore.Sites;
using Sitecore.Web;

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
            var links = Globals.LinkDatabase.GetReferrers(item);
            var referenceItemList = new List<Item>();
            referenceItemList.AddRange(
                links.Select(itemLink => Factory.GetDatabase("master").GetItem(itemLink.SourceItemID)));

           
            return referenceItemList;
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
            return !GetLinkedItems(item).Any();
        }

        /// <summary>
        /// Retrieves the siteinfo belonging to the item path.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The siteinfo.</returns>
        public static SiteInfo GetCorrectSite(string itemPath)
        {
            SiteInfo siteInfo = Factory.GetSiteInfoList().Where(info => itemPath.StartsWith(info.RootPath + info.StartItem, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return siteInfo;
        }
    }
}