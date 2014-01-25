using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines.GetContentEditorWarnings;
using Sitecore.Web.UI.Sheer;

namespace ContentUsageTools.Pipelines
{
    public class ContentEditorContentUsageMessage
    {
        public void Process(GetContentEditorWarningsArgs args)
        {
            if (args.Item == null)
            {
                return;
            }
            
            GetContentEditorWarningsArgs.ContentEditorWarning
			contentEditorWarning = args.Add();
            contentEditorWarning.Title = Translate.Text("TODO: display stuff here.");
            contentEditorWarning.AddOption(Translate.Text("TODO: Perhaps navigate"), "contenteditorusage:navigate");
        }
    }
}