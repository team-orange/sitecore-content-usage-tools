<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content Usage Report.aspx.cs" Inherits="ContentUsageTools.Reports.ContentUsageReport" %>

<%@ Import Namespace="Sitecore.Data.Items" %>
<%@ Import Namespace="Sitecore.Links" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Content Usage Report</title>
    <link href="//netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet">
</head>
<body style="max-height: 250px; overflow-y: scroll">
    <form method="post" runat="server" id="form1">

        <div class="container">
            <div class="row-fluid">
                <div class="span9">

                    <h1>Content Usage Report</h1>
                    <div class="form-group">
                        <div class="radio-inline">
                            <asp:RadioButton runat="server" ID="UseIndex" Text="Use the Index for speed search" GroupName="Index" />
                        </div>
                        <div class="radio-inline">
                            <asp:RadioButton runat="server" ID="NoIndex" Text="Use normal search" GroupName="Index" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="checkbox-inline">
                            <asp:CheckBox runat="server" ID="UnusedReport" GroupName="report" ValidationGroup="Report"  />
                        </div>
                        <div class="checkbox-inline">
                            <asp:CheckBox runat="server" ID="UsedMultipleTimesReport" GroupName="report" ValidationGroup="Report"  />
                        </div>
                        <asp:CustomValidator runat="server" ID="CvTypeOfReport" OnServerValidate="ValidateTypeOfReport" ValidationGroup="Report" Display="Dynamic"></asp:CustomValidator>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="PathLabel" runat="server" AssociatedControlID="PathTextBox" />
                        <asp:TextBox runat="server" ID="PathTextBox" Text="/sitecore/content" ValidationGroup="Report" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredPath" runat="server" ControlToValidate="PathTextBox" Display="Dynamic" ValidationGroup="Report"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <asp:Button runat="server" CssClass="btn btn-default" ID="GenerateReport" OnClick="GenerateReport_OnClick" ValidationGroup="Report"></asp:Button>
                    </div>
                    <asp:Repeater runat="server" ID="rptResultsUnusedReport" Visible="False">
                        <HeaderTemplate>
                            <table class="table">
                                <tr>
                                    <th>Name</th>
                                    <th>Path</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# ((ReportItem) Container.DataItem).DisplayName %></td>
                                <td><%# ((ReportItem) Container.DataItem).Path %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater runat="server" ID="rptResultsReferredItemsReport" Visible="False">
                        <HeaderTemplate>
                            <table class="table">
                                <tr>
                                    <th>Name</th>
                                    <th>Path</th>
                                    <th>References</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# ((ReportItem) Container.DataItem).DisplayName %></td>
                                <td><%# ((ReportItem) Container.DataItem).Path %></td>
                                <td>
                                    <asp:Repeater runat="server" DataSource="<%# ((ReportItem) Container.DataItem).ReferredItems %>">
                                        <HeaderTemplate>
                                            <ul class="list-unstyled">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li><a href="<%# LinkManager.GetItemUrl(((Item) Container.DataItem), new UrlOptions() { SiteResolving = true}) %>"><%# ((Item) Container.DataItem).Paths.ContentPath %></a></li>
                                        </ItemTemplate>
                                        <FooterTemplate></ul></FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </div>

    </form>
</body>
</html>
