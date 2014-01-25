using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContentUsageTools.Helpers;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace ContentUsageTools.layouts.Orange
{
    public partial class Content_Usage_Report : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            UnusedReport.Text = Translate.Text("Unused Report");
            UsedMultipleTimesReport.Text = Translate.Text("Multiple Time Report");
            PathLabel.Text = Translate.Text("Path Label");


        }

        protected void UnusedClick(object sender, EventArgs e)
        {

        }

        protected void UseMultipleTimesClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        protected void UsedByRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void ResultPanel_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected void GenerateReport_OnClick(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                IEnumerable<Item> result; 
                if (UnusedReport.Checked)
                {
                    result = new List<Item>();
                    result = ContentUsageToolsHelper.GetLinkedItems();
                }
            }
        }

        protected void ValidateTypeOfReport(object source, ServerValidateEventArgs args)
        {
            args.IsValid = UnusedReport.Checked || UsedMultipleTimesReport.Checked;
        }
    }
}