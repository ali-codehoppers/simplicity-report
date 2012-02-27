using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimplicityReportTest
{
    public partial class Authenticate : System.Web.UI.Page
    {

        /// <summary>
        /// For Test Account
        /// </summary>
        //        public const string CONSUMER_KEY = "3MVG9MHOv_bskkhQoIrnfjV3ecs4jJgoexrq4bzjcbA.7k_tTWbyQDJs9fJQdvyLjTX_euirVMP_UJ.BbGiPJ";
        //        public const string CONSUMER_SECRET = "425811161419072524";
        //        public const string ENVIRONMENT = "https://test.salesforce.com";

        /// <summary>
        /// For Live Account
        /// </summary>
        /// 

        public const string SANDBOX = "sandbox";
        public const string PRODUCTION = "production";
        public const string AUTHENTICATED_ENVIRONMENT = "AUTHENTICATED_ENVIRONMENT";

        public const string CONSUMER_KEY_PROD = "3MVG9PhR6g6B7ps7GQNDZr4rmxaW4mdiAu3gw6_Gx7tuHoxtVogYwBu65ke_6dKHkDf1dGq0ObPcyamtZG1qh";
        public const string CONSUMER_SECRET_PROD = "2756509967172651245";
        public const string ENVIRONMENT_PROD = "https://login.salesforce.com";

        public const string REDIRECT_URL_PROD = "https://ec2-23-20-7-131.compute-1.amazonaws.com/SCReports/authenticate2.aspx";


        public const string CONSUMER_KEY_TEST = "3MVG9MHOv_bskkhQoIrnfjV3ecs4jJgoexrq4bzjcbA.7k_tTWbyQDJs9fJQdvyLjTX_euirVMP_UJ.BbGiPJ";
        public const string CONSUMER_SECRET_TEST = "425811161419072524";
        public const string ENVIRONMENT_TEST = "https://test.salesforce.com";

        public const string REDIRECT_URL_TEST = "https://ec2-23-20-7-131.compute-1.amazonaws.com/SCReports/authenticate2.aspx";



        public const string ACCESS_TOKEN = "ACCESS_TOKEN";
        // public  const string INSTANCE_URL = "INSTANCE_URL";
        public const string INVOICE_ID = "invoiceId";
        public const string CALLER_URL = "caller_url";

        private string authenticateUrl = null;
        private string CurrentEnvironment = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            Logger.LogInfoMessage("I am in Authenticate");
            AuthenticationObject accessToken = (AuthenticationObject)Session[ACCESS_TOKEN];
            Logger.LogInfoMessage("Access Token is:" + accessToken);
            if (accessToken == null)
            {
                Logger.LogInfoMessage("Redirecting to:" + authenticateUrl);
                Response.Redirect(authenticateUrl);
                return;
            }
            else
            {
                if (CurrentEnvironment.CompareTo(SANDBOX) == 0)
                {
                    Response.Redirect(REDIRECT_URL_TEST);
                }
                else
                {
                    Response.Redirect(REDIRECT_URL_PROD);
                }
                return;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CurrentEnvironment = Session["environment"].ToString().ToLower();
            if (CurrentEnvironment.CompareTo(SANDBOX) == 0)
            {
                authenticateUrl = ENVIRONMENT_TEST
                        + "/services/oauth2/authorize?response_type=code&client_id="
                        + CONSUMER_KEY_TEST + "&redirect_uri="
                        + HttpUtility.UrlEncode(REDIRECT_URL_TEST, System.Text.Encoding.UTF8);
            }
            else
            {
                authenticateUrl = ENVIRONMENT_PROD
                        + "/services/oauth2/authorize?response_type=code&client_id="
                        + CONSUMER_KEY_PROD + "&redirect_uri="
                        + HttpUtility.UrlEncode(REDIRECT_URL_PROD, System.Text.Encoding.UTF8);
            }
        }

    }
}