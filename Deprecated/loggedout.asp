<% @LANGUAGE=JavaScript %>
<!-- #include FILE="_template1.asp" --->
<%
transitpage = true    // don't remember this page!

//===============================================================
//
//===============================================================
function RenderBody()
  {
  Response.Write("<P CLASS=b24-report-banner>You are now logged-out.</P>")

  Response.Write("<P>Click here to <A HREF=\"login.asp?ic=1\">login again</A>.</P>")
  }

//function RenderMenu() {}
//function RenderDevMenu() {}
//function RenderTop() {}


%>
<!-- #include FILE="_template2.asp" -->
<HEAD>
