<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content Usage Report.aspx.cs" Inherits="ContentUsageTools.layouts.Orange.Content_Usage_Report" %>

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
                    <asp:RadioButton runat="server"  ID="UnusedReport" GroupName="report" ValidationGroup="Report" />
                    <asp:RadioButton runat="server" ID="UsedMultipleTimesReport" GroupName="report" ValidationGroup="Report"/>
                    <asp:CustomValidator runat="server" ID="CvTypeOfReport"  OnServerValidate="ValidateTypeOfReport" ValidationGroup="Report" Display="Dynamic"></asp:CustomValidator>
                </div>
                <div>
                    <asp:Label ID="PathLabel" runat="server" AssociatedControlID="PathTextBox" />
                    <asp:TextBox runat="server" ID="PathTextBox" />
                    <asp:Button runat="server" ID="GenerateReport" OnClick="GenerateReport_OnClick" ValidationGroup="Report"></asp:Button>
                </div>
                <div>
                    <asp:Repeater runat="server" ID="ResultRepeater" OnItemDataBound="ResultPanel_OnItemDataBound">
                        <HeaderTemplate>
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            <asp:Literal runat="server" ID="ItemNameTitle" /></th>
                                        <th>
                                            <asp:Literal runat="server" ID="UsedByTitle" /></th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Repeater runat="server" ID="UsedByRepeater" OnItemDataBound="UsedByRepeater_OnItemDataBound">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:Literal runat="server" ID="ItemNameUsedBy" />
                                                    -
                                                    <asp:HyperLink runat="server" ID="WatchItem" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </td>
                                </tr>
                            </tbody>
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
