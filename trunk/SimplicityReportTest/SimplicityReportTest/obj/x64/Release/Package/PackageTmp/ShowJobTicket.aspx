<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowJobTicket.aspx.cs" Inherits="SimplicityReportTest.ShowJobTicket" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
    <div>
    <CR:CrystalReportViewer ID="MyCrystalReportViewer" runat="server" AutoDataBind="true" />
    </div>
    </form>
</body>
</html>

