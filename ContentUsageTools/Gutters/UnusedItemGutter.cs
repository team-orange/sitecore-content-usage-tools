using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor.Gutters;

namespace ContentUsageTools.Gutters
{
    /// <summary>
    /// Render a gutter icon next to the item in the Content Editor tree when an item is never used
    /// </summary>
    public class UnusedItemGutter : GutterRenderer
    {
        /// <summary>
        /// Represents a GutterIconDescriptor
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            if(ContentUsageToolsHelper

            return null;
        }
    }
}