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

namespace ContentUsageTools.Gutters
{
    using ContentUsageTools.Helpers;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Shell.Applications.ContentEditor.Gutters;
    using System;
    using System.Linq;

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
            Assert.IsNotNull(item, "item");
            GutterIconDescriptor descriptor = null;

            if (this.Evaluate(item))
            {
                descriptor = new GutterIconDescriptor
                {
                    Icon = "Control/32x32/bar_hor_d.png",
                    Tooltip = Translate.Text("This item is not being used, click to delete it"),
                    Click = String.Format("item:delete(id={0})", item.ID)
                };
            }

            return descriptor;
        }

        private bool Evaluate(Item current)
        {
            bool isPage = ContentUsageToolsHelper.IsPage(current);
            bool isUnused = ContentUsageToolsHelper.IsUnused(current);
            bool isValidContent = ContentUsageToolsHelper.IsInContentOrMediaLibrary(current);
            bool isStandardFields = current.Fields.All(f => f.Name.StartsWith("_"));

            return (!isPage && isUnused && isValidContent && !isStandardFields);
        }
    }
}