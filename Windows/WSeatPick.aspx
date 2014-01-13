<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WSeatPick.aspx.cs" Inherits="CSCode.Windows.WSeatPick" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

        <!-- Name: Seat picker -->
        <!-- Abstract: Pick a seat window -->
    <head runat="server">

        <link rel="stylesheet" type="text/css" href="../Styles/cssCustom.css" />
        <link rel="stylesheet" type="text/css" href="../Styles/cssBase.css" />
        <script type="text/javascript" src="../Scripts/jsSeatPick.js"></script>
       
        <title></title>

    </head>

    <body onload="ClickHandler();">

        <form id="frmSeatPicker" runat="server">

            <fieldset>

                <legend></legend>
                                                
                <%--Controls go here--%>
                <label id="lblControls">
                    
                    <br />

                    <%--Show Date--%>
                    <asp:HiddenField ID="hfShowDateID" runat="server" />
                    <asp:Label ID="lblShowDate" runat="server" Text="lblDate"></asp:Label>
                    
                    <br />
                    
                    <%--Show Time--%>
                    <asp:HiddenField ID="hfShowTimeID" runat="server" />
                    <asp:Label ID="lblShowTime" runat="server" Text="lblTime"></asp:Label>

                    <br />

                    <%--Show Name--%>
                    <asp:HiddenField ID="hfShowNameID" runat="server" />
                    <asp:Label ID="lblShowName" runat="server" Text="lblShow"></asp:Label>
            
                    <br />

                    <%--Price--%>
                    <asp:HiddenField ID="hfPriceID" runat="server" />
                    <asp:Label ID="lblPrice" runat="server" Text="lblPrice"></asp:Label>
            
                    <br />
                    <br />

                    <%--Temporary storage for Row-Seats--%>
                    <asp:HiddenField ID="hfRowSeat" runat="server" Value="" />
                    
                    <%--Pick Seat--%>                    
                    <asp:Button ID="btnPickSeat" CssClass="Button" runat="server" Text="Pick" OnClientClick="return GetListOfPickedSeats();" OnClick="btnPickSeat_Click" />

                    <br />

                    <%--Purchase--%>
                    <asp:Button ID="btnPurchase" CssClass="Button" runat="server" Text="Purchase" OnClick="btnPurchase_Click" Visible="false" />

                </label>
                
                <label id="lblPickYourSeat" runat="server">Pick your seat(s)</label>
                
                <span id="spnSeatSchema" runat="server"></span>

            </fieldset>

        </form>

        <span style="position:absolute; bottom:0; width:99%; text-align:center;">&copy; 2014 Sergey Furtakov</span>

    </body>

</html>
