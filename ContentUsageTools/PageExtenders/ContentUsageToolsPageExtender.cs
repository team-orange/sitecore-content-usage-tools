using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Layouts;
using Sitecore.Layouts.PageExtenders;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Sites;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Literal = Sitecore.Web.UI.HtmlControls.Literal;

namespace ContentUsageTools.PageExtenders
{
    /// <summary>
    /// Inserts page editor contributions to the page.
    /// </summary>
    public class ContentUsageToolsPageExtender : PageExtender
    {
        public override void Insert()
        {
            SiteContext site = Sitecore.Context.Site;
            if ((site != null)
                && site.EnableWebEdit
                && site.DisplayMode == DisplayMode.Edit
                && WebUtil.GetQueryString("sc_webedit") != "0")
            {
                RenderingReference reference = new RenderingReference(
                    new Literal()
                        {
                            Text = "<script type=\"text/javascript\" src=\"/sitecore modules/Content Usage Tools/js/ContentUsageTools.js\"></script>"
                        });
                reference.Placeholder = "webedit";
                reference.AddToFormIfUnused = true;
                Sitecore.Context.Page.AddRendering(reference);
            }
        }
    }
}