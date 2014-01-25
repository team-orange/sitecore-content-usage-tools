using ContentUsageTools.Pipelines;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentUsageTools.Helpers
{
    public static class ContentUsageToolsHelper
    {
        /// <summary>
        /// These websites should not be used when resolving what website an item belongs to.
        /// </summary>
        private readonly static string[] ExcludeSites = new[] { "shell", "modules_shell", "modules_website" };

        /// <summary>
        /// Return a list of item which is linked to the item in parameters and exclude all the item 
        /// which are not in the content or the media library
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<Item> GetLinkedItemsInContentAndMediaLibrary(Item item)
        {
            var links = Globals.LinkDatabase.GetReferrers(item);
            var referenceItemList = new List<Item>();
            referenceItemList.AddRange(
                links.Where(itemLink => IsInContentOrMediaLibrary(itemLink.GetSourceItem())).Select(itemLink => itemLink.GetSourceItem()));


            return referenceItemList.Distinct(new ItemEqualityComparer());
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

        public static bool IsInContentOrMediaLibrary(Item item)
        {
            return item.Paths.IsMediaItem || item.Paths.IsContentItem;
        }

        /// <summary>
        /// Check if an item is UnUsed 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsUnused(Item item)
        {
            return !GetLinkedItemsInContentAndMediaLibrary(item).Any();
        }

        /// <summary>
        /// Retrieves the siteinfo belonging to the item path.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The siteinfo.</returns>
        public static SiteInfo GetCorrectSite(string itemPath)
        {
            return Factory.GetSiteInfoList().FirstOrDefault(
                info =>
                {
                    if (ExcludeSites.Contains(info.Name.ToLowerInvariant()))
                    {
                        return false;
                    }
                    string startItem1 = info.RootPath + info.StartItem;
                    Item item1 = Context.ContentDatabase.GetItem(startItem1);
                    if (item1 == null)
                    {
                        return false;
                    }
                    return itemPath.StartsWith(item1.Paths.FullPath, StringComparison.OrdinalIgnoreCase);
                });
        }

        /// <summary>
        /// Return a list of itemId serparated by a pipe "|" which is linked to the item in parameters
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetLinkedItemsID(Item item)
        {

            var links = Globals.LinkDatabase.GetReferrers(item);
            var sbId = new StringBuilder();
            links.ForEach(it => sbId.AppendFormat("{0}|", it.SourceItemID));
            if (sbId.Length > 0)
            {
                sbId.Remove(sbId.Length - 1, 1);
            }
            return sbId.ToString();
        }
    }

    #region IEqualityComparer Implementations
    public class ItemEqualityComparer : IEqualityComparer<Item>
    {
        public bool Equals(Item x, Item y)
        {
            return x.ID.Equals(y.ID);
        }

        public int GetHashCode(Item obj)
        {
            return obj.ID.GetHashCode();
        }
    }
    #endregion
}