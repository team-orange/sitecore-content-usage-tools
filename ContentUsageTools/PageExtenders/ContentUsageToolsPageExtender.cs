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

namespace ContentUsageTools.PageExtenders
{
    using Sitecore.Layouts;
    using Sitecore.Layouts.PageExtenders;
    using Sitecore.Sites;
    using Sitecore.Web;
    using Literal = Sitecore.Web.UI.HtmlControls.Literal;

    /// <summary>
    /// Inserts page editor contributions to the page.
    /// </summary>
    public class ContentUsageToolsPageExtender : PageExtender
    {
        public override void Insert()
        {
            SiteContext site = Sitecore.Context.Site;

            if (site != null && site.EnableWebEdit && site.DisplayMode == DisplayMode.Edit && WebUtil.GetQueryString("sc_webedit") != "0")
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