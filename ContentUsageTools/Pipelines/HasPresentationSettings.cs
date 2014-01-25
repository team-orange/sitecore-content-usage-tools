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
    using Sitecore.Diagnostics;

    /// <summary>
    /// Check to see if the current item has presentation settings
    /// </summary>
    public class HasPresentationSettings
    {
        /// <summary>
        /// Processes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public virtual void Process(DetermineIfPagePipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.Item != null)
            {
                args.IsPage = (args.Item.Visualization.Layout != null);
            }
        }
    }
}