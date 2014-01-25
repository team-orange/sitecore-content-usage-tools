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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace ContentUsageTools.Commands
{
    public class DuplicateAndRelinkCommand : WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context == null
                || context.Items == null
                || !context.Items.Any())
            {
                return;
            }
            
            Item datasourceItem = context.Items[0];
            if (! ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(context.Items.First()).Any())
            {
                SheerResponse.Alert("The associated item is not being used anywhere else, so you don't have to 'duplicate and relink'.");
                return;
            }
            NameValueCollection parameters = new NameValueCollection();
            parameters["id"] = datasourceItem.ID.ToShortID().ToString();
            
            // Find a suitable proposed name
            int number = 2;
            string proposedName = string.Format("{0} {1}", datasourceItem.Name, number);
            while (datasourceItem.Parent.Axes.GetChild(proposedName) != null)
            {
                number++;
                proposedName = string.Format("{0} {1}", datasourceItem.Name, number);
            }
            parameters["proposedname"] = proposedName;

            Sitecore.Context.ClientPage.Start(this, "DuplicateAndRelink", parameters);
        }

        protected void DuplicateAndRelink(ClientPipelineArgs args)
        {
            if (args.IsPostBack)
            {
                if (args.HasResult && ! string.IsNullOrWhiteSpace(args.Result))
                {

                }
            }
            else
            {
                Sitecore.Context.ClientPage.ClientResponse.Input(
                    "Please provide a name for the duplicated item that will then be linked to this component",
                    args.Parameters["proposedname"]);
                args.WaitForPostBack();
            }
        }
    }
}