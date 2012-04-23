<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SimplicityReportTest.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <script type="text/javascript" language="javascript">
        if (location.protocol != "https:") {
            document.write("OAuth will not work correctly from plain http. " +
                    "Please use an https URL.");
        } else {
            document.write("<a href=\"authenticate.aspx\">Click here to retrieve contacts from Salesforce via REST/OAuth.</a>");
        }
        </script>
    </form>
</body>
</html>
