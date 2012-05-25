using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml;  
using CrystalDecisions.CrystalReports.Engine;

namespace SimplicityReportTest
{
    public partial class ShowReport : System.Web.UI.Page
    {
        public const string INVOICE_ID = "invoiceId";
        public const string Job_ID = "jobId";
        public const string REPORT_TYPE = "reportType";  


        private string reportType = "";
        DataSet dsReport = null;
        DataSet dsTempReport = null;
        DataSet ds = new DataSet();
        AuthenticationObject auth;

        private HashSet<Report> reportsHash = new HashSet<Report>();
        private Dictionary<String, Report> reportsDict = new Dictionary<string, Report>(); //Dictionary ko aik string jaye gee and report jaye gee
        String path = "C:\\SimplicityReportRecords"; // reports ka path
        System.IO.DirectoryInfo dir = null;       //Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.
        private string RequestedEnvironment = "";

        private List<ReportParameter> reportParameter = new List<ReportParameter>();  //list bana lee report parameter kii (parameter name and value)
        bool error = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session[Authenticate.CALLER_URL] = null; 
            dsReport = new SimplicityDBSchema();
            dsTempReport = new DataSet();

            if (Request["environment"] != null) //request for page environment
            {
                RequestedEnvironment = Request["environment"];
            }
            else
                RequestedEnvironment = Authenticate.PRODUCTION; //put production in requested Environment


            errorLabel.Text = "";
            if (SchemaUtilty.isDebugMode)
                TextBox1.Visible = true;
            try
            {
                dir = new System.IO.DirectoryInfo(path); //dir k ander reports ka path rakh day ga 
                if (!dir.Exists)
                    dir.Create(); 

            }
            catch (Exception ex)
            {
                TextBox1.Text += "\n" + ex.Message;
            }

            TextBox1.Text += "\nOutside auth"; 
            auth = (AuthenticationObject)Session[Authenticate.ACCESS_TOKEN];//////Taking Authentication from Session
            if (auth != null && Session[Authenticate.AUTHENTICATED_ENVIRONMENT] != null && Session[Authenticate.AUTHENTICATED_ENVIRONMENT].ToString().CompareTo(RequestedEnvironment) == 0)////Check Authenticated and Environment
            {
                TextBox1.Text += "\nInside auth";
                reportType = Request[REPORT_TYPE];
                if (reportType != null && reportType.Length > 0)
                {
                    TextBox1.Text += "\nInside report Type " + reportType;
                    ReadXMLConfiguration();

                    if (reportsDict.ContainsKey(reportType))
                    {
                        Report report = reportsDict[reportType];
                        TextBox1.Text += "\nReport_TYPE" + reportType;

                        foreach (ReportParameter param in report.ReportParamaters)
                        {
                            ReportParameter temp_param = new ReportParameter();
                            temp_param.ParameterName = param.ParameterName;
                            if (Request[param.ParameterName] != null)
                            {
                                temp_param.Value = Request[param.ParameterName];
                                reportParameter.Add(temp_param);
                                TextBox1.Text += "\n" + temp_param.ParameterName + " = " + temp_param.Value;
                            }
                            else
                            {
                                error = true;
                                ShowError("Missing report parameter " + temp_param.ParameterName);
                            }
                        }

                        if (!error)
                        {
                            foreach (ReportTable table in report.Tables)
                            {
                                try
                                {
                                    SchemaUtilty.PopulateDataSet(Page.Server, auth, dsReport, table, reportParameter, TextBox1);
                                }
                                catch (Exception ex)
                                {
                                    TextBox1.Text += "\n" + ex.Message;
                                    TextBox1.Text += "\n" + ex.StackTrace;
                                }

                            }

                            ReportDocument ReportDoc = new ReportDocument();
                            ReportDoc.Load(dir + "\\Reports\\" + report.RPTFileName);
                            ReportDoc.SetDataSource(dsReport);

                            MyCrystalReportViewer.ReportSource = ReportDoc;
                            MyCrystalReportViewer.RefreshReport();
                            MyCrystalReportViewer.Visible = true;

                        }


                    }
                    else
                        ShowError("No report with type " + reportType + " is found");



                    //String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);

                    //TextBox1.Text = Environment.CurrentDirectory.ToString();

                    //invoiceId = Request[INVOICE_ID];
                    //ReadXMLConfiguration();


                }
            }
            else
            {
                Session["environment"] = RequestedEnvironment;
                Session[Authenticate.ACCESS_TOKEN] = null;/////Assuring to Redirect Other Environment If Login Already
                Session[Authenticate.CALLER_URL] = HttpContext.Current.Request.Url.AbsoluteUri;
                Response.Redirect("~/Authenticate.aspx");
                //lblInvoice.Text = "no auth";
            }
        }

