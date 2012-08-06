<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportSchema.aspx.cs" Inherits="SimplicityReportTest.ReportSchema" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="errorLabel" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
        <div>
            <asp:Label runat="server">Report Type</asp:Label>
            <asp:TextBox ID="ReportID" runat="server"></asp:TextBox>
        </div>

        <asp:Button runat="server" ID="downloadReport" Text="Download" 
            onclick="downloadReport_Click"/>
    </div>
    </form>
</body>
</html>
