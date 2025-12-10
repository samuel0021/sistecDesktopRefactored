using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("matricula")]
        public int Matricula { get; set; }

        [JsonProperty("name")]
        public string NomeUsuario { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        [JsonProperty("setor")]
        public string Setor { get; set; }

        [JsonProperty("cargo")]
        public string Cargo { get; set; }

        [JsonProperty("id_aprovador")]
        public int? IdAprovador { get; set; }

        [JsonProperty("perfil")]
        public UserProfile IdPerfilUsuario { get; set; }

        [JsonProperty("nome")]
        public string PerfilNome { get; set; }

        [JsonProperty("nivel_acesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("descricao")]
        public string PerfilDescricao { get; set; }

        [JsonProperty("ramal")]
        public string Ramal { get; set; }

        // Propriedades extras (não vêm do JSON direto, se quiser computed)
        [JsonIgnore]
        public string Nome
        {
            get => NomeUsuario?.Split(' ').FirstOrDefault() ?? "";
            set
            {
                var sobrenomeAtual = Sobrenome ?? "";
                NomeUsuario = $"{value} {sobrenomeAtual}".Trim();
            }
        }

        [JsonIgnore]
        public string Sobrenome
        {
            get => string.Join(" ", (NomeUsuario?.Split(' ').Skip(1) ?? Array.Empty<string>()));
            set
            {
                var nomeAtual = Nome ?? "";
                NomeUsuario = $"{nomeAtual} {value}".Trim();
            }
        }
    }
}
