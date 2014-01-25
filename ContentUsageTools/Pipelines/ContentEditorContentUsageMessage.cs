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
    using ContentUsageTools.Helpers;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Pipelines.GetContentEditorWarnings;
    using System;
    using System.Linq;

    /// <summary>
    /// Shows a message in the content editor to display on which pages the item is being used as a datasource.
    /// </summary>
    public class ContentEditorContentUsageMessage
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            Assert.IsNotNull(args, "args");

            if (args.Item == null || ContentUsageToolsHelper.IsPage(args.Item) || args.Item.Fields.All(f => f.Name.StartsWith("_"))) 
            {
                return;
            }

            if (ContentUsageToolsHelper.IsUnused(args.Item))
            {
                // Display a message suggesting to remove this item
                GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning = args.Add();
                contentEditorWarning.Title = Translate.Text("This item is not being used on any page.");
                contentEditorWarning.AddOption(Translate.Text("Delete this item"), "item:delete");
            }
            else
            {
                // Display what pages the item is being used on as a datasource, so you can easily navigate to them
                GetContentEditorWarningsArgs.ContentEditorWarning contentEditorWarning = args.Add();
                contentEditorWarning.Title = Translate.Text("This item is being used on the following pages:");
                
                foreach (Item linkedPage in ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(args.Item))
                {
                    string text = String.Format("Open {0} <i>({1})</i>", linkedPage.DisplayName, linkedPage.Paths.ContentPath);
                    string command = String.Format("item:load(id={0})", linkedPage.ID);
                    
                    contentEditorWarning.AddOption(Translate.Text(text), command);
                }
            }
        }
    }
}