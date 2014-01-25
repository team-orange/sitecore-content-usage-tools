<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Content Usage Report.aspx.cs" Inherits="ContentUsageTools.Reports.ContentUsageReport" %>

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
                    <asp:TextBox runat="server" ID="PathTextBox" ValidationGroup="Report" />
                    <asp:RequiredFieldValidator ID="RequiredPath" runat="server" ControlToValidate="PathTextBox" Display="Dynamic" ValidationGroup="Report"></asp:RequiredFieldValidator>
                    <asp:Button runat="server" ID="GenerateReport" OnClick="GenerateReport_OnClick" ValidationGroup="Report"></asp:Button>
                </div>
                <asp:Repeater runat="server" ID="rptResultsUnusedReport" DataSource="UnusedReportItems" Visible="False">
                    <HeaderTemplate>
                        <table>
                            <tr>
                                <th>Name</th>
                                <th>Path</th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                </asp:Repeater>
            </div>
        </div>

    </form>
</body>
</html>
