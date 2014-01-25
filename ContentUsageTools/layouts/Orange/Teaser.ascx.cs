using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace ContentUsageTools.layouts.Orange
{
    public partial class Teaser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Sublayout current = this.Parent as Sublayout;

            if (current != null && !String.IsNullOrWhiteSpace(current.DataSource))
            {
                Item datasource = Sitecore.Context.Database.GetItem(current.DataSource);

                this.TitleField.Item = datasource;
                this.TextField.Item = datasource;
            }
        }
    }
}