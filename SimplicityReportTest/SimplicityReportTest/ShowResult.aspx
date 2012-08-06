<%@ Page Language="C#" ClientTarget="downlevel" AutoEventWireup="true" CodeBehind="ShowResult.aspx.cs" Inherits="SimplicityReportTest.ShowResult" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="errorLabel" runat="server" Text="" Visible="false" ForeColor=Red></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
        <div>
            <CR:CrystalReportViewer ID="MyCrystalReportViewer" runat="server" AutoDataBind="true" Visible="false"/>
        </div>
    </div>
    </form>
</body>
</html>
