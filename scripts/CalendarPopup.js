var datePickerDivID = "datepicker";
var iFrameDivID = "datepickeriframe";

var dayArrayShort = new Array('Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa');
var dayArrayMed = new Array('Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat');
var dayArrayLong = new Array('Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday');
var monthArrayShort = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec');
var monthArrayMed = new Array('Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'July', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec');
var monthArrayLong = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
  
// these variables define the date formatting we're expecting and outputting.
// If you want to use a different format by default, change the defaultDateSeparator
// and defaultDateFormat variables either here or on your HTML page.
var defaultDateSeparator = "-";		// common values would be "/" or "."
var defaultDateFormat = "dmy"	// valid values are "mdy", "dmy", and "ymd"
var dateSeparator = defaultDateSeparator;
var dateFormat = defaultDateFormat;

/**
This is the main function you'll call from the onClick event of a button.
Normally, you'll have something like this on your HTML page:

Start Date: <input name="StartDate"> 
<input type=button value="select" onclick="displayDatePicker('StartDate');">

That will cause the datepicker to be displayed beneath the StartDate field and
any date that is chosen will update the value of that field. If you'd rather have the
datepicker display beneath the button that was clicked, you can code the button:
like this:

<input type=button value="select" onclick="displayDatePicker('StartDate', this);">

So, pretty much, the first argument (dateFieldName) is a string representing the
name of the field that will be modified if the user picks a date, and the second
argument (displayBelowThisObject) is optional and represents an actual node
on the HTML document that the datepicker should be displayed below.

In version 1.1 of this code, the dtFormat and dtSep variables were added, allowing
you to use a specific date format or date separator for a given call to this function.
Normally, you'll just want to set these defaults globally with the defaultDateSeparator
and defaultDateFormat variables, but it doesn't hurt anything to add them as optional
parameters here. An example of use is:

<input type=button value="select" onclick="displayDatePicker('StartDate', false, 'dmy', '.');">

This would display the datepicker beneath the StartDate field (because the 
displayBelowThisObject parameter was false), and update the StartDate field with
the chosen value of the datepicker using a date format of dd.mm.yyyy
*/
function displayDatePicker(dateFieldName, displayBelowThisObject, dtFormat, dtSep, dtServerDate)
{
var dtSvrDate = new Date(dtServerDate);
  var targetDateField = document.getElementById(dateFieldName);

  // if we weren't told what node to display the datepicker beneath, just display it
  // beneath the date field we're updating
  if (!displayBelowThisObject)
    displayBelowThisObject = targetDateField;
  
  // if a date separator character was given, update the dateSeparator variable
  if (dtSep)
    dateSeparator = dtSep;
  else
    dateSeparator = defaultDateSeparator;
  
  // if a date format was given, update the dateFormat variable
  if (dtFormat)
    dateFormat = dtFormat;
  else
    dateFormat = defaultDateFormat;
  
  var x = displayBelowThisObject.offsetLeft;
  var y = displayBelowThisObject.offsetTop + displayBelowThisObject.offsetHeight;
  
  // deal with elements inside tables and such
  var parent = displayBelowThisObject;
  while (parent.offsetParent) {
    parent = parent.offsetParent;
    x += parent.offsetLeft;
    y += parent.offsetTop;
  }
  
  drawDatePicker(targetDateField, x, y, dtServerDate );
}


