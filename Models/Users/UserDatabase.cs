using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    public class UserDatabase
    {
        [JsonProperty("id_usuario")]
        public int Id { get; set; }

        [JsonProperty("matricula_aprovador")]
        public int Matricula { get; set; }

        [JsonProperty("nome_usuario")]
        public string Name { get; set; }

        [JsonProperty("setor_usuario")]
        public string Setor { get; set; }

        [JsonProperty("cargo_usuario")]
        public string Cargo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("tel_usuarios")]
        public string Telefone { get; set; }

        [JsonProperty("id_perfil_usuario")]
        public int PerfilId { get; set; }

        [JsonProperty("nome_perfil")]
        public string PerfilNome { get; set; }

        [JsonProperty("nivel_acesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("descricao_perfil_usuario")]
        public string PerfilDescricao { get; set; }

        [JsonProperty("password")]
        public string Senha { get; set; }

        public Profile Perfil => new Profile
        {
            Id = PerfilId,
            Nome = PerfilNome,
            NivelAcesso = Matricula,
            Descricao = PerfilDescricao
        };
    }
}
