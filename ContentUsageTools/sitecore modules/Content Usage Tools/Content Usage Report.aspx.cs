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

namespace ContentUsageTools.Reports
{
    using ContentUsageTools.Helpers;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.SearchTypes;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    /// <summary>
    /// Renders a report about the usage of items.
    /// </summary>
    public partial class ContentUsageReport : Page
    {
        private List<ReportItem> referredItemsReportItems;
        private List<ReportItem> unusedReportItems;
        private IEnumerable<Item> reportItems;

        /// <summary>
        /// Provides information about the specific report item.
        /// </summary>
        public class ReportItem
        {
            private List<Item> referredItems;

            /// <summary>
            /// Gets or sets the item.
            /// </summary>
            private Item Item { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether [search with index].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [search with index]; otherwise, <c>false</c>.
            /// </value>
            public bool SearchWithIndex { get; set; }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            public string DisplayName
            {
                get { return this.Item.DisplayName; }
            }

            /// <summary>
            /// Gets the path.
            /// </summary>
            public string Path
            {
                get { return this.Item.Paths.ContentPath; }
            }

            /// <summary>
            /// Gets the referred items.
            /// </summary>
            public List<Item> ReferredItems
            {
                get
                {
                    if (this.SearchWithIndex)
                    {
                        this.referredItems = new List<Item>();

                        using (IProviderSearchContext context = ContentSearchManager.GetIndex("sitecore_master_index").CreateSearchContext())
                        {
                            foreach (SearchResultItem source in context.GetQueryable<SearchResultItem>().Where(x => x.Paths.Contains(this.Item.ID) ))
                            {
                                foreach (string linkedItem in source["LinkedItems"].Split('|').Where(x => !String.IsNullOrEmpty(x)))
                                {
                                    Item item = Sitecore.Context.ContentDatabase.GetItem(linkedItem);

                                    if (ContentUsageToolsHelper.IsPage(item))
                                    {
                                        if (!this.referredItems.Contains(item,new ItemEqualityComparer()))
                                        {
                                            this.referredItems.Add(item);
                                        }
                                    }
                                }
                            }
                        }

                        return this.referredItems;
                    }

                    return this.referredItems ?? (this.referredItems = ContentUsageToolsHelper.GetLinkedItemsInContentAndMediaLibrary(this.Item).ToList());
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ReportItem"/> class.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <param name="searchWithIndex">if set to <c>true</c> [search with index].</param>
            public ReportItem(Item item, bool searchWithIndex)
            {
                this.Item = item;
                this.SearchWithIndex = searchWithIndex;
            }
        }

        /// <summary>
        /// Gets the referred items report items.
        /// </summary>
        public List<ReportItem> ReferredItemsReportItems
        {
            get
            {
                return this.referredItemsReportItems ??
                       (this.referredItemsReportItems = this.ReportItems.Where(item => !ContentUsageToolsHelper.IsUnused(item))
                                                                        .Select(item => new ReportItem(item, UseIndex.Checked))
                                                                        .Where(item => item.ReferredItems.Any())
                                                                        .ToList());
            }
        }

        /// <summary>
        /// Gets the unused report items.
        /// </summary>
        public List<ReportItem> UnusedReportItems
        {
            get
            {
                return this.unusedReportItems ??
                       (this.unusedReportItems = this.ReportItems.Where(item => ContentUsageToolsHelper.IsUnused(item))
                                                                 .Select(item => new ReportItem(item, UseIndex.Checked))
                                                                 .ToList());
            }
        }

        private IEnumerable<Item> ReportItems
        {
            get
            {
                if (this.reportItems == null)
                {
                    Item root = this.RootItem;
                    this.reportItems = root.Axes.GetDescendants().Where(item => !ContentUsageToolsHelper.IsPage(item) && !item.Fields.All(f => f.Name.StartsWith("_")) );
                }

                return this.reportItems;
            }
        }

        private Item RootItem
        {
            get { return Sitecore.Context.ContentDatabase.GetItem(this.PathTextBox.Text); }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title.Text = Translate.Text("Content Usage Report");
            this.UnusedReport.Text = Translate.Text("Unused Report");
            this.UsedMultipleTimesReport.Text = Translate.Text("Referred items report");
            this.PathLabel.Text = Translate.Text("Item start path");
            this.GenerateReport.Text = Translate.Text("Generate report");
            this.CvTypeOfReport.Text = Translate.Text("You must choose at least one of the reports");

            if (!this.Page.IsPostBack)
            {
                this.UseIndex.Checked = true;
                this.UnusedReport.Checked = true;
                this.RequiredPath.Text = Translate.Text("Please enter a valid path within Sitecore to start from");
            }
        }

        /// <summary>
        /// Handles the OnClick event of the GenerateReport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void GenerateReport_OnClick(object sender, EventArgs e)
        {
            this.ErrorLabel.Text = "";
            this.Page.Validate();

            if (this.Page.IsValid && this.RootItem != null)
            {
                try
                {
                    this.rptResultsUnusedReport.Visible = this.rptResultsReferredItemsReport.Visible = false;

                    if (this.UnusedReport.Checked)
                    {
                        this.rptResultsUnusedReport.Visible = true;
                        this.rptResultsUnusedReport.DataSource = this.UnusedReportItems;
                        this.rptResultsUnusedReport.DataBind();
                    }

                    if (this.UsedMultipleTimesReport.Checked)
                    {
                        this.rptResultsReferredItemsReport.Visible = true;
                        this.rptResultsReferredItemsReport.DataSource = this.ReferredItemsReportItems;
                        this.rptResultsReferredItemsReport.DataBind();
                    }
                }
                catch (KeyNotFoundException keyNotFoundExc)
                {
                    string message = "An exception occurred during generating the report; this may be due to the index not being up to date. Try the \"Normal report\" or rebuild the \"sitecore_master_index\" search index.";
                    Log.Error(message, keyNotFoundExc, this);
                    this.ErrorLabel.Text = Translate.Text(message);
                }
                catch (Exception exc)
                {
                    string message = Translate.Text("An exception occurred during generating the report. The error has been logged.");
                    Log.Error(message, exc, this);
                    this.ErrorLabel.Text = Translate.Text(message);
                }
            }
        }

        /// <summary>
        /// Validates the type of report.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="args">The <see cref="ServerValidateEventArgs"/> instance containing the event data.</param>
        protected void ValidateTypeOfReport(object source, ServerValidateEventArgs args)
        {
            args.IsValid = this.UnusedReport.Checked || this.UsedMultipleTimesReport.Checked;
        }
    }
}