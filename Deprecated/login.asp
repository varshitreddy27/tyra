<% @LANGUAGE="JavaScript" %>
<!-- #include FILE="_template1.asp" -->
<!-- #include VIRTUAL="/zinc/#input.asp" --->
<%
transitpage = true

function RenderMenu() {} // disable menu on this page

//===============================================================
//
//===============================================================
function RenderTop()
  {
  var page = new XMLPage()
  if(page)
    {
    var params = new Array()
    params[params.length] = new XSLParam("b24.Appname", "sales3");
    params[params.length] = new XSLParam("method", ""+Application("method"))

    //--- get the page layout template ---
    var pagetemplate = new XMLFile(xmlfolder+"docbook_menu.xml")
    XMLDebugWrite(pagetemplate)
    page.Add(pagetemplate)

    //--- process it ---
    RenderObjectXML(page, null, xslfolder+"report.xsl", params,Response)
    }

  //Response.Write(new Url("Logout", "abandonsession.asp").Str())
  }

//===============================================================
//
//===============================================================
function RenderBody()
  {
  if(isoffline)
    {
    Response.Write("<P CLASS=b24-report-banner>The Sales Interface is currently OFFLINE</P>")
    return
    }

  //--- check for errors --
  if(b24session && b24session.errors.Count())
    Response.Write(b24session.errors.Str())

  var loginform = LoginForm()
  if(loginform)
    Response.Write("<DIV>"+loginform.Str(/*dotabular*/true)+"</DIV>")
  }


//===============================================================
//
//===============================================================
function LoginForm()
  {
  var outobj = null

  var _form = new FormCtrl("loginform", "POST", thispage, null, /*disabled*/false)
  if(_form)
    {
    _form.Add(new TextCtrl("usr", 20, /*maxlength*/null, /*value*/username, /*disabled*/false, "Username"))
    _form.Add(new PasswordCtrl("pwd", 20, /*maxlength*/null, /*value*/userpassword, /*disabled*/false, "Password"))
    _form.Add(new HiddenCtrl("ic", "1"))
    _form.Add(new SubmitCtrl(null, "Login"))
    outobj = _form
    }

  return outobj
  }

%>
<!-- #include FILE="_template2.asp" -->

