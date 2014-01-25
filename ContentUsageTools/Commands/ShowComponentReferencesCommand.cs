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
    public class ShowComponentReferencesCommand : WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            if (context == null)
            {
                return;
            }

            Item current = context.Items.FirstOrDefault();

            if(current != null)
            {
                RetrieveReferences(current);
            }
        }

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

        private void RetrieveReferences(Item current)
        {
            Item pageItem = GetPageItem();

            List<Item> references = ContentUsageToolsHelper.GetLinkedItems(current).ToList();
            if (pageItem != null)
            {
                references = references.Where(r => r.ID != pageItem.ID).ToList();
            }

            if(references != null && references.Any())
            {

                string[] urls = references.Select(item =>
                    {
                        SiteInfo site = ContentUsageToolsHelper.GetCorrectSite(item.Paths.FullPath) ?? Sitecore.Context.Site.SiteInfo;

                        using (new SiteContextSwitcher(new SiteContext(site)))
                        {
                            return String.Format("{0}|{1}", item.Paths.Path, LinkManager.GetItemUrl(item));
                        }
                    }).ToArray();

                SheerResponse.Eval("parent.showComponentReferences('" + String.Join(",", urls) + "')");  
            }
        }
    }
}