/**
Draw the datepicker object (which is just a table with calendar elements) at the
specified x and y coordinates, using the targetDateField object as the input tag
that will ultimately be populated with a date.

This function will normally be called by the displayDatePicker function.
*/
function drawDatePicker(targetDateField, x, y, dtServerDate)
{
  var dt = getFieldDate(targetDateField.value);
  var dtSvrDate = new Date(dtServerDate);
  if(dt.getYear() < dtSvrDate.getYear())
	{
	  dt = dtSvrDate;
	}
	else if((dt.getYear() == dtSvrDate.getYear()) && (dt.getMonth() < dtSvrDate.getMonth()))
	{
	  dt = dtSvrDate;
	}
	else if(
			(dt.getYear() == dtSvrDate.getYear())
		&& (dt.getMonth() <= dtSvrDate.getMonth())
		&& (dt.getDate() < dtSvrDate.getDate())
		)
	{
		dt = dtSvrDate;
	}
  

  
  // the datepicker table will be drawn inside of a <div> with an ID defined by the
  // global datePickerDivID variable. If such a div doesn't yet exist on the HTML
  // document we're working with, add one.
  if (!document.getElementById(datePickerDivID)) {
    // don't use innerHTML to update the body, because it can cause global variables
    // that are currently pointing to objects on the page to have bad references
    //document.body.innerHTML += "<div id='" + datePickerDivID + "' class='dpDiv'></div>";
    var newNode = document.createElement("div");
    newNode.setAttribute("id", datePickerDivID);
    newNode.setAttribute("class", "dpDiv");
    newNode.setAttribute("style", "visibility: hidden;");
    document.body.appendChild(newNode);   
  }
  
  // move the datepicker div to the proper x,y coordinate and toggle the visiblity
  var pickerDiv = document.getElementById(datePickerDivID);
  pickerDiv.style.position = "absolute";
  pickerDiv.style.left = x + "px";
  pickerDiv.style.top = y + "px";
  pickerDiv.style.visibility = (pickerDiv.style.visibility == "visible" ? "hidden" : "visible");
  pickerDiv.style.zIndex = 10000;
  
  // draw the datepicker table
  refreshDatePicker(targetDateField.name, dt.getFullYear(), dt.getMonth(), dt.getDate(), dtServerDate);
 
}


