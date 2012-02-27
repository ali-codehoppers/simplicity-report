using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public class ReportTable
    {
        public String Name
        {
            get;
            set;
        }

        public Boolean UseDataSetSchema
        {
            get;
            set;
        }

        public List<String> Columns
        {
            get;
            set;
        }

        public ReportTable()
        {
            UseDataSetSchema = true;
            Columns = new List<string>();
            TableParameters = new List<ReportTableParameter>();
            TableRelationParameters = new List<ReportTableRelationParameter>();
        }

        public List<ReportTableParameter> TableParameters
        {
            get;
            set;
        }

        public List<ReportTableRelationParameter> TableRelationParameters
        {
            get;
            set;
        }

        
        public void AddParameter(String columnName, int reportParameterIndex)
        {
            TableParameters.Add(new ReportTableParameter(columnName, reportParameterIndex));
        }
    }
}