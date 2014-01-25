using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ContentUsageTools.Helpers;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Diagnostics;

namespace ContentUsageTools.Reports
{
    public partial class ContentUsageReport : System.Web.UI.Page
    {

        public class ReportItem
        {
            private Item Item { get; set; }

            public ReportItem(Item item, bool searchWithIndex)
            {
                Item = item;
                SearchWithIndex = searchWithIndex;
            }

            public bool SearchWithIndex { get; set; }

            public string DisplayName
            {
                get { return Item.DisplayName; }
            }

            public string Path
            {
                get { return Item.Paths.ContentPath; }
            }

            private List<Item> referredItems;
            public List<Item> ReferredItems
            {
                get
                {
                    if (SearchWithIndex)
                    {
                        referredItems = new List<Item>();
                        using (IProviderSearchContext context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                        {
                            foreach (SearchResultItem source in context.GetQueryable<SearchResultItem>().Where(x => x.Paths.Contains(Item.ID)))
                            {
                                foreach (var linkedItem in source["LinkedItems"].Split('|').Where(x => !string.IsNullOrEmpty(x)))
                                {
                                    var item = Sitecore.Context.ContentDatabase.GetItem(linkedItem);
                                    if (ContentUsageToolsHelper.IsPage(item) && item.Fields.All(f => f.Name.StartsWith("_")))
                                    {
                                        referredItems.Add(item);
                                    }
                                }
                            }
                        }


                        return referredItems;
                    }
                    return referredItems ?? (referredItems = ContentUsageToolsHelper.GetLinkedItems(Item).ToList());
                }
            }

        }

        private List<ReportItem> referredItemsReportItems;
        public List<ReportItem> ReferredItemsReportItems
        {
            get
            {



                return referredItemsReportItems ??
                       (referredItemsReportItems = ReportItems.Where(item => !ContentUsageToolsHelper.IsUnused(item))
                                                              .Select(item => new ReportItem(item, UseIndex.Checked))
                                                              .Where(item => item.ReferredItems.Any())
                                                              .ToList());
            }
        }

        private List<ReportItem> unusedReportItems;
        public List<ReportItem> UnusedReportItems
        {
            get
            {
                return unusedReportItems ??
                       (unusedReportItems = ReportItems.Where(item => ContentUsageToolsHelper.IsUnused(item))
                                                              .Select(item => new ReportItem(item, UseIndex.Checked))
                                                              .ToList());
            }
        }

        private IEnumerable<Item> reportItems;
        private IEnumerable<Item> ReportItems
        {
            get
            {
                if (reportItems == null)
                {
                    Item root = RootItem;
                    reportItems = root.Axes.GetDescendants()
                               .Where(item => !ContentUsageToolsHelper.IsPage(item)
                                              && !item.Fields.All(f => f.Name.StartsWith("_")));
                }
                return reportItems;
            }
        }

        private Item RootItem
        {
            get { return Sitecore.Context.ContentDatabase.GetItem(PathTextBox.Text); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            UnusedReport.Text = Translate.Text("Unused Report");
            UsedMultipleTimesReport.Text = Translate.Text("Referred items report");
            PathLabel.Text = Translate.Text("Item start path");
            GenerateReport.Text = Translate.Text("Generate report");
            CvTypeOfReport.Text = Translate.Text("You must choose at least one of the reports");
            if (!Page.IsPostBack)
            {
                UseIndex.Checked = true;
                UnusedReport.Checked = true;
                RequiredPath.Text = Translate.Text("Please enter a valid path within Sitecore to start from");
            }
        }

        protected void GenerateReport_OnClick(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            Page.Validate();
            if (Page.IsValid && RootItem != null)
            {
                try
                {
                    rptResultsUnusedReport.Visible = rptResultsReferredItemsReport.Visible = false;
                    if (UnusedReport.Checked)
                    {
                        rptResultsUnusedReport.Visible = true;
                        rptResultsUnusedReport.DataSource = UnusedReportItems;
                        rptResultsUnusedReport.DataBind();
                    }
                    if (UsedMultipleTimesReport.Checked)
                    {
                        rptResultsReferredItemsReport.Visible = true;
                        rptResultsReferredItemsReport.DataSource = ReferredItemsReportItems;
                        rptResultsReferredItemsReport.DataBind();
                    }
                }
                catch (KeyNotFoundException keyNotFoundExc)
                {
                    string message = "An exception occurred during generating the report; this may be due to the index not being up to date. Try the \"Normal report\" or rebuild the search index.";
                    Log.Error(message, keyNotFoundExc, this);
                    ErrorLabel.Text = Translate.Text(message);
                }
                catch (Exception exc)
                {
                    string message = Translate.Text("An exception occurred during generating the report. The error has been logged.");
                    Log.Error(message, exc, this);
                    ErrorLabel.Text = Translate.Text(message);
                }
            }
        }

        protected void ValidateTypeOfReport(object source, ServerValidateEventArgs args)
        {
            args.IsValid = UnusedReport.Checked || UsedMultipleTimesReport.Checked;
        }

    }
}