using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using B24.Common;
using B24.Common.Web;
using B24.Common.Logs;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using B24.Sales3.UI;

namespace B24.Sales3.UserControl
{
    public partial class AdvanceReport : System.Web.UI.UserControl
    {
        #region Private Members

        private Logger logger = new Logger(Logger.LoggerType.Sales3);
        private GlobalVariables global = GlobalVariables.GetInstance();
        private B24.Common.Web.BasePage basePage;
        private string task = string.Empty;
        private string reportName = string.Empty;
        private Guid reportQId = Guid.Empty;
        private Subscription subscription;
        private int userInputCount;

        /// <summary>
        /// Enum for Input control type
        /// </summary>
        private enum UIFieldType
        {
            ComboBoxInteger = 1,
            ComboBoxString = 2,
            DataTime = 3,
            TextBox = 5
        }

        /// <summary>
        /// Enum for Report Task
        /// </summary>
        private enum ReportTask
        {
            ViewRun,
            Run,
            DownLoad,
            Delete,
            View
        }

        #endregion

        #region Public Property

        public Subscription RequestorSubscription
        {
            get { return subscription; }
            set { subscription = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Page Load event
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                basePage = this.Page as B24.Common.Web.BasePage;
                GetValuesFromHiddenField();
                InitPopulate();
                LoadReport();
            }
            catch (Exception exception)
            {
                Response.Redirect("Error.aspx?message=" + exception.Message);
            }
        }

        /// <summary>
        /// Change row binding for report category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AvailableReportGridview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label descriptionLabel = (Label)e.Row.FindControl("DescriptionLabel");
                Label categoryLabel = (Label)e.Row.FindControl("CategoryLabel");
                Label reportNameLabel = (Label)e.Row.FindControl("ReportNameLabel");
                HiddenField reportNameHiddenFld = (HiddenField)e.Row.FindControl("ReportNameHiddenField");
                LinkButton runLinkButton = (LinkButton)e.Row.FindControl("RunLinkButton");

                runLinkButton.OnClientClick = "SetReportName('" + reportNameHiddenFld.Value + "','ViewRun','" + descriptionLabel.Text + "')";

