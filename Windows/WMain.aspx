<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WMain.aspx.cs" Inherits="CSCode.Windows.WMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

    <!-- Name: Sergey Furtakov -->
    <!-- Abstract: Cinema Sharp Project -->

<head runat="server">

    <link rel="stylesheet" type="text/css" href="../Styles/cssCustom.css" />
    <link rel="stylesheet" type="text/css" href="../Styles/cssBase.css" />
    <script type="text/javascript" src="../Scripts/jsSeatPick.js"></script>

    <title></title>
        
</head>

<body>

    <form id="frmMain" runat="server">
        
        <fieldset>

            <legend></legend>
            
            <asp:ListBox ID="lstDate" runat="server" Rows="1" Width="200px" OnSelectedIndexChanged="lstDate_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            
            <br />
            <br />
            <br />
            <span style="display:none" id="spnDate" runat="server"></span>
                        
            <div id="divMoviesList" runat="server"></div>

        </fieldset>

    </form>

    <span style="position:absolute; bottom:0; width:99%; text-align:center;">&copy; 2014 Sergey Furtakov</span>

</body>

</html>
