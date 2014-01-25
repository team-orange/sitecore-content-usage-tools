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

namespace ContentUsageTools.Fields.Editor
{
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Layouts;
    using Sitecore.Shell.Applications.ContentEditor;
    using System.Collections.Generic;

    /// <summary>
    /// Custom field allowing to select an existing placeholder from the selected layout.
    /// </summary>
    public class PlaceholderSelectionField : ValueLookupEx
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceholderSelectionField"/> class.
        /// </summary>
        public PlaceholderSelectionField()
        {
            this.ValueName = "Placeholder Key";
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="current">The current.</param>
        /// <returns></returns>
        protected override Item[] GetItems(Item current)
        {
            Assert.ArgumentNotNull(current, "current");
            return this.GetAvailablePlaceholders(current);
        }

        private Item[] GetAvailablePlaceholders(Item current)
        {
            List<Item> virtualItems = new List<Item>();
            Item root = Factory.GetDatabase("master").GetItem("/sitecore/layout/Placeholder Settings");

            // {17717CE5-3ABB-493A-A1CC-7BF0244B60F8} <- Page A item

            RenderingReference[] configuredPlaceholders = current.Visualization.GetRenderings(DeviceItem.ResolveDevice(Factory.GetDatabase("master")), false);


            foreach (Item descendant in root.Axes.GetDescendants())
            {
                if (descendant.Template.Key != "placeholder")
                    continue;

                virtualItems.Add(descendant);
            }

            return virtualItems.ToArray();
        }
    }
}