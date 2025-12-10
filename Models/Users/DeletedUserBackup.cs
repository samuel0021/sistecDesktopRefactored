using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Users
{
    /// <summary>
    /// Represents a backup of a deleted user, containing all relevant information needed for potential restoration.
    /// </summary>
    /// <remarks>This class provides properties to access the details of a deleted user, such as their
    /// original ID, name,  and the reason for deletion. It also includes metadata about the backup status and
    /// restoration details.</remarks>
    public class DeletedUserBackup
    {
        public bool CanRestore => StatusBackup == "ATIVO";

        [JsonProperty("id_backup")]
        public int BackupId { get; set; }

        [JsonProperty("id_usuario_original")]
        public int UsuarioOriginalId { get; set; }

        [JsonProperty("matricula")]
        public string Matricula { get; set; }

        [JsonProperty("nome_usuario")]
        public string Name { get; set; }

        [JsonProperty("setor_usuario")]
        public string Setor { get; set; }

        [JsonProperty("cargo_usuario")]
        public string Cargo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("senha")]
        public string Senha { get; set; }

        [JsonProperty("tel_usuarios")]
        public string Telefone { get; set; }

        [JsonProperty("id_perfil_usuario")]
        public int PerfilId { get; set; }

        [JsonProperty("id_aprovador_usuario")]
        public int? IdAprovador { get; set; }

        [JsonProperty("fk_chamados_id_chamado")]
        public int? ChamadoRelacionadoId { get; set; }

        [JsonProperty("nome_perfil")]
        public string PerfilNome { get; set; }

        [JsonProperty("nivel_acesso")]
        public int NivelAcesso { get; set; }

        [JsonProperty("motivo_delecao")]
        public string MotivoDelecao { get; set; }

        [JsonProperty("usuario_que_deletou")]
        public string UsuarioQueDeletou { get; set; }

        [JsonProperty("data_delecao")]
        public DateTime DataDelecao { get; set; }

        [JsonProperty("status_backup")]
        public string StatusBackup { get; set; }

        [JsonProperty("data_restauracao")]
        public DateTime? DataRestauracao { get; set; }

        [JsonProperty("usuario_que_restaurou")]
        public string UsuarioQueRestaurou { get; set; }
    }
}
