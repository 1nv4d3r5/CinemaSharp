// -------------------------------------------------------------------------
// Name:     WMain
// Author:   Sergey Furtakov
// Abstract: Main Page
// -------------------------------------------------------------------------

// -------------------------------------------------------------------------
// Imports
// -------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CSCode.Modules;
using nsUtilities;

namespace CSCode.Windows
{

    public partial class WMain : System.Web.UI.Page
    {

        // -------------------------------------------------------------------------
        //  Name: Page_Load
        //  Abstract: Load Date ListBox from database
        // -------------------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack == false)
                {
                    CDatabaseUtilities.LoadDateListBoxFromDatabase(1, lstDate); // intPlaceID = 1 for now
                    CreateListOfShows();
                }
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }



        // -------------------------------------------------------------------------
        //  Name: CreateListOfShows
        //  Abstract: Create and display the list of shows
        // -------------------------------------------------------------------------
        protected void CreateListOfShows()
        {
            try
            {
                String strShowList;
                int intDateIndex;
                LiteralControl lcMoviesList = new LiteralControl();

                // Get Date index from ListBox
                intDateIndex = Convert.ToInt32(lstDate.SelectedItem.Value);

                // Get the Show List HTML
                strShowList = CSCode.Modules.CDatabaseUtilities.LoadShowListFromDatabase( intDateIndex );

                // Clean up the list
                divMoviesList.Controls.Clear();

                // Display new list
                lcMoviesList.Text = strShowList;
                divMoviesList.Controls.Add( lcMoviesList );

                // Save Date Index in the page
                spnDate.InnerText = lstDate.SelectedItem.Value;
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);

            }
        }



        // -------------------------------------------------------------------------
        //  Name: lstDate_SelectedIndexChanged
        //  Abstract: Display the list of shows if date changed
        // -------------------------------------------------------------------------
        protected void lstDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CreateListOfShows();
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }



        // -------------------------------------------------------------------------
        //  Name: OpenSeatPickPage
        //  Abstract: Redirect to WSeatPick page
        // -------------------------------------------------------------------------
        protected void OpenSeatPickPage()
        {
            try
            {
                Response.Redirect("WSeatPick.aspx?intShowTimeID=", false);
            }
            catch (Exception excError)
            {
                CWriteLog.WriteLog(excError, false);
            }
        }        
    }
}