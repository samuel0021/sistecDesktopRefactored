using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Dashboard
{
    public class DashboardStatsResponse
    {
        public int status { get; set; }
        public string message { get; set; }
        public DashboardStats data { get; set; }
    }
    public class AnalystDataResponse
    {
        public List<AnalystDataItem> data { get; set; }
    }

    public class MonthlyDataResponse
    {
        public List<MonthlyDataItem> data { get; set; }
    }

    public class CategoryDataResponse
    {
        public List<CategoryDataItem> data { get; set; }
    }

    public class YearlyDataResponse
    {
        public List<YearlyDataItem> data { get; set; }
    }
}
