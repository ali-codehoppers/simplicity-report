using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public class ReportTableParameter
    {
        public String ColumnName;
        public int ReportParameterIndex;

        public ReportTableParameter(String columnName, int value)
        {
            ColumnName = columnName;
            ReportParameterIndex = value;
        }

        public ReportTableParameter()
        {

        }
    }
}