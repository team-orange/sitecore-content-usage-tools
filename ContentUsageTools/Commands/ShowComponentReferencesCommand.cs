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

namespace ContentUsageTools.Commands
{
    using ContentUsageTools.Helpers;
    using Sitecore.Collections;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Applications.WebEdit.Commands;
    using Sitecore.Shell.Framework.Commands;
    using Sitecore.Web;
    using Sitecore.Web.UI.Sheer;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Determine what other pages the selected rendering's content is being used on.
    /// </summary>
    public class ShowComponentReferencesCommand : WebEditCommand
    {
        /// <summary>
        /// Executes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            
            if (context == null || context.Items == null || !context.Items.Any())
            {
                return;
            }

            Item pageItem = GetPageItem();

            Item datasourceItem = context.Items[0];
            List<Item> references = ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(datasourceItem).ToList();
            
            if (pageItem != null)
            {
                references = references.Where(r => r.ID != pageItem.ID).ToList();
            }

            string[] urls = references.Select(item => String.Format("{0}|{1}", item.Paths.ContentPath, ContentUsageToolsHelper.GetResolvedPageUrl(item))).ToArray();
            SheerResponse.Eval(string.Format("parent.showComponentReferences('{0}', '{1}')", datasourceItem.ID, String.Join(",", urls)));
        }

        private static Item GetPageItem()
        {
            Item pageItem = null;

            if (Sitecore.Context.Database != null && HttpContext.Current != null && HttpContext.Current.Request.UrlReferrer != null)
            {
                SafeDictionary<string> qsParams = WebUtil.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query);
                
                if (qsParams != null && qsParams.ContainsKey("id") && qsParams.ContainsKey("db"))
                {
                    Database database = Database.GetDatabase(qsParams["db"]);
                    pageItem = (database != null) ? database.GetItem(HttpUtility.UrlDecode(qsParams["id"])) : null;
                }
            }

            return pageItem;
        }
    }
}