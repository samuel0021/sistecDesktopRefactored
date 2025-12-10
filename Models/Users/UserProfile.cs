using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    public class UserProfile
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("nivel_acesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }
    }
}