/**
This is the function that actually draws the datepicker calendar.
*/
function refreshDatePicker(dateFieldName, year, month, day, dtServerDate)
{
  // if no arguments are passed, use today's date; otherwise, month and year
  // are required (if a day is passed, it will be highlighted later)
  var thisDay = new Date();
  
  if ((month >= 0) && (year > 0)) {
    thisDay = new Date(year, month, 1);
  } else {
    day = thisDay.getDate();
    thisDay.setDate(1);
  }
  

  // the calendar will be drawn as a table
  // you can customize the table elements with a global CSS style sheet,
  // or by hardcoding style and formatting elements below
  var crlf = "\r\n";
  var TABLE = "<table cols=7 class='dpTable'>" + crlf;
  var xTABLE = "</table>" + crlf;
  var TR = "<tr class='dpTR'>";
  var TR_title = "<tr class='dpTitleTR'>";
  var TR_days = "<tr class='dpDayTR'>";
  var TR_todaybutton = "<tr class='dpTodayButtonTR'>";
  var xTR = "</tr>" + crlf;
  var TD = "<td class='dpTD'";	// leave this tag open, because we'll be adding an onClick event 
  var TDDisable = "<td class='dpTDDisable'>";
  var xTDDisable = "</td>";
  
  var TD_title = "<td colspan=5 class='dpTitleTD'>";
  var TD_buttons = "<td class='dpButtonTD'>";
  var TD_todaybutton = "<td colspan=7 class='dpTodayButtonTD'>";
  var TD_days = "<td class='dpDayTD'>";
  var TD_selected = "<td class='dpDayHighlightTD'";	// leave this tag open, because we'll be adding an onClick event
  var xTD = "</td>" + crlf;
  var DIV_title = "<div class='dpTitleText'>";
  var DIV = "<div align='center'>"
  var DIV_selected = "<div class='dpDayHighlight'>";
  var xDIV = "</div>";
  var TR_currLabel = "<tr class='dpCurrLabelTR'>";
  var TD_currLabel = "<td colspan=7>";
  
  // start generating the code for the calendar table
  var html = TABLE;
  
  // this is the title bar, which displays the month and the buttons to
  // go back to a previous month or forward to the next month
  html += TR_title;
  html += TD_buttons + getButtonCode(dateFieldName, thisDay, -1, "&lt;", dtServerDate) + xTD;
  html += TD_title + DIV_title + monthArrayLong[thisDay.getMonth()] + " " + thisDay.getFullYear() + xDIV + xTD;
  html += TD_buttons + getButtonCode(dateFieldName, thisDay, 1, "&gt;", dtServerDate) + xTD;
  html += xTR;
  
  // this is the row that indicates which day of the week we're on
  html += TR_days;
  for(i = 0; i < dayArrayShort.length; i++)
  {
    html += TD_days + dayArrayShort[i] + xTD;
  }
  html += xTR;
  
  // now we'll start populating the table with days of the month
  html += TR;
  
  // first, the leading blanks
  for (i = 0; i < thisDay.getDay(); i++)
    html += TD + "&nbsp;" + xTD;
  
  // now, the days of the month
  do {
    dayNum = thisDay.getDate();
    TD_onclick = " onclick=\"updateDateField('" + dateFieldName + "', '" + getDateString(thisDay) + "');\">";
    
    if (dayNum == day)
      html += TD_selected + TD_onclick + DIV_selected + dayNum + xDIV + xTD;
    else
	{		
		var dtSvrDate = new Date(dtServerDate);		

		if(thisDay.getMonth() == dtSvrDate.getMonth() && dayNum < (dtSvrDate.getDate()) && dtSvrDate.getYear() == thisDay.getYear())
		{
		    html += TDDisable + dayNum + xTDDisable;			
		}
		else
			html += TD + TD_onclick + dayNum + xTD;
		}
    
    // if this is a Saturday, start a new row
    if (thisDay.getDay() == 6)
      html += xTR + TR;
    
    // increment the day
    thisDay.setDate(thisDay.getDate() + 1);
  } while (thisDay.getDate() > 1)
  
  // fill in any trailing blanks
  if (thisDay.getDay() > 0) {
    for (i = 6; i > thisDay.getDay(); i--)
      html += TD + "&nbsp;" + xTD;      
  }
  html += xTR;
  
    
  // add a button to allow the user to easily return to today, or close the calendar
  var today = new Date();
  var month = today.getMonth() + 1;
  var day   = today.getDate();
  var year  = today.getYear();
  if (whatbrowser() == "NN6" && year < 1900)
  {
	 year += 1900;
  }
  if( month < 10 )
  {
	 month = '0' + month;
  }
  if( day < 10 )
  {
	day = '0' + day;
  }  
  var todayString = "Today : " + day + defaultDateSeparator + monthArrayShort[parseInt(month, 10) -1] + defaultDateSeparator + year;
  
  html += TR_currLabel + TD_currLabel 
  html += DIV + todayString  + xDIV 
  html += xTR + xTD; 
  
  html += TR_todaybutton + TD_todaybutton;  
 
  
   //html += "<button class='dpTodayButton' onClick='refreshDatePicker(\"" + dateFieldName + "\");'>this month</button> ";
  html += "<button class='dpTodayButton' onClick='updateDateField(\"" + dateFieldName + "\");'>close</button>";
  html += xTD + xTR; 
  
  // and finally, close the table
  html += xTABLE; 

  
  document.getElementById(datePickerDivID).innerHTML = html;
  // add an "iFrame shim" to allow the datepicker to display above selection lists
  adjustiFrame();
}


/**
Convenience function for writing the code for the buttons that bring us back or forward
a month.
*/
function getButtonCode(dateFieldName, dateVal, adjust, label, dtServerDate)
{
  var newMonth = (dateVal.getMonth() + adjust) % 12;
  var newYear = dateVal.getFullYear() + parseInt((dateVal.getMonth() + adjust) / 12);
  if (newMonth < 0) {
    newMonth += 12;
    newYear += -1;
  }
  var dtSvrDate = new Date(dtServerDate);
  if(dtSvrDate.getMonth() == dateVal.getMonth() && label =="&lt;" && dtSvrDate.getYear() == dateVal.getYear())
    return "<button disabled class='dpButtonDisable' onClick='refreshDatePicker(\"" + dateFieldName + "\", " + newYear + ", " + newMonth + ", null,\"" + dtServerDate + "\");'>" + label + "</button>";
  else  
  return "<button class='dpButton' onClick='refreshDatePicker(\"" + dateFieldName + "\", " + newYear + ", " + newMonth + ", null,\"" + dtServerDate + "\");'>" + label + "</button>";
}


