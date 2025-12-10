using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class TicketResponse : ApiResponse
    {
        public TicketDatabase Data { get; set; }
    }

    public class TicketsResponse : ApiResponse
    {
        public List<TicketDatabase> Data { get; set; }
    }

    public class ScaledTicketsResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<ScaledTicket> Data { get; set; }
    }
}
