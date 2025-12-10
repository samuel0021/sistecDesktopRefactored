using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    public class Profile
    {
        [JsonProperty("id_perfil_usuario")]
        public int Id { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("nivel_acesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("descricao_perfil_usuario")]
        public string Descricao { get; set; }
    }
}
