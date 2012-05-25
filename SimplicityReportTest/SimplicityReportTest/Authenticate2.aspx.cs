using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Configuration;

namespace SimplicityReportTest
{
    public partial class Authenticate2 : System.Web.UI.Page
    {
        private string tokenUrl;
        private string CurrentEnvironment = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.LogInfoMessage("I am Authenticate 2");
            string code = Request["code"];
            Logger.LogInfoMessage("Code is: " + code);
            
            string parameters = "";
            if (CurrentEnvironment.Equals(Authenticate.SANDBOX))
            {

                parameters = "code=" + HttpUtility.UrlEncode(code)
                    + "&grant_type=authorization_code"
                    + "&client_id=" + HttpUtility.UrlEncode(Authenticate.CONSUMER_KEY_TEST)
                    + "&client_secret=" + HttpUtility.UrlEncode(Authenticate.CONSUMER_SECRET_TEST)
                    + "&redirect_uri=" + HttpUtility.UrlEncode(ConfigurationSettings.AppSettings["REDIRECT_URL"]);
            }
            else
            {
                parameters = "code=" + HttpUtility.UrlEncode(code)
                 + "&grant_type=authorization_code"
                 + "&client_id=" + HttpUtility.UrlEncode(Authenticate.CONSUMER_KEY_PROD)
                 + "&client_secret=" + HttpUtility.UrlEncode(Authenticate.CONSUMER_SECRET_PROD)
                 + "&redirect_uri=" + HttpUtility.UrlEncode(ConfigurationSettings.AppSettings["REDIRECT_URL"]);
            }
            Logger.LogInfoMessage("Paramters:" + parameters);
            string result = HttpPost(tokenUrl, parameters);
            if (result != null)
            {
                Logger.LogInfoMessage(result);
                MemoryStream mStream = new MemoryStream(UTF8Encoding.Default.GetBytes(result));

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(AuthenticationObject));
                AuthenticationObject auth = (AuthenticationObject)ser.ReadObject(mStream);
                Session.Add(Authenticate.ACCESS_TOKEN, auth);
                //if (Session[ShowInvoice.INVOICE_ID] != null)
                //    Response.Redirect("~/ShowInvoice.aspx?" + ShowInvoice.INVOICE_ID + "=" + Session[ShowInvoice.INVOICE_ID].ToString());
                //else
                //    Response.Redirect("~/ShowInvoice.aspx?" + ShowInvoice.INVOICE_ID + "=a0kL00000008P7W");
                Session[Authenticate.AUTHENTICATED_ENVIRONMENT] = CurrentEnvironment;
                if (Session[Authenticate.CALLER_URL] != null && Session[Authenticate.CALLER_URL].ToString().Length > 0)
                    Response.Redirect(Session[Authenticate.CALLER_URL].ToString());
                else
                    Response.Redirect("~/ShowReport.aspx");

                //               if (Session[ShowInvoice.INVOICE_ID] != null)
                //Response.Redirect("~/ShowJobTicket.aspx?" + ShowJobTicket.JOB_ID + "=" + Session[ShowInvoice.INVOICE_ID].ToString());
                //                    Response.Redirect("~/ShowProForma.aspx?" + ShowProForma.INVOICE_ID + "=" + Session[ShowProForma.INVOICE_ID].ToString());
                //                else
                //Response.Redirect("~/ShowJobTicket.aspx?" + ShowJobTicket.JOB_ID + "=a0OL000000003fXMAQ");
                //Response.Redirect("~/ShowJobTicket.aspx?" + ShowJobTicket.JOB_ID + "=a0OL000000006DOMAY");
                //                    Response.Redirect("~/ShowProForma.aspx?" + ShowProForma.INVOICE_ID + "=a0kL00000008P7W");


            }
            else if (Session[Authenticate.ACCESS_TOKEN] != null)
            {
                if (Session[Authenticate.CALLER_URL] != null && Session[Authenticate.CALLER_URL].ToString().Length > 0)
                    Response.Redirect(Session[Authenticate.CALLER_URL].ToString());
                else
                    Response.Redirect("~/ShowReport.aspx");
            }



        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e); 
            CurrentEnvironment = Session["environment"].ToString().ToLower();
            Logger.LogInfoMessage(CurrentEnvironment);
            if (CurrentEnvironment.Equals(Authenticate.SANDBOX)) // ager sand box hai to test environment pay lay jaye
            {
                tokenUrl = Authenticate.ENVIRONMENT_TEST + "/services/oauth2/token";
                Logger.LogInfoMessage(tokenUrl);
            }
            else              // ager environment sand box nahi hai to production environment pay lay jaye
            {
                tokenUrl = Authenticate.ENVIRONMENT_PROD + "/services/oauth2/token";
                Logger.LogInfoMessage(tokenUrl);
            }
        }
        public string HttpPost(string URI, string Parameters)
        {
            try
            {
                Logger.LogInfoMessage("I am in Http Post");
                System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                //                req.

                // Add parameters to post
                byte[] data = System.Text.Encoding.ASCII.GetBytes(Parameters);
                req.ContentLength = data.Length;
                System.IO.Stream os = req.GetRequestStream();
                os.Write(data, 0, data.Length);
                os.Close();
                Logger.LogInfoMessage("Parameters Added");
                // Do the post and get the response.
                System.Net.WebResponse resp = req.GetResponse();
                if (resp == null)
                {
                    Logger.LogInfoMessage("Got NULL Repsonse");
                    return null;
                }
                Logger.LogInfoMessage("Got Repsonse");
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
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
            }
            catch (Exception ex)
            {
                Logger.LogInfoMessage("There is an EXCEPTION");

            }
            return null;
        }
    }
}