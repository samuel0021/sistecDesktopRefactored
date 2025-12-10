using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.IA
{
    public class IaResultsResponse : ApiResponse
    {
        [JsonProperty("data")]
        public IaResults Data { get; set; }
    }
}
