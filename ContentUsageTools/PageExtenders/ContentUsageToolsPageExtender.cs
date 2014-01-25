using Sitecore.Layouts;
using Sitecore.Layouts.PageExtenders;
using Sitecore.Sites;
using Sitecore.Web;
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
                Literal js = new Literal()
                {
                    Text = "<script type=\"text/javascript\" src=\"/sitecore modules/Content Usage Tools/js/ContentUsageTools.js\"></script>"
                };

                Literal css = new Literal()
                {
                    Text = "<link href=\"/sitecore modules/Content Usage Tools/css/ContentUsageTools.css\" rel=\"stylesheet\" type=\"text/css\">"
                };

                this.InjectScript(js);
                this.InjectScript(css);
            }
        }

        private void InjectScript(Literal literal)
        {
            RenderingReference reference = new RenderingReference(literal)
            {
                Placeholder = "webedit",
                AddToFormIfUnused = true
            };

            Sitecore.Context.Page.AddRendering(reference);
        }
    }
}