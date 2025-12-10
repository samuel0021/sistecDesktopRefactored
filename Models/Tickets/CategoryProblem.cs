using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class CategoryProblem
    {
        public string Categoria { get; set; }
        public string Label { get; set; }
        public List<ProblemItem> Problemas { get; set; }
    }

    public class ProblemItem
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
