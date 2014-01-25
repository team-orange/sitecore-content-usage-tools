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
    using Sitecore.Configuration;
    using System;

    /// <summary>
    /// Provides access to settings stored in Sitecore patch files.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets the setting.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The value of the setting.</returns>
        public static string GetSetting(string name)
        {
            return Settings.GetSetting(name, String.Empty);
        }

        /// <summary>
        /// Gets the setting as array.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns>The value of the setting as an array.</returns>
        public static string[] GetSettingAsArray(string name, char seperator = '|')
        {
            string settingValue = GetSetting(name);
            return (!String.IsNullOrWhiteSpace(settingValue)) ? settingValue.Split(new char[] { seperator }, StringSplitOptions.RemoveEmptyEntries) : new String[0];
        }
    }
}