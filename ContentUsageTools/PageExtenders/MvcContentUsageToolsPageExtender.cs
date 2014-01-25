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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Pipelines.RenderPageExtenders;
using Sitecore.Sites;
using Sitecore.Web;

namespace ContentUsageTools.PageExtenders
{
    /// <summary>
    /// Inserts page editor contributions to the page for MVC solutions.
    /// </summary>
    public class MvcContentUsageToolsPageExtender : RenderPageExtendersProcessor
    {
        public override void Process(RenderPageExtendersArgs args)
        {
            SiteContext site = Sitecore.Context.Site;

            if ((((site != null) && ((site.DisplayMode == DisplayMode.Edit) && site.EnableWebEdit)) && ! Sitecore.Context.PageDesigner.IsDesigning) && (WebUtil.GetQueryString("sc_webedit") != "0"))
            {
                base.Process(args);
            }
        }

        protected override bool Render(TextWriter writer)
        {
            Assert.ArgumentNotNull(writer, "writer");
            const string jsInclude = "<script type=\"text/javascript\" src=\"/sitecore modules/Content Usage Tools/js/ContentUsageTools.js\"></script>";
            const string cssInclude = "<link href=\"/sitecore modules/Content Usage Tools/css/ContentUsageTools.css\" rel=\"stylesheet\" type=\"text/css\">";
            
            writer.Write(jsInclude);
            writer.Write(cssInclude);
            return true;
        }
    }
}