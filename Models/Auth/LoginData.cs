using Newtonsoft.Json;
using sistecDesktopRefactored.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Auth
{
    /// <summary>
    /// Represents the login data containing user information.
    /// </summary>
    /// <remarks>This class is used to deserialize JSON data related to user login.</remarks>
    public class LoginData
    {
        [JsonProperty("user")]
        public User User { get; set; }
    }
}