using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor;
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
            GutterIconDescriptor descriptor = null;

            if (!ContentUsageToolsHelper.IsPage(item) && ContentUsageToolsHelper.IsUnused(item))
            {
                descriptor = new GutterIconDescriptor();
                descriptor.Icon = "Control/32x32/bar_hor_d.png"; //Remove the Hardcodede Path - Settings 
                descriptor.Tooltip = "This item is unused, you can probably delete it"; //Translation 
            }

            return descriptor;
        }
    }
}