/**
Convert a JavaScript Date object to a string, based on the dateFormat and dateSeparator
variables at the beginning of this script library.
*/
function getDateString(dateVal)
{
  var dayString = "00" + dateVal.getDate();
  var mthShortString = monthArrayShort[parseInt(dateVal.getMonth())];
  var monthString = "00" + (dateVal.getMonth()+1);
  dayString = dayString.substring(dayString.length - 2);
  monthString = monthString.substring(monthString.length - 2);
  
  switch (dateFormat) {
    case "dmy" :
      return dayString + dateSeparator + mthShortString + dateSeparator + dateVal.getFullYear();
    case "ymd" :
      return dateVal.getFullYear() + dateSeparator + mthShortString + dateSeparator + dayString;
    case "mdy" :
        return mthShortString + dateSeparator + dayString + dateSeparator + dateVal.getFullYear();
    default :
      return dayString + dateSeparator + mthShortString + dateSeparator + dateVal.getFullYear();
  }
}


/**
Convert a string to a JavaScript Date object.
*/
function getFieldDate(dateString)
{
  var dateVal;
  var dArray;
  var d, m, y;
  
  try {
    dArray = splitDateString(dateString);
    if (dArray) {
      switch (dateFormat) {
        case "dmy" :
          d = parseInt(dArray[0], 10);
          m = toMonth(dArray[1], 10);
          y = parseInt(dArray[2], 10);
          break;
        case "ymd" :
          d = parseInt(dArray[2], 10);
          m = toMonth(dArray[1], 10);
          y = parseInt(dArray[0], 10);
          break;
        case "mdy" :
          d = parseInt(dArray[1], 10);
          m = toMonth(dArray[0], 10);
          y = parseInt(dArray[2], 10);
        default :
          d = parseInt(dArray[1], 10);
          m = toMonth(dArray[0], 10);
          y = parseInt(dArray[2], 10);
          break;
      }
      dateVal = new Date(y, m, d);
    } else {
      // for defect 45063	
      if((dateString.length==0) && (navigator.platform.indexOf("Mac")==0)) 
      {
       dateVal = new Date();
      }
      else
      {
       dateVal = new Date(dateString);
      } 
    }
  } catch(e) {
    dateVal = new Date();
  }
  
  return dateVal;
}


/**
Try to split a date string into an array of elements, using common date separators.
If the date is split, an array is returned; otherwise, we just return false.
*/
function splitDateString(dateString)
{
  var dArray;
  if (dateString.indexOf("/") >= 0)
    dArray = dateString.split("/");
  else if (dateString.indexOf(".") >= 0)
    dArray = dateString.split(".");
  else if (dateString.indexOf("-") >= 0)
    dArray = dateString.split("-");
  else if (dateString.indexOf("\\") >= 0)
    dArray = dateString.split("\\");
  else
    dArray = false;
  
  return dArray;
}

