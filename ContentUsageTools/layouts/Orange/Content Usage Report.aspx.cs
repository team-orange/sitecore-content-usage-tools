using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ContentUsageTools.Helpers;
using Sitecore.Configuration;
using Sitecore.Data.Clones;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Web.UI.XslControls;

namespace ContentUsageTools.layouts.Orange
{
    public partial class Content_Usage_Report : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            UnusedReport.Text = Translate.Text("Unused Report");
            UsedMultipleTimesReport.Text = Translate.Text("Multiple Time Report");
            PathLabel.Text = Translate.Text("Path Label");
            GenerateReport.Text = Translate.Text("Report Me !");
            CvTypeOfReport.Text = Translate.Text("You must choose one of the report");
            if (!Page.IsPostBack)
            {
                UnusedReport.Checked = true;
                RequiredPath.Text = Translate.Text("Please enter a path");
            }

        }

        protected void UsedByRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                  //<asp:Literal runat="server" ID="ItemNameUsedBy" />
                  //                                      -
                  //                                  <asp:HyperLink runat="server" ID="WatchItem" />

                var item = e.Item.DataItem as Item;
                var itemNameUsedBy = e.Item.FindControl("ItemNameUsedBy") as Literal;
                var watchItem = e.Item.FindControl("WatchItem") as HyperLink;
                itemNameUsedBy.Text = item.Name;
                watchItem.Text = Translate.Text("Watch the Item");
                
                watchItem.NavigateUrl = LinkManager.GetItemUrl(item, new UrlOptions()
                {
                    SiteResolving = true,
                    
                });
            }
        }

        protected void ResultPanel_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (UsedMultipleTimesReport.Checked)
                {

                    var item = e.Item.DataItem as test;

                    var panUsedBy = e.Item.FindControl("PanUsedBy") as Panel;
                    panUsedBy.Visible = true;
                    var usedByRepeater = e.Item.FindControl("UsedByRepeater") as Repeater;
                    usedByRepeater.DataSource = item.Items;
                    usedByRepeater.DataBind();
                    var itemName = e.Item.FindControl("ItemName") as Literal;
                    itemName.Text = item.Name;
                }
                else
                {
                    var item = e.Item.DataItem as Item;
                    var itemName = e.Item.FindControl("ItemName") as Literal;
                    itemName.Text = item.Name;
                }

            }

            //throw new NotImplementedException();
        }

        protected void GenerateReport_OnClick(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                List<Item> result = new List<Item>();
                var path = Factory.GetDatabase("master").SelectItems(PathTextBox.Text);
                if (UnusedReport.Checked)
                {

                    result = path.Where(item => ContentUsageToolsHelper.IsUnused(item)).ToList();
                    ResultRepeater.DataSource = result;
                    ResultRepeater.DataBind();
                }
                else
                {
                    var test = new List<test>();
                    foreach (var item in path)
                    {
                        var linkedItems = ContentUsageToolsHelper.GetLinkedItems(item).ToList();
                        var containsMultipleReference = linkedItems.Any();
                        if (containsMultipleReference)
                        {
                            test.Add(new test()
                            {
                                Name = item.Name,
                                Items = linkedItems
                            });
                        }
                        ResultRepeater.DataSource = test;
                        ResultRepeater.DataBind();
                    }

                    //result = path.Where(item => ContentUsageToolsHelper.GetLinkedItems(item).Any()).Select(x => new
                    //{
                    //    Item = x,
                    //    Items = item

                    //}).ToList();
                }



            }
        }

        protected void ValidateTypeOfReport(object source, ServerValidateEventArgs args)
        {
            args.IsValid = UnusedReport.Checked || UsedMultipleTimesReport.Checked;
        }

        public class test
        {
            public string Name { get; set; }
            public IEnumerable<Item> Items { get; set; }
        }
    }
}