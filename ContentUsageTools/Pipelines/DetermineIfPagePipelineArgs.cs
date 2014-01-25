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

namespace ContentUsageTools.Pipelines
{
    using Sitecore.Data.Items;
    using Sitecore.Pipelines;

    /// <summary>
    /// Holds information about the passed item whether it is a real page or not.
    /// </summary>
    public class DetermineIfPagePipelineArgs : PipelineArgs
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        public Item Item { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is page].
        /// </summary>
        public bool IsPage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetermineIfPagePipelineArgs"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public DetermineIfPagePipelineArgs(Item item)
        {
            this.Item = item;
        }
    }
}