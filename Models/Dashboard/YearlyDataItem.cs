using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Dashboard
{
    public class YearlyDataItem
    {
        public string month { get; set; }
        public int abertos { get; set; }
        public int resolvidos { get; set; }
    }
}
