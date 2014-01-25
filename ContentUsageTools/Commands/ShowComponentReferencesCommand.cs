using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Sites;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ContentUsageTools.Commands
{
    /// <summary>
    /// Determine what other pages the selected rendering's content is being used on.
    /// </summary>
    public class ShowComponentReferencesCommand : WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context == null
                || context.Items == null
                || ! context.Items.Any())
            {
                return;
            }

            Item pageItem = GetPageItem();

            Item datasourceItem = context.Items[0];
            List<Item> references = ContentUsageToolsHelper.GetLinkedItems(datasourceItem).ToList();
            if (pageItem != null)
            {
                references = references.Where(r => r.ID != pageItem.ID).ToList();
            }
            
            string[] urls = references.Select(item =>
                {
                    SiteInfo site = ContentUsageToolsHelper.GetCorrectSite(item.Paths.FullPath) ?? Sitecore.Context.Site.SiteInfo;

                    using (new SiteContextSwitcher(new SiteContext(site)))
                    {
                        return String.Format("{0}|{1}", item.Paths.Path, LinkManager.GetItemUrl(item));
                    }
                }).ToArray();

            SheerResponse.Eval(string.Format("parent.showComponentReferences('{0}', '{1}')", datasourceItem.ID, String.Join(",", urls)));
        }

        /// <summary>
        /// Determine what page is currently being viewed in the page editor, so we can exclude it from the references list.
        /// </summary>
        /// <returns></returns>
        private static Item GetPageItem()
        {
            if (Sitecore.Context.Database != null 
                && HttpContext.Current != null
                && HttpContext.Current.Request.UrlReferrer != null)
            {
                SafeDictionary<string> qsParams = WebUtil.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query);
                if (qsParams != null && qsParams.ContainsKey("id") && qsParams.ContainsKey("db"))
                {
                    Database database = Database.GetDatabase(qsParams["db"]);
                    return database != null
                        ? database.GetItem(HttpUtility.UrlDecode(qsParams["id"]))
                        : null;
                }
            }
            return null;
        }
    }
}