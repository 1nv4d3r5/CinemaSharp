// ----------------------------------------------------------------------
// Name:        WSeatPick
// Abstract:    Pick the Seat form
// Author:      Sergey Furtakov
//              Sergey.Furtakov@gmail.com
// ----------------------------------------------------------------------


// ----------------------------------------------------------------------
// Using
// ----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using nsUtilities;
using System.Web.UI.HtmlControls;

namespace CSCode.Windows
{
    public partial class WSeatPick : System.Web.UI.Page
    {
        // ----------------------------------------------------------------------
        // Form Variables
        // ----------------------------------------------------------------------
        // Nothing here

        // --------------------------------------------------------------------------------
        // Name: Page_Load
        // Abstract: Display the seat schema.
        // --------------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    CreateSeatSchema();
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }



        // --------------------------------------------------------------------------------
        // Name: CreateSeatSchema
        // Abstract: Create and display the seat schema
        // --------------------------------------------------------------------------------
        protected void CreateSeatSchema()
        {
            try
            {
                String strSeatSchemaHTML;
                LiteralControl lcSeatSchema = new LiteralControl();
                int intDateIndex = 0;
                int intShowTimeID = 0;

                // Display the header for seat schema if page is reloaded
                if (Page.IsPostBack == true)
                {
                    lblPickYourSeat.InnerText = "Pick your seat(s)";
                }

                GetDateAndTimeFromQueryString(ref intDateIndex, ref intShowTimeID);

                // Get the schema in HTML
                strSeatSchemaHTML = CSCode.Modules.CDatabaseUtilities.LoadSeatSchemaFromDatabase(intDateIndex, intShowTimeID, frmSeatPicker);

                // Pre-clean up                
                spnSeatSchema.Controls.Clear();
                
                // Add to page
                lcSeatSchema.Text = strSeatSchemaHTML;
                spnSeatSchema.Controls.Add(lcSeatSchema);
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }



        // -------------------------------------------------------------------------
        //  Name: GetDateAndTimeFromQueryString
        //  Abstract: Get intDateIndex and intShowTimeID from Query String
        // -------------------------------------------------------------------------
        private void GetDateAndTimeFromQueryString(ref int intDateIndex, ref int intShowTimeID)
        {
            try
            {
                int intIndexOfDateIndex = 0;
                int intIndexOfShowTimeID = 0;
                int intStringLength = 0;
                String strQueryString = "";
                String strDateIndexString = "";
                String strShowTimeIDString = "";

                // Since Request.QueryString.Get does not work...
                // Decode Query String                
                strQueryString = Server.UrlDecode(Request.QueryString.ToString());

                // Find indexes of query keys
                intIndexOfDateIndex = strQueryString.IndexOf("intDateIndex");
                intIndexOfShowTimeID = strQueryString.IndexOf("intShowTimeID");
                intStringLength = strQueryString.Length;

                // Extract intDateIndex key value
                strDateIndexString = strQueryString.Substring(intIndexOfDateIndex + 13, intStringLength - intIndexOfDateIndex - 13);
                Int32.TryParse(strDateIndexString, out intDateIndex);
                
                // Extract intShowTimeID key value
                strShowTimeIDString = strQueryString.Substring(intIndexOfShowTimeID + 14, intIndexOfDateIndex - intIndexOfShowTimeID - 15);
                Int32.TryParse(strShowTimeIDString, out intShowTimeID);                
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }
        


        // --------------------------------------------------------------------------------
        // Name:        btnPickSeat_Click
        // Abstract:    Open confirmation window, hide/unhide Pick Seat, Purchase buttons
        // --------------------------------------------------------------------------------
        protected void btnPickSeat_Click(object sender, EventArgs e)
        {
            try
            {
                if (OpenConfirmationDialog() == true)
                {
                    // Hide/unhide buttons
                    btnPickSeat.Visible = false;
                    btnPurchase.Visible = true;
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }
        

                
        // --------------------------------------------------------------------------------
        // Name:        OpenConfirmationDialog
        // Abstract:    Replace a seat schema with purchase details
        // --------------------------------------------------------------------------------
        protected bool OpenConfirmationDialog()
        {
            bool blnResult = false;

            try
            {
                String strNewLine = "<br />";
                List<int> liSeats = null;
                String strSeatList = "";
                int intIndex = 0;
                Decimal decPreTotal = 0M;
                Decimal decPrice = 0M;
                int intAmountOfTickets = 0;
                Decimal decTax = 0.065M;
                Decimal decTotal = 0M;
                HtmlGenericControl hgcConfirmDateAndTime = new HtmlGenericControl();
                HtmlGenericControl hgcConfirmShowName = new HtmlGenericControl();
                HtmlGenericControl hgcConfirmSeatList = new HtmlGenericControl();
                HtmlGenericControl hgcConfirmPreTotal = new HtmlGenericControl();
                HtmlGenericControl hgcConfirmTotal = new HtmlGenericControl();

                // Get a picked seats list
                liSeats = GetPickedRowSeats();

                // Any seats picked?
                if (liSeats.Count > 0)
                {
                    // Yes, so
                    // Clean the window
                    lblPickYourSeat.InnerText = "";
                    spnSeatSchema.Controls.Clear();

                    // Prepare:                
                    // Payment options...( go here... )

                    // Date + Time
                    hgcConfirmDateAndTime.InnerHtml = lblShowDate.Text + ", " + lblShowTime.Text + strNewLine;

                    // Show Name
                    hgcConfirmShowName.InnerHtml = lblShowName.Text + strNewLine;

                    // Build a seat list string
                    strSeatList = "Your seats: " + strNewLine;

                    for (intIndex = 0; intIndex < liSeats.Count - 1; intIndex += 2)
                    {
                        strSeatList += liSeats.ElementAt(intIndex).ToString() + " Row, " +
                                       liSeats.ElementAt(intIndex + 1).ToString() + " Seat" + strNewLine;
                    }

                    hgcConfirmSeatList.InnerHtml = strNewLine + strSeatList;

                    // Pretotal and Total                
                    // Get Price
                    Decimal.TryParse(lblPrice.Text.Substring(1), out decPrice);

                    // Get amount
                    intAmountOfTickets = liSeats.Count / 2;

                    // Get Pretotal
                    decPreTotal = decPrice * intAmountOfTickets;
                    hgcConfirmPreTotal.InnerHtml = strNewLine + "Pre Total: $" + decPreTotal.ToString() + strNewLine;

                    // Total
                    decTotal = decPreTotal * (1 - decTax);
                    hgcConfirmTotal.InnerHtml = "Total: " + decTotal.ToString("c");

                    // Display all
                    spnSeatSchema.Controls.Add(hgcConfirmDateAndTime);
                    spnSeatSchema.Controls.Add(hgcConfirmShowName);
                    spnSeatSchema.Controls.Add(hgcConfirmSeatList);
                    spnSeatSchema.Controls.Add(hgcConfirmPreTotal);
                    spnSeatSchema.Controls.Add(hgcConfirmTotal);

                    // Success
                    blnResult = true;
                }                
            }
            catch (Exception excError)
            {
                // Will pop up on the server...( change it later )
                CWriteLog.WriteLog(excError, false);
            }

            return blnResult;
        }



        // --------------------------------------------------------------------------------
        // Name:        GetPickedRowSeats
        // Abstract:    Get the list of Rows and Seats of picked seats.
        // --------------------------------------------------------------------------------
        private List<int> GetPickedRowSeats()
        {
            List<int> liRowSeats = new List<int>();

            try
            {
                String strRowSeats;
                Array astrRowAndSeats;
                String strNumber;
                int intID;
                int intIndex;

                // Get the Row-Seat string
                strRowSeats = hfRowSeat.Value;

                astrRowAndSeats = strRowSeats.Split('-', ',');

                // Go throuhg the list
                for (intIndex = 0; intIndex < astrRowAndSeats.Length - 1; intIndex += 1)
                {
                    // Get the ID
                    strNumber = (String)astrRowAndSeats.GetValue(intIndex);
                    Int32.TryParse(strNumber, out intID);

                    // Add the ID
                    liRowSeats.Add(intID);
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return liRowSeats;
        }



        // --------------------------------------------------------------------------------
        // Name:        btnPurchase_Click
        // Abstract:    Update database and go to Thank you page
        // --------------------------------------------------------------------------------
        protected void btnPurchase_Click(object sender, EventArgs e)
        {
            int intDateIndex = 0;
            int intShowTimeID = 0;
            List<int> liRowAndSeatIDs = new List<int>();
            String strScript = "";

            // Get Date and Time
            Int32.TryParse(hfShowDateID.Value, out intDateIndex);
            Int32.TryParse(hfShowTimeID.Value, out intShowTimeID);

            // Get IDs
            liRowAndSeatIDs = GetPickedRowSeats();

            // Try to update database
            if (CSCode.Modules.CDatabaseUtilities.MakeSeatsPicked(intDateIndex, intShowTimeID, liRowAndSeatIDs) == true)
            {
                // Success, so go to Thank You page
                Response.Redirect("PThankYouForPurchase.html", false);
            }
            else
            {
                // Refresh buttons
                btnPickSeat.Visible = true;
                btnPurchase.Visible = false;

                CreateSeatSchema();
                
                // Create a script call string                      
                strScript = @"<script language='javascript'>SelectPickedAndDisplayTakenSeats();</script>";
                
                // Run JavaScript
                System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "", strScript, false);
            }
        }
    }
}