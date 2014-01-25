using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;

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
            // TODO: implement
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context.Items != null
                && context.Items.Any()
                && ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(context.Items.First()).Any())
            {
                return base.QueryState(context);
            }
            return CommandState.Disabled;
        }
    }
}