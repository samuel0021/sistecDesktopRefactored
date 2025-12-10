using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Auth
{
    public class LoginResponse : ApiResponse
    {
        [JsonProperty("data")]
        public LoginData Data { get; set; }
    }
}
