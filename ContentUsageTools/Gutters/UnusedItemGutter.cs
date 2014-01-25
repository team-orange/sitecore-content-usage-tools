using System.Linq;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Globalization;
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

            if (! ContentUsageToolsHelper.IsPage(item)
                && ContentUsageToolsHelper.IsUnused(item) && ContentUsageToolsHelper.IsInContentOrMediaLibrary(item)
                && !item.Fields.All(f => f.Name.StartsWith("_")))
            {
                descriptor = new GutterIconDescriptor
                {
                    Icon = "Control/32x32/bar_hor_d.png",
                    Tooltip = Translate.Text("This item is unused, you can probably delete it")
                };
            }

            return descriptor;
        }
    }
}