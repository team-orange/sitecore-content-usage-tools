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

namespace ContentUsageTools.ComputedField
{
    using ContentUsageTools.Helpers;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.ComputedFields;
    using Sitecore.Data.Items;

    /// <summary>
    /// This class serves the linked items from the search index.
    /// </summary>
    public class LinkedItems : IComputedIndexField
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the type of the return.
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// Computes the field value.
        /// </summary>
        /// <param name="indexable">The indexable.</param>
        /// <returns>The value.</returns>
        public object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            
            if (item == null )
                return null;
            
            return ContentUsageToolsHelper.IsInContentOrMediaLibrary(item) ? ContentUsageToolsHelper.GetLinkedItemsID(item) : null;
        }
    }
}