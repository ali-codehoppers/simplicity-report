<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowReports.aspx.cs" Inherits="SimplicityReportTest.ShowReports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 250px; height: 120px; border: 2px solid #ccc; padding: 5px; margin: 5px;">
        JobTicket:
        <br>
        <br>
        <div style="vertical-align: middle">
            Job ID:
            <asp:TextBox ID="JobTicketJobID" runat="server"></asp:TextBox>
        </div>
        <br>
        <div style="text-align: center">
            <asp:Button ID="ButtonJobTicket" runat="server" Text="Show Report" OnClick="ButtonJobTicket_Click" />
        </div>
    </div>
    <div style="width: 250px; height: 120px; border: 2px solid #ccc; padding: 5px; margin: 5px;">
        ProForma:
        <br>
        <br>
        <div style="vertical-align: middle">
            Invoice ID:
            <asp:TextBox ID="ProFormaInvoiceID" runat="server"></asp:TextBox>
        </div>
        <br>
        <div style="text-align: center">
            <asp:Button ID="ButtonProForma" runat="server" Text="Show Report" OnClick="ButtonProForma_Click" />
        </div>
    </div>
    <div style="width: 250px; height: 120px; border: 2px solid #ccc; padding: 5px; margin: 5px;">
        Estimate:
        <br>
        <br>
        <div style="vertical-align: middle">
            Job ID:
            <asp:TextBox ID="EstimateJobID" runat="server"></asp:TextBox>
        </div>
        <br>
        <div style="text-align: center">
            <asp:Button ID="ButtonEstimate" runat="server" Text="Show Report" OnClick="ButtonEstimate_Click" />
        </div>
    </div>
    <!--    <div style="width: 250px; height: 120px; border: 2px solid #ccc; padding: 5px; margin: 5px;">
        OrderInvoice:
        <br>
        <br>
        <div style="vertical-align: middle">
            Invoice ID:
            <asp:TextBox ID="OrderInvoiceID" runat="server"></asp:TextBox>
        </div>
        <br>
        <div style="text-align: center">
            <asp:Button ID="ButtonOrderInvoice" runat="server" Text="Show Report" />
        </div>
    </div>
    -->
    </form>
</body>
</html>
