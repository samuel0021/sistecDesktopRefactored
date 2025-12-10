using Newtonsoft.Json;
using sistecDesktopRefactored.Models.Auth;
using sistecDesktopRefactored.Models.Dashboard;
using sistecDesktopRefactored.Models.IA;
using sistecDesktopRefactored.Models.Tickets;
using sistecDesktopRefactored.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models
{
    /// <summary>
    /// Represents the response from an API call, including the success status, HTTP status code, and a message.
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}
