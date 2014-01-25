<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content Usage Report.aspx.cs" Inherits="ContentUsageTools.Reports.ContentUsageReport" %>
<%@ Import Namespace="Sitecore.Data.Items" %>
<%@ Import Namespace="Sitecore.Links" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Content Usage Report</title>
</head>
<body>
    <form method="post" runat="server" id="mainform">

        <div>
            <div>
                <div>
                    <asp:CheckBox runat="server" ID="UnusedReport" GroupName="report" ValidationGroup="Report" AutoPostBack="True" />
                    <asp:CheckBox runat="server" ID="UsedMultipleTimesReport" GroupName="report" ValidationGroup="Report" AutoPostBack="True" />
                    <asp:CustomValidator runat="server" ID="CvTypeOfReport" OnServerValidate="ValidateTypeOfReport" ValidationGroup="Report" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div>
                    <asp:Label ID="PathLabel" runat="server" AssociatedControlID="PathTextBox" />
                    <asp:TextBox runat="server" ID="PathTextBox" Text="/sitecore/content" ValidationGroup="Report" />
                    <asp:RequiredFieldValidator ID="RequiredPath" runat="server" ControlToValidate="PathTextBox" Display="Dynamic" ValidationGroup="Report"></asp:RequiredFieldValidator>
                    <br /><asp:Button runat="server" ID="GenerateReport" OnClick="GenerateReport_OnClick" ValidationGroup="Report"></asp:Button>
                </div>

                <asp:Repeater runat="server" ID="rptResultsUnusedReport" Visible="False">
                    <HeaderTemplate>
                        <table>
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
                        <table>
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
                                    <HeaderTemplate><ul></HeaderTemplate>
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

    </form>
</body>
</html>
