<% @LANGUAGE=Javascript %>
<!-- #include FILE="_common.asp" --->
<%
//-----------------------------------------------
// the following line allows you to force the
// sessionstate recored to be flushed by adding
// a ?fs=1 parameter to the url
//-----------------------------------------------
var flushstate = (Request.QueryString("fs").Count>0)? true : false

state.Abandon()

var rurl = "loggedout.asp" //"default.asp"
DoLogout(flushstate)
DebugWrite("rurl = "+rurl+"<br>")
B24Redirect(rurl)
%>
<HTML>
<HEAD>
<TITLE><%=Application("title") %>Abandon Session (Tool)</TITLE>
</HEAD>
<BODY>
<P> Session has been dropped
</BODY>
