// -------------------------------------------------------------------------
// Module:   CDatabaseUtilities
// Author:   Sergey Furtakov
// Abstract: Database utilities
// -------------------------------------------------------------------------

// -------------------------------------------------------------------------
// Imports
// -------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using nsUtilities;
using System.Web.UI;
using System.Xml.Linq;

namespace CSCode.Modules
{
    public class CDatabaseUtilities
    {
        // -------------------------------------------------------------------------
        //  Constants
        // -------------------------------------------------------------------------
        private const String strLOG_FILE_EXTENSION = ".Log";

        // -------------------------------------------------------------------------
        //  Properties
        // -------------------------------------------------------------------------
        // Access Connection string (notice the use of the relative path)                           
        // Private m_strDatabaseConnectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;" +
        //                                                  "Data Source=" & Application.StartupPath & "..\..\..\Database\dbCinema.mdb;" +
        //                                                  "User ID=Admin;" +
        //                                                  "Password=;";

        // SQL Server 2012
        private static String m_strDatabaseConnectionString =   "Provider=SQLOLEDB;" +
                                                                "Data Source=(local);" +
                                                                "Database=dbCinema;" +
                                                                "Trusted_Connection=yes;";

        private static OleDbConnection m_conAdministrator = null;


        // -------------------------------------------------------------------------
        //  Name: OpenDatabaseConnection
        //  Abstract: Open a connection to the database.
        // -------------------------------------------------------------------------
        public static Boolean OpenDatabaseConnection()
        {
            Boolean blnResult = false;

              try
              {                  
                  // Allocate memory
                  m_conAdministrator = new OleDbConnection();

                  // Open a connection to database
                  m_conAdministrator.ConnectionString = m_strDatabaseConnectionString;
                  m_conAdministrator.Open();

                  // If we got that far then everything is ok
                  blnResult = true;
              }
              catch (Exception excError)
              {
                  CWriteLog.WriteLog(excError, false);
              }

            return blnResult;
        }



