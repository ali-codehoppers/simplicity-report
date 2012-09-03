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

        public static bool PopulateDataSet(HttpServerUtility Server, AuthenticationObject auth,String tableRelation,String tableName, DataSet dataSet, String reportTable,List<ReportParameter> reportParameters,  TextBox textBox)
        {

            DataSet ds;
            String SQLString = reportTable;
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
                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sr.ReadToEnd().Trim(), tableName);
                        if (!tableRelation.Trim().Equals("Empty"))
                            doc.SelectNodes(tableRelation);
                        StringReader stringReader = new StringReader(doc.OuterXml);
                        if (isDebugMode)
                            textBox.Text += doc.OuterXml + "\n\n\n";

                        ds.ReadXml(stringReader, XmlReadMode.InferSchema);

                        if (ds.Tables.Count > 1)
                        {
                            if (!tableRelation.Trim().Equals("Empty"))
                            {
                                textBox.Text += "Relation=" + tableRelation + "\n\n\n";
                                if (ds.Tables[tableRelation] != null)
                                {
                                    textBox.Text += "Hello";
                                    textBox.Text += "Rows Count =" + ds.Tables[tableRelation].Rows.Count;
                                    //dataSet.Tables[tableName].Merge(ds.Tables[tableRelation], true, MissingSchemaAction.Ignore);
                                    for (int j = 0; j < ds.Tables[tableRelation].Rows.Count; j++)
                                    {
                                        textBox.Text += "\n with Relation New Row =" + tableName;
                                        DataRow row = dataSet.Tables[tableName].NewRow();
                                        for (int i = 0; i < ds.Tables[tableRelation].Rows[j].ItemArray.Count(); i++)
                                        {
                                            if (dataSet.Tables[tableName].Columns.Contains(ds.Tables[tableRelation].Columns[i].ColumnName))
                                            {
                                                textBox.Text += "\n New Column ,Name=" + ds.Tables[tableRelation].Columns[i].ColumnName;
                                                if (ds.Tables[tableRelation].Rows[j].ItemArray[i].Equals(""))
                                                {
                                                    textBox.Text += "\n NULL" ;
                                                    row[ds.Tables[tableRelation].Columns[i].ColumnName] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    row[ds.Tables[tableRelation].Columns[i].ColumnName] = ds.Tables[tableRelation].Rows[j].ItemArray[i];
                                                }
                                            }
                                            textBox.Text += ds.Tables[tableRelation].Columns[i].ColumnName + ":\t\t" + ds.Tables[tableRelation].Rows[j].ItemArray[i] + "\n";
                                        }
                                        textBox.Text += "Add row 1 from 1";
                                        dataSet.Tables[tableName].Rows.Add(row);
                                        //ds.Tables[tableRelation].Rows.Add(row);
                                    }
                                }
                            }
                            else
                            {
                                if (ds.Tables[1] != null)
                                {
                                    //dataSet.Tables[tableName].Merge(ds.Tables[1], true, MissingSchemaAction.Ignore);
                                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                                    {
                                        textBox.Text += "\n New Row "  ;
                                        DataRow row = dataSet.Tables[tableName].NewRow();
                                        for (int i = 0; i < ds.Tables[1].Rows[j].ItemArray.Count(); i++)
                                        {
                                            if (dataSet.Tables[tableName].Columns.Contains(ds.Tables[1].Columns[i].ColumnName))
                                            {
                                                textBox.Text += "\n New Column ,Name=" + ds.Tables[1].Columns[i].ColumnName;
                                                if (ds.Tables[1].Rows[j].ItemArray[i].Equals(""))
                                                {
                                                    textBox.Text += "\n NULL";
                                                    row[ds.Tables[1].Columns[i].ColumnName] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    row[ds.Tables[1].Columns[i].ColumnName] = ds.Tables[1].Rows[j].ItemArray[i];
                                                }
                                            }
                                            textBox.Text += ds.Tables[1].Columns[i].ColumnName + ":\t\t" + ds.Tables[1].Rows[j].ItemArray[i] + "\n";
                                        }
                                        textBox.Text += "add row 2 from 2";
                                        textBox.Text += "\n add Row \n";
                                        dataSet.Tables[tableName].Rows.Add(row);
                                        //dataSet.Tables[tableName].Merge(ds.Tables[1], true, MissingSchemaAction.Ignore);
                                    }
                                }
                            }
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
        public static void CreateDataSet(HttpServerUtility Server, string path, AuthenticationObject auth, string tableName, DataSet dataSet, TextBox textBox)
        {
            DataTable dt = new DataTable(tableName);
            
            String token = auth.access_token;
            string url = auth.instance_url + "/services/data/v22.0/sobjects/" + tableName + "/describe";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            req.Headers.Add("Authorization", "OAuth " + auth.access_token);
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
                if (resp != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sr.ReadToEnd().Trim(), tableName);
                    XmlElement rootElement = doc.DocumentElement;

                    XmlNodeList elementListfields = rootElement.GetElementsByTagName("fields");
                    XmlNodeList elementListname = rootElement.GetElementsByTagName("name");
                    XmlNodeList elementListtype = rootElement.GetElementsByTagName("type");

                    for (int i = 1; i < elementListfields.Count; i++)
                    {
                        var x = i - 1;
                        if (x < elementListtype.Count)
                        {
                            textBox.Text += elementListname[x].InnerXml.ToString() + "\n" + elementListtype[x].InnerXml.ToString();
                            if (elementListtype[x].InnerXml.ToString().Equals("datetime"))
                            {
                                dt.Columns.Add((elementListname[i].InnerXml).ToString(), typeof(DateTime));
                            }
                            else if (elementListtype[x].InnerXml.ToString().Equals("date"))
                            {
                                dt.Columns.Add((elementListname[i].InnerXml).ToString(), typeof(DateTime));
                            }
                            else if (elementListtype[x].InnerXml.ToString().Equals("boolean"))
                            {
                                dt.Columns.Add((elementListname[i].InnerXml).ToString(), typeof(Boolean));
                            }
                            else if (elementListtype[x].InnerXml.ToString().Equals("double") || elementListtype[x].InnerXml.ToString().Equals("currency"))
                            {
                                dt.Columns.Add((elementListname[i].InnerXml).ToString(), typeof(double));
                            }
                            else
                            {
                                dt.Columns.Add((elementListname[i].InnerXml).ToString(), typeof(string));
                            }
                        }
                    }
                    dataSet.Tables.Add(dt);
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
            }
                    
        }

    }
}