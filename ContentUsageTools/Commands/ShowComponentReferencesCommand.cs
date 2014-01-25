﻿using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            ItemLink[] references = Sitecore.Globals.LinkDatabase.GetItemReferrers(current, false);

            if(references != null && references.Any())
            {

            }
        }
    }
}