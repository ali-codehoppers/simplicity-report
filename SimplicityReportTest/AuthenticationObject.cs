using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public class AuthenticationObject
    {
        private string _id;
        public string id { get { return _id; } set { _id = value; } }

        private string _issued_at;
        public string issued_at { get { return _issued_at; } set { _issued_at = value; } }

        private string _refresh_token;
        public string refresh_token { get { return _refresh_token; } set { _refresh_token = value; } }

        private string _instance_url;
        public string instance_url { get { return _instance_url; } set { _instance_url = value; } }

        private string _signature;
        public string signature { get { return _signature; } set { _signature = value; } }

        private string _access_token;
        public string access_token { get { return _access_token; } set { _access_token = value; } }

        

    }
}