        private bool ReadXMLConfiguration()
        {
            //String folderPath = Environment.GetFolderPath(Environment.SpecialFolder.System);
            //TextBox1.Text += folderPath;
            bool isReadingTables = false;
            bool isReadingReportParameter = false;
            bool isReadingTableParameter = false;
            bool isReadingTableRelationParameter = false;


            try
            {
                XmlTextReader reader = new XmlTextReader(dir.FullName + "\\reportSetting.xml");
                String value = null;
                TextBox1.Text += dir.FullName + "\\reportSetting.xml";
                Report report = null;
                ReportTable reportTable = null;
                ReportTableParameter tableParam = null;
                ReportTableRelationParameter rtrParameter = null;
                ReportParameter rpParam = null;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            if (reader.Name == "Report")
                            {
                                report = new Report();
                                TextBox1.Text += "\n <Report>";
                            }
                            else if (reader.Name == "ReportParameters")
                            {
                                isReadingReportParameter = true;
                                TextBox1.Text += "\n <ReportParameters>";
                            }
                            else if (reader.Name == "TableParameters")
                            {
                                isReadingTableParameter = true;
                                TextBox1.Text += "\n <TableParameters>";
                            }
                            else if (reader.Name == "TableRelationParameters")
                            {
                                isReadingTableRelationParameter = true;
                                TextBox1.Text += "\n <TableRelationParameters>";
                            }
                            else if (reader.Name == "Table")
                            {
                                //isReadingTableParameter = true;
                                isReadingTables = true;
                                reportTable = new ReportTable();
                                TextBox1.Text += "\n <Table>";
                            }

                            else if (reader.Name == "Parameter")
                            {
                                if (isReadingReportParameter)
                                    rpParam = new ReportParameter();
                                else if (isReadingTableRelationParameter)
                                    rtrParameter = new ReportTableRelationParameter();
                                else
                                    tableParam = new ReportTableParameter();
                                TextBox1.Text += "\n <Parameter>";
                            }

                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            value = reader.Value;
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            if (reader.Name == "ID")
                            {
                                report.ID = value;
                                TextBox1.Text += "\n <ID>" + value + "</ID>";
                            }
                            else if (reader.Name == "Name")
                            {
                                if (isReadingReportParameter)
                                    TextBox1.Text += "\nisReadingReportParameter";
                                else if (isReadingTableParameter)
                                    TextBox1.Text += "\nisReadingTableParameter";
                                else if (isReadingTables)
                                    TextBox1.Text += "\nisReadingTables";


                                if (isReadingReportParameter)
                                    rpParam.ParameterName = value;
                                else if (isReadingTableParameter)
                                    tableParam.ColumnName = value;
                                else if (isReadingTables)
                                    reportTable.Name = value;
                                TextBox1.Text += "\n <Name>" + value + "</Name>";

                            }
                            else if (reader.Name == "TableName")
                            {
                                rtrParameter.TableName = value;
                            }
                            else if (reader.Name == "ParentFieldName")
                            {
                                rtrParameter.ParentColumnName = value;
                            }
                            else if (reader.Name == "ChildFieldName")
                            {
                                rtrParameter.ChildColumnName = value;
                            }
                            else if (reader.Name == "ReportParameterIndex")
                            {
                                int val = 0;
                                int.TryParse(value, out val);
                                tableParam.ReportParameterIndex = val;
                                TextBox1.Text += "\n <ReportParameterIndex>" + value + "</ReportParameterIndex>";
                            }
                            else if (reader.Name == "UseDataSetSchema")
                            {
                                bool res = false;
                                Boolean.TryParse(value, out res);
                                reportTable.UseDataSetSchema = res;
                                TextBox1.Text += "\n <UseDataSetSchema>" + value + "</UseDataSetSchema>";
                            }
                            else if (reader.Name == "RPTFileName")
                            {
                                report.RPTFileName = value;
                                TextBox1.Text += "\n <RPTFileName>" + value + "</RPTFileName>";
                            }

                            else if (reader.Name == "Table")
                            {
                                report.Tables.Add(reportTable);
                                isReadingTables = false;
                                TextBox1.Text += "\n </Table>";
                            }
                            else if (reader.Name == "Parameter")
                            {
                                if (isReadingReportParameter)
                                    report.ReportParamaters.Add(rpParam);
                                else if (isReadingTableRelationParameter)
                                    reportTable.TableRelationParameters.Add(rtrParameter);
                                else
                                    reportTable.TableParameters.Add(tableParam);
                                TextBox1.Text += "\n </Parameter>";
                            }
                            else if (reader.Name == "TableParameters")
                            {
                                isReadingTableParameter = false;
                                TextBox1.Text += "\n </TableParameters>";
                            }
                            else if (reader.Name == "TableRelationParameters")
                            {
                                isReadingTableRelationParameter = false;
                                TextBox1.Text += "\n </TableRelationParameters>";
                            }
                            else if (reader.Name == "ReportParameters")
                            {
                                isReadingReportParameter = false;
                                TextBox1.Text += "\n </ReportParameters>";
                            }
                            else if (reader.Name == "Report")
                            {
                                reportsDict.Add(report.ID, report);
                                TextBox1.Text += "\n </Report>";
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TextBox1.Text += "\n" + ex.Message;
                TextBox1.Text += "\n" + ex.StackTrace;
            }

            return true;
        }

        private void ShowError(String message)
        {
            errorLabel.Visible = true;
            errorLabel.Text += message;
        }
    }
}