using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    /// <summary>
    /// Represents the response containing a list of user data from an API call.
    /// </summary>
    /// <remarks>This class extends <see cref="ApiResponse"/> to include a collection of user
    /// information.</remarks>
    public class UsersResponse : ApiResponse
    {
        public List<UserDatabase> Data { get; set; }
    }

    public class UserResponse : ApiResponse
    {
        public UserDatabase Data { get; set; }
    }
    public class ProfilesResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public List<UserProfile> Data { get; set; }
    }
}