                // To Show report Category
                if (!string.IsNullOrEmpty(categoryLabel.Text) && string.IsNullOrEmpty(descriptionLabel.Text) && string.IsNullOrEmpty(reportNameLabel.Text))
                {
                    CssStyleCollection style = e.Row.Cells[0].Style;
                    style.Value = "font-weight:bold;background-color:#f4f5f9";
                    descriptionLabel.Text = categoryLabel.Text;
                    descriptionLabel.Font.Bold = true;
                    e.Row.Cells[0].ColumnSpan = 5;
                    // To Remove extra cells for category row
                    while (e.Row.Cells.Count > 1)
                    {
                        e.Row.Cells.RemoveAt(1);
                    }
                }
            }
        }

        /// <summary>
        /// To Change complete status and assign click event for View, Delete, Download links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AvailableResultGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label completedLabel = (Label)e.Row.FindControl("CompletedLabel");
                LinkButton downLoadLinkButton = (LinkButton)e.Row.FindControl("DownLoadLinkButton");
                LinkButton deleteLinkButton = (LinkButton)e.Row.FindControl("DeleteLinkButton");
                LinkButton viewLinkButton = (LinkButton)e.Row.FindControl("ViewLinkButton");

                if (completedLabel.Text == "Pending")
                {
                    e.Row.Controls.Remove(viewLinkButton);
                    e.Row.Controls.Remove(downLoadLinkButton);
                    e.Row.Controls.Remove(deleteLinkButton);
                    e.Row.Cells.RemoveAt(2);
                }
                else
                {
                    HiddenField reportQIdHidden = (HiddenField)e.Row.FindControl("ReportQidHiddenField");
                    viewLinkButton.OnClientClick = "SetReportQId('" + reportQIdHidden.Value + "','View')";
                    deleteLinkButton.OnClientClick = "SetReportQId('" + reportQIdHidden.Value + "','Delete')";
                    downLoadLinkButton.OnClientClick = "SetReportQId('" + reportQIdHidden.Value + "','DownLoad')";
                }
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// To Load the report
        /// </summary>
        private void LoadReport()
        {
            int reportType = 5;
            Guid availableResultGuidParam = basePage.User.UserID;
            string paramPrefix = "availablegeneral ";
            if (subscription != null)
            {
                reportType = 3;
                availableResultGuidParam = subscription.SubscriptionID;
                paramPrefix = "availableSched ";
            }
            ReportFactory reportfactory = new ReportFactory(global.UserConnStr);
            List<B24.Common.Report> reportList = reportfactory.GetAvailableReports(basePage.User.UserID, reportType);
            AvailableReportGridview.DataSource = reportList;
            AvailableReportGridview.DataBind();

            //List<B24.Common.Report> resultList = reportfactory.GetReportResults(availableResultGuidParam, paramPrefix);
            //if (resultList.Count > 0)
            //{
            //    NoResultLabel.Visible = false;
            //    AvailableResultGridView.DataSource = resultList;
            //    AvailableResultGridView.DataBind();
            //}
            //else
            //{
                NoResultLabel.Visible = true;
                ResultHelpTextLabel.Visible = false;
                AvailableResultGridView.Visible = false;
            //}
        }

        /// <summary>
        /// Get all query string, form and hidden field values here
        /// </summary>
        private void GetValuesFromHiddenField()
        {
            if (!string.IsNullOrEmpty(TaskHiddenField.Value))
            {
                task = TaskHiddenField.Value;
            }
            if (!string.IsNullOrEmpty(ReportSPNameHiddenField.Value))
            {
                reportName = ReportSPNameHiddenField.Value;
            }
            if (!string.IsNullOrEmpty(ReportQueueIDHiddenField.Value))
            {
                reportQId = new Guid(ReportQueueIDHiddenField.Value);
            }
        }

        /// <summary>
        /// Populate corresponding view and control load based on Tasks
        /// For Task = View -> Show the report result for give report id
        /// For Task = Run -> Run the report for given report name
        /// For Task = DownLoad -> Provide report result as downloadable format
        /// For Task = ViewRun -> To Show the report parameter UI to run report
        /// For Task = Delete -> Delete the report for given report id
        /// </summary>
        private void InitPopulate()
        {
            try
            {
                ReportView.ActiveViewIndex = 0;

                if (task == ReportTask.ViewRun.ToString())
                {
                    SetParameterUI();
                    if (userInputCount > 0)
                    {
                        TaskHiddenField.Value = ReportTask.Run.ToString();
                        ReportView.ActiveViewIndex = 1;
                    }
                    else
                    {
                        RunReport();
                    }
                    ResultSubmittedPanel.Visible = false;
                    CancelButton.Text = "Cancel";
                    ReportDescriptionLabel.Text = ReportNameHiddenField.Value;
                    AnotherDateRangeButton.Visible = false;
                }
                else if (task == ReportTask.Run.ToString())
                {
                    RunReport();
                }
                else if (task == ReportTask.DownLoad.ToString())
                {
                    ReportFactory reportFactory = new ReportFactory(global.UserConnStr);
                    string reportString = reportFactory.GetReportView(reportQId, ReportTask.DownLoad.ToString());
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=b24_report_" + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.xls";
                    string styleString = "<html> <head><style type='text/css'> .b24-doc-title {font-size:22px;font-weight:bold;color:#008000;}";
                    styleString += ".b24-report-banner {color:#003300;font-size:14px;font-weight:bold;margin-top:10px;margin-bottom:5px;}";
                    styleString += ".b24-report {font-size:11px; background:#808080;color:#000000;margin-left:0px;margin-top:0px;margin-bottom:10px;}";
                    styleString += ".b24-report-title {background:#99CC99; color:#000000;}";
                    styleString += ".b24-report-data {font-size:9px;background:#FFFFFF;color:#000000;text-align:left;}";
                    styleString += "</style> </head> <body>";
                    reportString = styleString + reportString + "</body> </html>";
                    Response.Write(reportString);
                    Response.Flush();
                }
                else if (task == ReportTask.Delete.ToString())
                {
                    ReportFactory reportFactory = new ReportFactory(global.UserConnStr);
                    if (subscription != null)
                    {
                        reportFactory.DeleteReport(subscription.SubscriptionID, reportQId);
                    }
                    else
                    {
                        reportFactory.DeleteReport(basePage.User.UserID, reportQId);
                    }
                }
                else if (task == ReportTask.View.ToString())
                {
                    ReportFactory reportFactory = new ReportFactory(global.UserConnStr);
                    string reportHTML = reportFactory.GetReportView(reportQId, ReportTask.View.ToString());
                    ReportResultViewPlaceHolder.Controls.Add(new LiteralControl(reportHTML));
                    ReportView.ActiveViewIndex = 2;
                }
            }
            catch (SqlException sqlException)
            {
                logger.Log(Logger.LogLevel.Error, " InitPopulate SqlException ", sqlException);
                return;
            }
            catch (ArgumentNullException argumentNullException)
            {
                logger.Log(Logger.LogLevel.Error, " InitPopulate ArgumentNullException ", argumentNullException);
                return;
            }
        }

        /// <summary>
        /// Get stored procedure Parameter detail by using GetReportParamDetails 
        /// Get parameter UI details by using GetReportUIDetails
        /// Create UI by calling CreateDynamicControls method with parameter detail
        /// </summary>
        private void SetParameterUI()
        {
            try
            {
                ReportFactory reportFactory = new ReportFactory(global.UserConnStr);
                SqlCommand reportParameterDetails = reportFactory.GetReportParamDetails(reportName);
                List<ReportParameter> reportParameterUI = reportFactory.GetReportUIDetails(basePage.User.UserID, (subscription == null) ? basePage.User.UserID : subscription.SubscriptionID, reportName);
                userInputCount = 0;
                foreach (SqlParameter parameter in reportParameterDetails.Parameters)
                {
                    if (parameter.ParameterName != "@RETURN_VALUE" &&
                        parameter.ParameterName != "@userid" &&
                        parameter.ParameterName != "@internalrequesterid" &&
                        parameter.ParameterName != "@noemail" &&
                        parameter.ParameterName != "@pwdroot" &&
                        parameter.ParameterName != "@schedule")
                    {
                        CreateDynamicControls(parameter.ParameterName, reportParameterUI);
                    }
                }
                ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<tr>"));
                ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<td colspan='2'>"));
                Button reportRunButton = new Button();
                reportRunButton.ID = "RunButtton";
                reportRunButton.Text = "Run";
                //reportRunButton.OnClientClick = "ChangeTask('ViewRun')" ;
                ReportParameterPlaceHolder.Controls.Add(reportRunButton);
                ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td>"));
                ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</tr>"));
                ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</table>"));
            }
            catch (ArgumentNullException argumentNullException)
            {
                logger.Log(Logger.LogLevel.Error, " SetParameterUI argumentNullException ", argumentNullException);
                return;
            }
            catch (SqlException sqlException)
            {
                logger.Log(Logger.LogLevel.Error, " SetParameterUI sqlException ", sqlException);
                return;
            }
        }

        /// <summary>
        /// To Create Dynamic Control for Report input
        /// Get the parameter UI Details from this method parameter reportParameterUI
        /// Create Dynamic controls for each parameters other than userid,internalrequesterid,noemail,schedule,pwdroot
        /// Add Dynamic controls name and clientid to hidden field to get their values while postback.
        /// Control will be decided based on Parametertype 3 -> Datetime, 2 -> Combo box, 5 -> Text box
        /// Increment the userInputCount variable for each dynamic control.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="reportParamterUI"></param>
        private void CreateDynamicControls(string paramName, List<ReportParameter> reportParamterUI)
        {
            try
            {
                foreach (ReportParameter reportParameter in reportParamterUI)
                {
                    if ("@" + reportParameter.ParameterName == paramName)
                    {
                        string controlId = reportParameter.ParameterName;
                        if (userInputCount == 0)
                        {
                            ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<table>"));
                        }
                        ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<tr>"));

                        switch (reportParameter.ParamterType)
                        {
                            case (int)UIFieldType.ComboBoxInteger:
                                controlId = controlId + Convert.ToString((int)UIFieldType.ComboBoxInteger, CultureInfo.InvariantCulture);
                                DropDownList ddIntegerList = (DropDownList)ReportParameterView.FindControl("DD" + controlId);
                                if (ddIntegerList == null)
                                {
                                    Label paramLabel = new Label();
                                    paramLabel.Text = reportParameter.ParameterDescription;
                                    ddIntegerList = new DropDownList();
                                    ddIntegerList.ID = "DD" + controlId;
                                    HiddenField hiddenField = new HiddenField();
                                    hiddenField.ID = controlId;
                                    hiddenField.Value = reportParameter.ActualValue;
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<td>"));
                                    ReportParameterPlaceHolder.Controls.Add(paramLabel);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td><td>"));
                                    ReportParameterPlaceHolder.Controls.Add(ddIntegerList);
                                    ReportParameterPlaceHolder.Controls.Add(hiddenField);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td>"));
                                    ddIntegerList.Attributes["onchange"] = "GetDropDownValue('" + ddIntegerList.ClientID + "','" + hiddenField.ClientID + "');";
                                    if (string.IsNullOrEmpty(DynamicFieldNames.Value))
                                        DynamicFieldNames.Value = hiddenField.ClientID + "#" + hiddenField.ID;
                                    else
                                        DynamicFieldNames.Value = DynamicFieldNames.Value + "@" + hiddenField.ClientID + "#" + hiddenField.ID;
                                    userInputCount++;
                                }

                                ddIntegerList.Items.Add(new ListItem(reportParameter.DisplayValue, reportParameter.ActualValue));

                                break;
                            case (int)UIFieldType.ComboBoxString:
                                controlId = controlId + Convert.ToString((int)UIFieldType.ComboBoxString, CultureInfo.InvariantCulture);
                                DropDownList ddList = (DropDownList)ReportParameterView.FindControl(controlId);
                                if (ddList == null)
                                {
                                    Label paramLabel = new Label();
                                    paramLabel.Text = reportParameter.ParameterDescription;
                                    ddList = new DropDownList();
                                    ddList.ID = controlId;
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<td>"));
                                    ReportParameterPlaceHolder.Controls.Add(paramLabel);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td><td>"));
                                    ReportParameterPlaceHolder.Controls.Add(ddList);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td>"));
                                    if (string.IsNullOrEmpty(DynamicFieldNames.Value))
                                        DynamicFieldNames.Value = ddList.ClientID + "#" + ddList.ID;
                                    else
                                        DynamicFieldNames.Value = DynamicFieldNames.Value + "@" + ddList.ClientID + "#" + ddList.ID;
                                    userInputCount++;
                                }
                                ddList.Items.Add(new ListItem(reportParameter.DisplayValue, reportParameter.ActualValue));
                                break;
                            case (int)UIFieldType.TextBox:
                                controlId = controlId + Convert.ToString((int)UIFieldType.TextBox, CultureInfo.InvariantCulture);
                                TextBox paramTextBox = (TextBox)ReportParameterView.FindControl(controlId);
                                if (paramTextBox == null)
                                {
                                    Label paramLabel = new Label();
                                    paramLabel.Text = reportParameter.ParameterDescription;
                                    paramTextBox = new TextBox();
                                    paramTextBox.ID = controlId;
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<td>"));
                                    ReportParameterPlaceHolder.Controls.Add(paramLabel);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td><td>"));
                                    ReportParameterPlaceHolder.Controls.Add(paramTextBox);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td>"));
                                    if (string.IsNullOrEmpty(DynamicFieldNames.Value))
                                        DynamicFieldNames.Value = paramTextBox.ClientID + "#" + paramTextBox.ID;
                                    else
                                        DynamicFieldNames.Value = DynamicFieldNames.Value + "@" + paramTextBox.ClientID + "#" + paramTextBox.ID;
                                    userInputCount++;
                                }
                                paramTextBox.Text = reportParameter.DisplayValue;
                                break;
                            case (int)UIFieldType.DataTime:
                                controlId = controlId + Convert.ToString((int)UIFieldType.DataTime, CultureInfo.InvariantCulture);
                                TextBox paramCalendarTxext = (TextBox)ReportParameterView.FindControl(controlId);
                                if (paramCalendarTxext == null)
                                {
                                    Label paramLabel = new Label();
                                    paramLabel.Text = reportParameter.ParameterDescription;
                                    TextBox calendarTextBox = new TextBox();
                                    calendarTextBox.ID = controlId;
                                    Image imageCalendar = new Image();
                                    imageCalendar.ID = "Image" + controlId;
                                    imageCalendar.ImageUrl = "~/images/Calendar.gif";
                                    AjaxControlToolkit.CalendarExtender paramCalendar = new AjaxControlToolkit.CalendarExtender();
                                    paramCalendar.ID = "Calendar" + controlId;
                                    paramCalendar.TargetControlID = calendarTextBox.ID;
                                    paramCalendar.PopupButtonID = imageCalendar.ID;
                                    paramCalendar.Format = "MMM dd, yyyy";
                                    if (paramName == "@startdateinput")
                                    {
                                        calendarTextBox.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
                                    }
                                    else
                                    {
                                        calendarTextBox.Text = DateTime.Now.AddDays(-1).ToString("MMM dd, yyyy", CultureInfo.InvariantCulture);
                                    }
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("<td>"));
                                    ReportParameterPlaceHolder.Controls.Add(paramLabel);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td><td>"));
                                    ReportParameterPlaceHolder.Controls.Add(calendarTextBox);
                                    ReportParameterPlaceHolder.Controls.Add(imageCalendar);
                                    ReportParameterPlaceHolder.Controls.Add(paramCalendar);
                                    ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</td>"));
                                    if (string.IsNullOrEmpty(DynamicFieldNames.Value))
                                        DynamicFieldNames.Value = calendarTextBox.ClientID + "#" + calendarTextBox.ID;
                                    else
                                        DynamicFieldNames.Value = DynamicFieldNames.Value + "@" + calendarTextBox.ClientID + "#" + calendarTextBox.ID;

                                    userInputCount++;
                                }
                                break;
                        }
                        ReportParameterPlaceHolder.Controls.Add(new LiteralControl("</tr>"));
                    }
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                logger.Log(Logger.LogLevel.Error, " CreateDynamicControls argumentNullException ", argumentNullException);
                return;
            }
        }

        /// <summary>
        /// To get the parameter details for given store procedure name
        /// Then Assign values for userid,internalrequesterid,noemail,schedule,pwdroot
        /// Then Read other field values from dynamic control
        /// Run the report with all parameter values.
        /// </summary>
        private void RunReport()
        {
            try
            {
                ReportFactory reportFactory = new ReportFactory(global.UserConnStr);
                SqlCommand reportParameterDetails = reportFactory.GetReportParamDetails(reportName);
                foreach (SqlParameter param in reportParameterDetails.Parameters)
                {
                    switch (param.ParameterName)
                    {
                        case "@userid":
                            if (subscription == null)
                                reportParameterDetails.Parameters[param.ParameterName].Value = basePage.User.UserID;
                            else
                                reportParameterDetails.Parameters[param.ParameterName].Value = subscription.SubscriptionID;
                            break;
                        case "@internalrequesterid":
                            reportParameterDetails.Parameters[param.ParameterName].Value = basePage.User.UserID;
                            break;
                        case "@noemail":
                            reportParameterDetails.Parameters[param.ParameterName].Value = 0;
                            break;
                        case "@schedule":
                            reportParameterDetails.Parameters[param.ParameterName].Value = "runonce";
                            break;
                        case "@pwroot":
                            if (subscription == null)
                            {
                                reportParameterDetails.Parameters[param.ParameterName].Value = null;
                            }
                            else
                            {
                                reportParameterDetails.Parameters[param.ParameterName].Value = subscription.PasswordRoot;
                            }
                            break;
                        case "@RETURN_VALUE":
                            reportParameterDetails.Parameters[param.ParameterName].Value = DBNull.Value;
                            break;
                        default:
                            // get dynamic field values
                            string paramValue = GetValuesForParameter(param);
                            if (String.IsNullOrEmpty(paramValue))
                            {
                                reportParameterDetails.Parameters[param.ParameterName].Value = null;
                            }
                            else
                            {
                                reportParameterDetails.Parameters[param.ParameterName].Value = paramValue;
                            }
                            break;
                    }
                }
                // To Run the report
                reportParameterDetails.CommandText = reportName;
                reportParameterDetails.CommandType = CommandType.StoredProcedure;
                string runResult = reportFactory.RunReport(reportParameterDetails);
                ResultSubmittedPanel.Controls.Add(new LiteralControl(runResult));
                ResultSubmittedPanel.Visible = true;
                CancelButton.Text = "Ok";
                AnotherDateRangeButton.Visible = true;
                ReportView.ActiveViewIndex = 1;
            }
            catch (SqlException sqlException)
            {
                logger.Log(Logger.LogLevel.Error, " RunReport SqlException ", sqlException);
                return;
            }

        }

        /// <summary>
        /// To get the Client Id from hidden field and getting values of dynamic user input controls
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetValuesForParameter(SqlParameter param)
        {
            string dynamicFieldNames = DynamicFieldNames.Value;
            string[] paramFieldNames = dynamicFieldNames.Split('@');
            string textBoxId = param.ParameterName.Substring(1) + Convert.ToString((int)UIFieldType.TextBox, CultureInfo.InvariantCulture);
            string comboBoxId = param.ParameterName.Substring(1) + Convert.ToString((int)UIFieldType.ComboBoxString, CultureInfo.InvariantCulture);
            string DateFieldId = param.ParameterName.Substring(1) + Convert.ToString((int)UIFieldType.DataTime, CultureInfo.InvariantCulture);
            string comboIntegerId = param.ParameterName.Substring(1) + Convert.ToString((int)UIFieldType.ComboBoxInteger, CultureInfo.InvariantCulture);

            string paramInput = string.Empty;
            foreach (string paramNames in paramFieldNames)
            {
                string[] paramName = paramNames.Split('#');
                if (textBoxId == paramName[1] || comboBoxId == paramName[1] || DateFieldId == paramName[1] || comboIntegerId == paramName[1])
                {
                    //Client Id get replaced with $ for _ symbol when it is postpack
                    paramInput = Request.Form[paramName[0].Replace('_', '$')];
                    break;
                }
            }
            return paramInput;
        }

        #endregion
    }
}