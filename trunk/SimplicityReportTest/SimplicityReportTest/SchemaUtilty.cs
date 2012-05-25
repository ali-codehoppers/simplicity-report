using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Net;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace SimplicityReportTest
{
    public class SchemaUtilty
    {
        public static bool isDebugMode = true;
        public static String GetFullQueryString(DataSet dataSet, string tableName)
        {
            String query = "";
            DataTable dataTable = dataSet.Tables[tableName];

            if (dataTable != null)
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (query.Length == 0)
                        query += "Select ";
                    else
                        query += ", ";
                    query += column.ColumnName;
                }
                query += " from " + tableName;
            }
            return query;
        }


        public static String GetFullQueryString(DataSet dataSet, ReportTable reportTable, List<ReportParameter> reportParameters)
        {
            String query = "";
            DataTable dataTable = dataSet.Tables[reportTable.Name];


            if (reportTable.UseDataSetSchema)
            {
                if (dataTable != null)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (query.Length == 0)
                            query += "Select ";
                        else
                            query += ", ";
                        query += column.ColumnName;
                    }
                    query += " from " + reportTable.Name;
                    String whereQuery = "";
                    if (reportTable.TableParameters != null && reportTable.TableParameters.Count > 0)
                    {
                        foreach (ReportTableParameter param in reportTable.TableParameters)
                        {
                            if (whereQuery.Length == 0)
                                whereQuery += " where ";
                            else
                                whereQuery += " and ";
                            whereQuery+= param.ColumnName + "= '" + reportParameters[param.ReportParameterIndex-1].Value+"'";
                        }
                    }
                    if (reportTable.TableRelationParameters != null && reportTable.TableRelationParameters.Count > 0)
                    {
                        foreach (ReportTableRelationParameter param in reportTable.TableRelationParameters)
                        {
                            DataTable table = dataSet.Tables[param.TableName];
                            DataColumn col = table.Columns[param.ParentColumnName];
                            DataRow row = table.Rows[0];
                            String value = row[col].ToString();

                            if (whereQuery.Length == 0)
                                whereQuery += " where ";
                            else
                                whereQuery += " and ";
                            whereQuery += param.ChildColumnName + "= '" + value + "'";
                        }
                    }
                    query += whereQuery;
                }
            }
            return query;
        }

        public static bool PopulateDataSet(HttpServerUtility Server, AuthenticationObject auth, DataSet dataSet, ReportTable reportTable, List<ReportParameter> reportParameters, TextBox textBox)
        {


            DataSet ds;
            String SQLString = GetFullQueryString(dataSet, reportTable, reportParameters);
            if (isDebugMode)
                textBox.Text += "\n\n" + SQLString;
            string url = auth.instance_url + "/services/data/v22.0/query?q=" + Server.UrlEncode(SQLString);

            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Headers.Add("Authorization", "OAuth " + auth.access_token);
            WebResponse resp = null;
            
            try
            {
                 resp = req.GetResponse();
                 if (resp != null)
                 {
                     ds = new DataSet();
                     System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                     XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sr.ReadToEnd().Trim(), reportTable.Name);
                     StringReader stringReader = new StringReader(doc.OuterXml);
                     if(isDebugMode)
                        textBox.Text += doc.OuterXml + "\n\n\n";
                     ds.ReadXml(stringReader, XmlReadMode.InferSchema);
                     if (ds.Tables.Count > 1)
                     {
                         dataSet.Tables[reportTable.Name].Merge(ds.Tables[1], true, MissingSchemaAction.Ignore);
                     }
                     if (isDebugMode)
                     {
                         if (ds.Tables.Count > 1)
                         {
                             textBox.Text += ds.Tables[1].Rows.Count.ToString() + " Records\n";
                             for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                             {
                                 for (int i = 0; i < ds.Tables[1].Rows[j].ItemArray.Count(); i++)
                                 {
                                     textBox.Text += ds.Tables[1].Columns[i].ColumnName + ":\t\t" + ds.Tables[1].Rows[j].ItemArray[i] + "\n";
                                     //textBox.Text += dataSet.Tables[tableName].Columns[ds.Tables[tableName].Columns[i].ColumnName].ColumnName + "\t\t" + ds.Tables[tableName].Rows[j].ItemArray[i] + "   \n";
                                 }
                                 textBox.Text += "\n\n";
                             }
                         }
                         textBox.Text += "\nActual DataSet\n";
                         for (int j = 0; j < dataSet.Tables[reportTable.Name].Rows.Count; j++)
                         {
                             for (int i = 0; i < dataSet.Tables[reportTable.Name].Rows[j].ItemArray.Count(); i++)
                             {
                                 textBox.Text += "ORG: " + dataSet.Tables[reportTable.Name].Columns[i].ColumnName + ":\t\t" + dataSet.Tables[reportTable.Name].Rows[j].ItemArray[i] + "\n";
                                 //textBox.Text += dataSet.Tables[tableName].Columns[ds.Tables[tableName].Columns[i].ColumnName].ColumnName + "\t\t" + ds.Tables[tableName].Rows[j].ItemArray[i] + "   \n";
                             }
                             textBox.Text += "\n\n";
                         }
                     }
                 }
            }
            catch (WebException ex)
            {
                Logger.LogInfoMessage(ex.Message);
                if (ex.Response != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(ex.Response.GetResponseStream());
                    Logger.LogInfoMessage(sr.ReadToEnd().Trim());
                }
                Logger.LogInfoMessage("There is a WEB EXCEPTION");
                textBox.Text += ex.Message + "\n\n\n";
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogInfoMessage(ex.Message);
                textBox.Text += ex.Message + "\n\n\n";
                return false;
            }

            return true;

        }


        public static bool PopulateDataSet(HttpServerUtility Server, AuthenticationObject auth, DataSet dataSet, string tableName, string condition, TextBox textBox)
        {

            DataSet ds;
            String SQLString = GetFullQueryString(dataSet, tableName);
            SQLString += condition;
            if (isDebugMode)
                textBox.Text += "\n\n" + SQLString + "\n\n";
            //String SQLString = "Select id,Name from simplicity__recJobStatusType__c";
            string url = auth.instance_url + "/services/data/v22.0/query?q=" + Server.UrlEncode(SQLString);
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Headers.Add("Authorization", "OAuth " + auth.access_token);
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
                if (resp != null)
                {
                    ds = new DataSet();
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sr.ReadToEnd().Trim(), tableName);
                    StringReader stringReader = new StringReader(doc.OuterXml);
                    //                    TextBox1.Text += doc.OuterXml + "\n\n\n";
                    ds.ReadXml(stringReader, XmlReadMode.InferSchema);

                    if (isDebugMode)
                    {
                        if (ds.Tables.Count > 1)
                        {
                            textBox.Text += tableName + ":\n\n";
                            foreach (DataColumn column in ds.Tables[1].Columns)
                            {
                                textBox.Text += "\t" + column.ColumnName + "\t\t\t" + column.DataType.FullName + "\n\n";
                            }
                        }
                    }

                    if (ds.Tables.Count > 1)
                    {
                        dataSet.Tables[tableName].Merge(ds.Tables[1], true, MissingSchemaAction.Ignore);
                        //try
                        //{
                        //    dataSet.Tables[tableName].Merge(ds.Tables[1], true, MissingSchemaAction.Ignore);
                        //}
                        //catch (Exception e)
                        //{
                        //    textBox.Text += e.StackTrace;
                        //}
                    }
                    if (isDebugMode)
                    {
                        if (ds.Tables.Count > 1)
                        {
                            textBox.Text += ds.Tables[1].Rows.Count.ToString() + " Records\n";
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                for (int i = 0; i < ds.Tables[1].Rows[j].ItemArray.Count(); i++)
                                {
                                    textBox.Text += ds.Tables[1].Columns[i].ColumnName + ":\t\t" + ds.Tables[1].Rows[j].ItemArray[i] + "\n";
                                    //textBox.Text += dataSet.Tables[tableName].Columns[ds.Tables[tableName].Columns[i].ColumnName].ColumnName + "\t\t" + ds.Tables[tableName].Rows[j].ItemArray[i] + "   \n";
                                }
                                textBox.Text += "\n\n";
                            }
                        }
                        textBox.Text += "\nActual DataSet\n";
                        for (int j = 0; j < dataSet.Tables[tableName].Rows.Count; j++)
                        {
                            for (int i = 0; i < dataSet.Tables[tableName].Rows[j].ItemArray.Count(); i++)
                            {
                                textBox.Text += "ORG: " + dataSet.Tables[tableName].Columns[i].ColumnName + ":\t\t" + dataSet.Tables[tableName].Rows[j].ItemArray[i] + "\n";
                                //textBox.Text += dataSet.Tables[tableName].Columns[ds.Tables[tableName].Columns[i].ColumnName].ColumnName + "\t\t" + ds.Tables[tableName].Rows[j].ItemArray[i] + "   \n";
                            }
                            textBox.Text += "\n\n";
                        }
                    }

                }
            }
            catch (WebException ex)
            {
                Logger.LogInfoMessage(ex.Message);
                if (ex.Response != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(ex.Response.GetResponseStream());
                    Logger.LogInfoMessage(sr.ReadToEnd().Trim());
                }
                Logger.LogInfoMessage("There is a WEB EXCEPTION");
                textBox.Text += ex.Message + "\n\n\n";
                return false;

            }

            return true;
        }


    }
}