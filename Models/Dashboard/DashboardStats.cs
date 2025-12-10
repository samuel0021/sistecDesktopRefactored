using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Dashboard
{
    public class DashboardStats
    {
        public int abertos { get; set; }
        public int aprovados { get; set; }
        public int rejeitados { get; set; }
        public int triagem_ia { get; set; }
        public int aguardando_resposta { get; set; }
        public int com_analista { get; set; }
        public int resolvidos { get; set; }
        public int fechados { get; set; }
        public int escalados { get; set; }
        public int total { get; set; }
    }
}
