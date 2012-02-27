using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public class ReportParameter
    {
        public String ParameterName;
        public String Value;

        public ReportParameter(String parameterName, String value)
        {
            ParameterName = parameterName;
            Value = value;
        }

        public ReportParameter()
        {

        }
    }
}