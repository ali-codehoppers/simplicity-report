using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public class Report
    {
        public String ID { get; set; }

        public String RPTFileName { get; set; }

        public List<ReportTable> Tables { get; set; }

        public List<ReportParameter> ReportParamaters
        {
            get;
            set;
        }

        public Report()
        {
            Tables = new List<ReportTable>();
            ReportParamaters = new List<ReportParameter>();
        }

        public void AddReprotParameter(String parameterName, String value)
        {
            ReportParamaters.Add(new ReportParameter(parameterName, value));
        }
    }
}