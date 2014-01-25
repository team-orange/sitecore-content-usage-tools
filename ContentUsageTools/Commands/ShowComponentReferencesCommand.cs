using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
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
            IEnumerable<Item> references = ContentUsageToolsHelper.GetLinkedItems(current);

            if(references != null && references.Any())
            {
                List<string> urls = new List<string>();

                references.ToList().ForEach(item => urls.Add(String.Format("{0}|{1}", item.Paths.ContentPath, LinkManager.GetItemUrl(item))));

                SheerResponse.Eval("parent.showComponentReferences('" + String.Join(",", urls.ToArray()) + "')");  
            }
        }
    }
}