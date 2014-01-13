// ----------------------------------------------------------------------
// Name:        jsSeatPick.js
// Abstract:    Script for WSeatPick.aspx
// Author:      Sergey Furtakov
//              Sergey.Furtakov@gmail.com
// ----------------------------------------------------------------------


// --------------------------------------------------------------------------------
// Name:        OpenSeatPickPage
// Abstract:    Create a query string and go to Seat Pick page
// --------------------------------------------------------------------------------
function OpenSeatPickPage(id)
{
    window.location.href = "WSeatPick.aspx"
                         + "?intShowTimeID=" + id
                         + "?intDateIndex=" + spnDate.textContent.toString();
}



// --------------------------------------------------------------------------------
// Name:        GetHTMLElement
// Abstract:    Get the HTML element that has been clicked
// --------------------------------------------------------------------------------
function GetHTMLElement(e)
{
    var htmTarget = e.target;
    var htmTag = [];

    // Get the tag type
    htmTag.tagType = htmTarget.tagName.toLowerCase();

    // ..class
    htmTag.tagClass = htmTarget.className.split(' ');

    // ..and id
    htmTag.id = htmTarget.id;

    // htmTag.parent = htmTag.parentNode.tagName.toLowerCase();
    
    return htmTag;
}



// --------------------------------------------------------------------------------
// Name:        ClickHandler
// Abstract:    Click handler for seats
// --------------------------------------------------------------------------------
function ClickHandler()
{
    document.body.onclick = function (e)
    {
        var htmTag = GetHTMLElement(e);        
        var htmSeat;

        // spnAvailableSeat or spnPickedSeat clicked?
        if (htmTag.tagType == 'span' && ( htmTag.tagClass == 'spnAvailableSeat' || htmTag.tagClass == 'spnPickedSeat' ) )
        {
            // Yes, so take the seat
            htmSeat = document.getElementById(htmTag.id);
            
            // Available?
            if (htmSeat.className == "spnAvailableSeat")
            {
                // Yes, so make Picked
                htmSeat.className = "spnPickedSeat";
            }
            else if (htmSeat.className == "spnPickedSeat")
            {
                // Picked, so make Available
                htmSeat.className = "spnAvailableSeat";
            }
        };

        return true;
    };
}



// --------------------------------------------------------------------------------
// Name:        GetListOfPickedSeats
// Abstract:    Click handler for seats
// --------------------------------------------------------------------------------
function GetListOfPickedSeats()
{
    var blnResult = false;
    var intIndex = 0;
    var htmRowSeat = document.getElementById("hfRowSeat");

    // Clear
    htmRowSeat.value = '';

    // Get all seats
    var htmTag = document.getElementsByTagName("span");

    // Go throug all seats
    for (intIndex = 0; intIndex < htmTag.length; intIndex += 1)
    {
        // Picked seat?
        if (htmTag[intIndex].className == "spnPickedSeat")
        {
            // Yes, so save result
            htmRowSeat.value += htmTag[intIndex].id + ',';
        }
    }

    // Any seats picked?
    if (htmRowSeat.value != '')
    {
        blnResult = true;
    }

    return blnResult;
}



// --------------------------------------------------------------------------------
// Name:        SelectPickedAndDisplayTakenSeats
// Abstract:    Select Seats that have been picked and display 
// taken seat message or error message if none has been taken
// --------------------------------------------------------------------------------
function SelectPickedAndDisplayTakenSeats()
{
    var intPickedSeatIndex = 0;
    var intDisplayedSeatIndex = 0;
    var astrUserPickedSeats = [];
    var htmRowSeat = document.getElementById("hfRowSeat");
    var strTakenSeatsMessage = 'OOOOOPPPSS!\nSomebody just picked the seat(s)\n';
    var strRowSeat = '';
    var blnUnavailableFound = false;

    // Get all seats
    var htmTag = document.getElementsByTagName("span");
    
    astrUserPickedSeats = htmRowSeat.value.split(",");
    
    // Go through all picked by user seats list
    for (intPickedSeatIndex = 0; intPickedSeatIndex < astrUserPickedSeats.length; intPickedSeatIndex += 1)
    {
        // Go throug all displayed seats
        for (intDisplayedSeatIndex = 0; intDisplayedSeatIndex < htmTag.length; intDisplayedSeatIndex += 1)
        {
            // Found a matching seat?
            if (htmTag[intDisplayedSeatIndex].id == astrUserPickedSeats[intPickedSeatIndex])
            {
                // Yes, Available?
                if (htmTag[intDisplayedSeatIndex].className == "spnAvailableSeat")
                {
                    // Yes, so make Picked
                    htmTag[intDisplayedSeatIndex].className = "spnPickedSeat";
                }
                else
                {
                    // No - Unavailable, so add to displaying message
                    strRowSeat = astrUserPickedSeats[intPickedSeatIndex].toString();
                    strTakenSeatsMessage += CreateTakenSeatMessage(strRowSeat);
                    blnUnavailableFound = true;
                }
            }
        }
    }

    // Taken seat found?
    if (blnUnavailableFound == true)
    {
        // Yes, so finish the message and display
        strTakenSeatsMessage += '\n\nPlease pick another';
        alert(strTakenSeatsMessage);
    }
    else
    {
        // Just display error messge
        alert('Unexpected error occured :(\n' + 'Try again please');
    }

    return true;
}



// --------------------------------------------------------------------------------
// Name:        CreateTakenSeatMessage
// Abstract:    Create a 'Row# Seat#' message
// --------------------------------------------------------------------------------
function CreateTakenSeatMessage( strRowSeat )
{
    var strTakenSeatsMessage = '';
    var intIndexOfDash = 0;

    intIndexOfDash = strRowSeat.indexOf('-', 0);

    strTakenSeatsMessage += '\nRow #'
                          + strRowSeat.substring(0, intIndexOfDash)
                          + ' Seat #'
                          + strRowSeat.substring(intIndexOfDash + 1, 22);

    return strTakenSeatsMessage;
}