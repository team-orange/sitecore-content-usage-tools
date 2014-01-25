using ContentUsageTools.Helpers;
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
                return;

            Item current = context.Items.FirstOrDefault();

            if(current != null)
            {
                this.RetrieveReferences(current);
            }
        }

        private void RetrieveReferences(Item current)
        {
            List<Item> references = ContentUsageToolsHelper.GetLinkedItems(current).ToList();

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