        // -------------------------------------------------------------------------
        //  Name: CloseDatabaseConnection
        //  Abstract: Close the connection to the database.
        // -------------------------------------------------------------------------
        public static Boolean CloseDatabaseConnection()
        {
            Boolean blnResult = false;

            try
            {
                if (m_conAdministrator != null)
                {
                    // If the connection is open then close it
                    if (m_conAdministrator.State != ConnectionState.Closed)
                    {
                        m_conAdministrator.Close();
                        m_conAdministrator = null;

                        // If we got that far then everything is ok
                        blnResult = true;
                    }
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return blnResult;
        }



        // -------------------------------------------------------------------------
        //  Name: LoadDateListBoxFromDatabase
        //  Abstract: Load Date ListBox from database
        // -------------------------------------------------------------------------
        public static Boolean LoadDateListBoxFromDatabase( int intPlaceID, ListBox lstTarget )
        {
            Boolean blnResult = false;

            try
            {
                DataTable dtSource = new DataTable();
                OleDbCommand cmdSQLStatement = new OleDbCommand();
                OleDbDataAdapter daPlaceDates = new OleDbDataAdapter();
                ListItem liItem;
                int intDateID = 0;
                DateTime dtmDate;
                int intIndex = 0;

                if (OpenDatabaseConnection())
                {
                    // Clean up the list
                    lstTarget.Items.Clear();

                    // Create SQL command object to run our stored procedure
                    cmdSQLStatement.Connection = m_conAdministrator;
                    cmdSQLStatement.CommandText = "SELECT" + 
                                                        " intDateIndex" +
                                                        ",dtmDate" +
                                                  " FROM" + 
                                                        " TPlaceDates" +
                                                  " WHERE" +
                                                        " intPlaceID = " + intPlaceID.ToString() +
                                                  " ORDER BY" +
                                                        " dtmDate DESC";

                    // Set SQL statement
                    daPlaceDates.SelectCommand = cmdSQLStatement;

                    // Retrieve the all the records from select command and store in dataset provided
                    daPlaceDates.Fill(dtSource);

                    // Loop through all records
                    for (intIndex = 0; intIndex < dtSource.Rows.Count; intIndex += 1)
                    {
                        // Create a List Item to hold the date information
                        intDateID = dtSource.Rows[intIndex].Field<int>("intDateIndex");
                        dtmDate = dtSource.Rows[intIndex].Field<DateTime>("dtmDate");
                        liItem = new ListItem((dtmDate).ToString("m"), intDateID.ToString());

                        // Add the Item to list
                        lstTarget.Items.Add(liItem);
                    }

                    // Anything in list?
                    if (lstTarget.Items.Count > 0)
                    {
                        // Yes, so select last date ( change it to today... )
                        lstTarget.SelectedIndex = 0;
                    }

                    // Clean up
                    dtSource.Dispose();

                    // Success
                    blnResult = true;
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
            finally
            {
                CloseDatabaseConnection();
            }

            return blnResult;
        }


        
        // -------------------------------------------------------------------------
        //  Name: LoadShowListFromDatabase
        //  Abstract: Load Show List ListBox from database
        // -------------------------------------------------------------------------
        public static String LoadShowListFromDatabase( int intDateIndex )
        {
            String strResultHTML = "";

            try
            {
                DataTable dtSource = new DataTable();
                OleDbCommand cmdSQLStatement = new OleDbCommand();
                OleDbDataAdapter daPlaceDateShowTimes = new OleDbDataAdapter();
                int intIndex = 0;
                int intShowTimeID = 0;
                String strShowTime;
                int intShowNameID;
                String strShowName;

                if (OpenDatabaseConnection())
                {
                    // Create SQL command object to run our stored procedure
                    cmdSQLStatement.Connection = m_conAdministrator;
                    cmdSQLStatement.CommandText = "uspGetListOfMovies";
                    cmdSQLStatement.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intPlaceID", 1));
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intDateIndex", intDateIndex));

                    // Set SQL statement
                    daPlaceDateShowTimes.SelectCommand = cmdSQLStatement;

                    // Retrieve the all the records from select command and store in dataset provided
                    daPlaceDateShowTimes.Fill(dtSource);

                    // Loop through all records
                    for (intIndex = 0; intIndex < dtSource.Rows.Count; intIndex += 1)
                    {
                        // Retrieve a show information from data table
                        intShowTimeID = dtSource.Rows[intIndex].Field<int>("intShowTimeID");
                        strShowTime = dtSource.Rows[intIndex].Field<String>("strShowTime");
                        intShowNameID = dtSource.Rows[intIndex].Field<int>("intShowNameID");
                        strShowName = dtSource.Rows[intIndex].Field<String>("strShowName");

                        // Create an HTML
                        strResultHTML += GetShowNameAndTimeTag(strShowName, intShowTimeID, strShowTime);
                    }

                    // Clean up
                    dtSource.Dispose();
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
            finally
            {
                CloseDatabaseConnection();
            }

            return strResultHTML;
        }



        // -------------------------------------------------------------------------
        //  Name: GetListOfShowsHTML
        //  Abstract: Create a span HTML tag string of format:
        // "<span class=\"spnShowInfo\">strShowName" +
        //      "<a  href=\"#\" id=\"intShowTimeID\" onclick=\"OpenSeatPickPage(id);\">strShowTime</a> +
        // "</span>\n\t\t\t<br />\n\t\t\t"
        // -------------------------------------------------------------------------
        public static String GetShowNameAndTimeTag(String strShowName, int intShowTimeID, String strShowTime)
        {
            String strResultHTML = "";

            try
            {
                String strSpanBeginning = "<span ";
                String strSpanEnding = "</span>\n\t\t\t";
                String strClassShowInfo = "class=\"spnShowInfo\">";
                String strNewLine = "<br />\n\t\t\t";
                String strABeginin = "<a  href=\"#\" id=\"";
                String strAMiddle = "\" onclick=\"OpenSeatPickPage(id);\">";
                String strAEnding = "</a>";

                strResultHTML += strSpanBeginning + strClassShowInfo +
                                strShowName +
                                strABeginin + intShowTimeID.ToString() + strAMiddle + strShowTime + strAEnding +
                                strSpanEnding +
                                strNewLine;
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return strResultHTML;
        }



        // -------------------------------------------------------------------------
        //  Name: LoadSeatSchemaFromDatabase
        //  Abstract: Load Seat Schema from database
        // -------------------------------------------------------------------------
        public static String LoadSeatSchemaFromDatabase(int intDateIndex, int intShowTimeID, System.Web.UI.HtmlControls.HtmlForm frmTarget)
        {
            String strResultHTML = "";

            try
            {
                DataTable dtSource = new DataTable();
                OleDbCommand cmdSQLStatement = new OleDbCommand();
                OleDbDataAdapter daPlaceDateShowTimes = new OleDbDataAdapter();
                int intIndex = 0;
                int intRowID;
                int intSeatID;
                int intSellStatusID;

                if (OpenDatabaseConnection())
                {
                    // Create SQL command object to run our stored procedure
                    cmdSQLStatement.Connection = m_conAdministrator;
                    cmdSQLStatement.CommandText = "uspGetSeatSchema";
                    cmdSQLStatement.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intPlaceID", 1));
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intDateIndex", intDateIndex));
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intShowTimeID", intShowTimeID));

                    // Set SQL statement
                    daPlaceDateShowTimes.SelectCommand = cmdSQLStatement;

                    // Retrieve the all the records from select command and store in dataset provided
                    daPlaceDateShowTimes.Fill(dtSource);

                    // Loop through all records
                    for (intIndex = 0; intIndex < dtSource.Rows.Count; intIndex += 1)
                    {
                        // Create a List Item to hold the date information
                        intRowID = dtSource.Rows[intIndex].Field<int>("intRowID");              // Use just IDs instead of Name columns 
                        intSeatID = dtSource.Rows[intIndex].Field<int>("intSeatID");            // temporary
                        intSellStatusID = dtSource.Rows[intIndex].Field<int>("intSellStatusID");

                        strResultHTML += GetSeatTag(intRowID, intSeatID, intSellStatusID);
                    }

                    DisplayShowInfo(dtSource, frmTarget);

                    // Clean up
                    dtSource.Dispose();
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
            finally
            {
                CloseDatabaseConnection();
            }

            return strResultHTML;
        }


                
        // -------------------------------------------------------------------------
        //  Name: GetSeatTag
        //  Abstract: Create a span tag with ID="intRowID-intSeatID" and 
        //                              class ="spnAvailableSeat/spnUnavailableSeat"
        // -------------------------------------------------------------------------
        private static String GetSeatTag(int intRowID, int intSeatID, int intSellStatusID)
        {
            String strResultHTML = "";

            try
            {
                String strSpanBeginning = "<span ";
                String strIDBeginning = "id=\"";
                String strIDEnding = "\"";
                String strClassAvailableSeat = "class=\"spnAvailableSeat\"";
                String strClassUnavailableSeat = "class=\"spnUnavailableSeat\"";
                String strOnClick = "onclick=\"ChangeSelectedSeatStatus(e);\" >";
                String strSpanEnding = "</span>\n\t\t\t";
                String strNewLine = "<br />\n\t\t\t";

                // Open the tag and create ID ( in format "row-seat" )
                strResultHTML += strSpanBeginning +
                                 strIDBeginning + intRowID.ToString() + "-" + intSeatID.ToString() + strIDEnding;

                // Available seat?
                if (intSellStatusID == 1)
                {
                    // Yes, so make available
                    strResultHTML += strClassAvailableSeat;
                }
                else
                {
                    strResultHTML += strClassUnavailableSeat;
                }

                // Add onClick and close the tag
                strResultHTML += strOnClick + strSpanEnding;

                // End of the row?
                if (intSeatID == 20)    // Yeah, it is hardcoded for now
                {
                    // Yes, so add new line
                    strResultHTML += strNewLine;
                }   
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return strResultHTML;
        }



        // -------------------------------------------------------------------------
        //  Name: DisplayShowInfo
        //  Abstract: Display the show info on the form
        // -------------------------------------------------------------------------
        private static void DisplayShowInfo( DataTable dtSource, System.Web.UI.HtmlControls.HtmlForm frmTarget)
        {
            try
            {
                int intDateID = 0;
                HiddenField hfShowDateID;
                DateTime dtmDate;
                Label lblShowDate;
                int intShowTimeID = 0;
                HiddenField hfShowTimeID;
                String strShowTime;
                Label lblShowTime;
                int intShowNameID = 0;
                HiddenField hfShowNameID;
                String strShowName;
                Label lblShowName;
                int intPriceID = 0;
                HiddenField hfPrice;
                Decimal decPrice;
                Label lblPrice;

                // Date
                intDateID = dtSource.Rows[0].Field<int>("intDateIndex");
                hfShowDateID = (HiddenField)FindControlRecursive(frmTarget, "hfShowDateID");
                hfShowDateID.Value = intDateID.ToString();
                
                dtmDate = dtSource.Rows[0].Field<DateTime>("dtmDate");
                lblShowDate = (Label)FindControlRecursive(frmTarget, "lblShowDate");
                lblShowDate.Text = dtmDate.ToString("m");

                // Time
                intShowTimeID = dtSource.Rows[0].Field<int>("intShowTimeID");
                hfShowTimeID = (HiddenField)FindControlRecursive(frmTarget, "hfShowTimeID");
                hfShowTimeID.Value = intShowTimeID.ToString();

                strShowTime = dtSource.Rows[0].Field<String>("strShowTime");
                lblShowTime = (Label)FindControlRecursive(frmTarget, "lblShowTime");
                lblShowTime.Text = strShowTime;

                // Name
                intShowNameID = dtSource.Rows[0].Field<int>("intShowNameID");
                hfShowNameID = (HiddenField)FindControlRecursive(frmTarget, "hfShowNameID");
                hfShowNameID.Value = intShowNameID.ToString();

                strShowName = dtSource.Rows[0].Field<String>("strShowName");
                lblShowName = (Label)FindControlRecursive(frmTarget, "lblShowName");
                lblShowName.Text = strShowName;

                // Price
                intPriceID = dtSource.Rows[0].Field<int>("intPriceID");
                hfPrice = (HiddenField)FindControlRecursive(frmTarget, "hfPriceID");
                hfPrice.Value = intPriceID.ToString();

                decPrice = dtSource.Rows[0].Field<Decimal>("curPrice");
                lblPrice = (Label)FindControlRecursive(frmTarget, "lblPrice");
                lblPrice.Text = "$" + decPrice.ToString();             
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);

            }
        }



        // -------------------------------------------------------------------------
        //  Name: FindControlRecursive
        //  Abstract: Find the control on the form recursively
        // -------------------------------------------------------------------------
        private static Control FindControlRecursive(Control ctrTarget, string strControlId)
        {
            Control ctrResult = null;

            try
            {
                Control ctrFound = null;

                // IDs match?
                if (ctrTarget.ID == strControlId)
                {
                    // Yes, so control found.
                    ctrResult = ctrTarget;
                }
                else
                {
                    // No, so try to find nested controls
                    foreach (Control ctrInnerControl in ctrTarget.Controls)
                    {
                        ctrFound = FindControlRecursive(ctrInnerControl, strControlId);
                        
                        // If not null, then control found.
                        if (ctrFound != null)
                        {
                            ctrResult = ctrFound;
                            break;
                        }
                    }
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return ctrResult;
        }



        // -------------------------------------------------------------------------
        //  Name: LoadSeatSchemaFromDatabase
        //  Abstract: Load Seat Schema from database
        // -------------------------------------------------------------------------
        public static bool MakeSeatsPicked(int intDateIndex, int intShowTimeID, List<int> liRowAndSeatIDs)
        {
            bool blnResult = false;

            try
            {
                DataTable dtSource = new DataTable();
                OleDbCommand cmdSQLStatement = new OleDbCommand();
                OleDbDataAdapter daPlaceDateShowTimes = new OleDbDataAdapter();
                OleDbDataReader drCommandResult;
                XElement xRowSeats;
                bool blnUpdateResult = false;

                if (OpenDatabaseConnection())
                {
                    // Create SQL command object to run our stored procedure
                    cmdSQLStatement.Connection = m_conAdministrator;
                    cmdSQLStatement.CommandText = "uspPickSeats2";
                    cmdSQLStatement.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intPlaceID", 1));
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intDateIndex", intDateIndex));
                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@intShowTimeID", intShowTimeID));

                    xRowSeats = GetRowSeatsXML(liRowAndSeatIDs);

                    cmdSQLStatement.Parameters.Add(new OleDbParameter("@XMLRowSeats", xRowSeats.ToString()));

                    // Execute sotored procedure
                    drCommandResult = cmdSQLStatement.ExecuteReader();

                    // Get the result flag
                    drCommandResult.Read();
                    blnUpdateResult = (bool)drCommandResult.GetValue(0);
                    
                    // Clean up
                    drCommandResult.Close();
                    dtSource.Dispose();
                    
                    // General success if update wwas succesful
                    blnResult = blnUpdateResult;
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
            finally
            {
                CloseDatabaseConnection();
            }

            return blnResult;
        }



        // -------------------------------------------------------------------------
        //  Name:       GetRowSeatsXML
        //  Abstract:   Get the XML from the list of Row and Seat IDs
        // -------------------------------------------------------------------------
        private static XElement GetRowSeatsXML( List<int> liRowAndSeatIDs)
        {
            XElement xRowSeats = null;

            try
            {
                int intIndex;
                
                // Create a parent node
                xRowSeats = new XElement("TRowSeats");
                
                // Go through the list of IDs
                for (intIndex = 0; intIndex < liRowAndSeatIDs.Count; intIndex += 2)
                {
                    // Create a node with the ID pair
                    XElement xNewRowSeat =
                        new XElement("RowSeat",
                            new XAttribute("intRowID", liRowAndSeatIDs.ElementAt(intIndex)),
                            new XAttribute("intSeatID", liRowAndSeatIDs.ElementAt(intIndex + 1))
                            );

                    // Add to parent
                    xRowSeats.Add(xNewRowSeat);
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }

            return xRowSeats;
        }
    }
}