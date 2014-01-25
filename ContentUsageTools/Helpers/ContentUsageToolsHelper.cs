/* Copyright (C) 2014 Dražen Janjiček, Johann Baziret, Robin Hermanussen

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program. If not, see http://www.gnu.org/licenses/. */

namespace ContentUsageTools.Helpers
{
    using ContentUsageTools.Pipelines;
    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.ContentSearch.Utilities;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Pipelines;
    using Sitecore.Sites;
    using Sitecore.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Common = ContentUsageTools.Common;

    /// <summary>
    /// Helper class for shared functionality.
    /// </summary>
    public static class ContentUsageToolsHelper
    {
        /// <summary>
        /// These websites should not be used when resolving what website an item belongs to.
        /// </summary>
        private static string[] ExcludeSites
        {
            get
            {
                return ConfigurationHelper.GetSettingAsArray(Common.Constants.Config.ExcludedWebsiteNamesSetting);
            }
        }

        /// <summary>
        /// Return a list of item which is linked to the item in parameters and exclude all the item 
        /// which are not in the content or the media library
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The linked items.</returns>
        public static IEnumerable<Item> GetLinkedItemsInContentAndMediaLibrary(Item item)
        {
            Assert.IsNotNull(item, "item");

            ItemLink[] links = Globals.LinkDatabase.GetReferrers(item);
            List<Item> referenceItemList = new List<Item>();

            referenceItemList.AddRange(links
                .Where(itemLink => IsInContentOrMediaLibrary(itemLink.GetSourceItem()))
                .Select(itemLink => itemLink.GetSourceItem())
            );

            return referenceItemList.Distinct(new ItemEqualityComparer());
        }

        /// <summary>
        /// Check if an item is considered a page.
        /// By default, this checks to see if the page has presentation settings defined.
        /// Please refer to the <contentusagetools.determineifpage /> pipeline to extend this function.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true|false</returns>
        public static bool IsPage(Item item)
        {
            DetermineIfPagePipelineArgs args = new DetermineIfPagePipelineArgs(item);
            CorePipeline.Run("contentusagetools.determineifpage", args);

            return args.IsPage;
        }

        /// <summary>
        /// Determines whether [is in content or media library] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>true|false</returns>
        public static bool IsInContentOrMediaLibrary(Item item)
        {
            return (item.Paths.IsMediaItem || item.Paths.IsContentItem);
        }

        /// <summary>
        /// Check if the item is in the core database
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsInCoreDatabase(Item item)
        {
            return item.Database.Equals(Factory.GetDatabase("core"));
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
        /// Resolves the url for the item to the correct site and returns the url.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The page URL.</returns>
        public static string GetResolvedPageUrl(Item item)
        {
            SiteInfo site = GetCorrectSite(item.Paths.FullPath) ?? Context.Site.SiteInfo;

            using (new SiteContextSwitcher(new SiteContext(site)))
            {
                return LinkManager.GetItemUrl(item);
            }
        }

        /// <summary>
        /// Retrieves the siteinfo belonging to the item path.
        /// </summary>
        /// <param name="itemPath"></param>
        /// <returns>The siteinfo.</returns>
        private static SiteInfo GetCorrectSite(string itemPath)
        {
            return Factory.GetSiteInfoList().FirstOrDefault(
                info =>
                {
                    if (ExcludeSites.Contains(info.Name.ToLowerInvariant()))
                    {
                        return false;
                    }

                    string startItem1 = info.RootPath + info.StartItem;
                    Item item = Context.ContentDatabase.GetItem(startItem1);

                    if (item == null)
                    {
                        return false;
                    }

                    return itemPath.StartsWith(item.Paths.FullPath, StringComparison.OrdinalIgnoreCase);
                });
        }

        /// <summary>
        /// Return a list of itemId serparated by a pipe "|" which is linked to the item in parameters
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetLinkedItemsID(Item item)
        {
            ItemLink[] links = Globals.LinkDatabase.GetReferrers(item);
            StringBuilder sb = new StringBuilder();

            foreach (var itemLink in links)
            {
                if (!sb.ToString().Contains(itemLink.SourceItemID.ToString()))
                {
                    sb.AppendFormat("{0}|", itemLink.SourceItemID);
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}