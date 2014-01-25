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

namespace ContentUsageTools.Common
{
    /// <summary>
    /// Provides commonly used constants in the solution.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Provides config related constants (referring to the ContentUsageTools.config patch file).
        /// </summary>
        public static class Config
        {
            public static string ExcludedContentTemplateIdsSetting = "ExcludedContentTemplateIds";
            public static string ExcludedWebsiteNamesSetting = "ExcludedWebsiteNames";
        }
    }
}