/**
Update the field with the given dateFieldName with the dateString that has been passed,
and hide the datepicker. If no dateString is passed, just close the datepicker without
changing the field value.

Also, if the page developer has defined a function called datePickerClosed anywhere on
the page or in an imported library, we will attempt to run that function with the updated
field as a parameter. This can be used for such things as date validation, setting default
values for related fields, etc. For example, you might have a function like this to validate
a start date field:

function datePickerClosed(dateField)
{
  var dateObj = getFieldDate(dateField.value);
  var today = new Date();
  today = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  
  if (dateField.name == "StartDate") {
    if (dateObj < today) {
      // if the date is before today, alert the user and display the datepicker again
      alert("Please enter a date that is today or later");
      dateField.value = "";
      document.getElementById(datePickerDivID).style.visibility = "visible";
      adjustiFrame();
    } else {
      // if the date is okay, set the EndDate field to 7 days after the StartDate
      dateObj.setTime(dateObj.getTime() + (7 * 24 * 60 * 60 * 1000));
      var endDateField = document.getElementsByName("EndDate").item(0);
      endDateField.value = getDateString(dateObj);
    }
  }
}

*/
function updateDateField(dateFieldName, dateString)
{
  var targetDateField = document.getElementsByName(dateFieldName).item(0);
  if (dateString){
   
    targetDateField.value = dateString;
   
  }
  document.getElementById(datePickerDivID).style.visibility = "hidden";
  adjustiFrame();
   if (targetDateField.disabled == false){
       targetDateField.focus();
	}
  
  // after the datepicker has closed, optionally run a user-defined function called
  // datePickerClosed, passing the field that was just updated as a parameter
  // (note that this will only run if the user actually selected a date from the datepicker)
  if ((dateString) && (typeof(datePickerClosed) == "function"))
    datePickerClosed(targetDateField);
}


/**
Use an "iFrame shim" to deal with problems where the datepicker shows up behind
selection list elements, if they're below the datepicker. The problem and solution are
described at:

http://dotnetjunkies.com/WebLog/jking/archive/2003/07/21/488.aspx
http://dotnetjunkies.com/WebLog/jking/archive/2003/10/30/2975.aspx
*/
function adjustiFrame(pickerDiv, iFrameDiv)
{
  if (!document.getElementById(iFrameDivID)) {
    // don't use innerHTML to update the body, because it can cause global variables
    // that are currently pointing to objects on the page to have bad references
    //document.body.innerHTML += "<iframe id='" + iFrameDivID + "' src='javascript:false;' scrolling='no' frameborder='0'>";
    var newNode = document.createElement("iFrame");
    newNode.setAttribute("id", iFrameDivID);
    newNode.setAttribute("src", "javascript:false;");
    newNode.setAttribute("scrolling", "no");
    newNode.setAttribute("frameborder", "0");
    document.body.appendChild(newNode);
  }
  
  if (!pickerDiv)
    pickerDiv = document.getElementById(datePickerDivID);
  if (!iFrameDiv)
    iFrameDiv = document.getElementById(iFrameDivID);
  
  try {
    iFrameDiv.style.position = "absolute";
    iFrameDiv.style.width = pickerDiv.offsetWidth;
    iFrameDiv.style.height = pickerDiv.offsetHeight;
    iFrameDiv.style.top = pickerDiv.style.top;
    iFrameDiv.style.left = pickerDiv.style.left;
    iFrameDiv.style.zIndex = pickerDiv.style.zIndex - 1;
    iFrameDiv.style.visibility = pickerDiv.style.visibility;
  } catch(e) {
  }
}

/****************************************************************************

// Function added by Virtusa CHN ATC for checking the browser.

*****************************************************************************/

function whatbrowser(){
        
        var thisBrowser = "Mozilla";
        
        if(document.layers){
            thisbrowser="NN4";
        }
        if(document.all){
            thisbrowser="ie";
        }
        if(!document.all && document.getElementById){
            thisbrowser="NN6";
        }
        
		return thisbrowser;
}
// CR023 Timezone enhancements
//returns the corresponding month in terms of number to the abbreviation myAbbr
//pre: myAbbr is a standard, 3-character abbreviation
//post: full name string is returned
function toMonth(myAbbr){
   switch (myAbbr){
      case "Jan": return "00"; break;
      case "Feb": return "01"; break;
      case "Mar": return "02"; break;
      case "Apr": return "03"; break;
      case "May": return "04"; break;
      case "Jun": return "05"; break;
      case "Jul": return "06"; break;
      case "Aug": return "07"; break;
      case "Sep": return "08"; break;
      case "Oct": return "09"; break;
      case "Nov": return "10"; break;
      case "Dec": return "11"; break;
      default: return ""; break;   
   }
}


