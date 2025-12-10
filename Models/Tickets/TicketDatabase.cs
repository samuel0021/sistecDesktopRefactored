using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class TicketDatabase
    {
        [JsonProperty("id_chamado")]
        public int Id { get; set; }

        [JsonProperty("titulo_chamado")]
        public string Title { get; set; }

        [JsonProperty("descricao_detalhada")]
        public string Description { get; set; }

        [JsonProperty("descricao_status_chamado")]
        public string Status { get; set; }

        [JsonProperty("prioridade_chamado")]
        public int Priority { get; set; }

        [JsonProperty("descricao_categoria_chamado")]
        public string Category { get; set; }

        [JsonProperty("descricao_problema_chamado")]
        public string Problem { get; set; }

        [JsonProperty("usuario_abertura")]
        public string UserOpener { get; set; }

        [JsonProperty("email_usuario")]
        public string EmailUser { get; set; }

        [JsonProperty("usuario_resolucao")]
        public string ResolutionUser { get; set; }

        [JsonProperty("id_usuario_abertura")]
        public int UserId { get; set; }

        [JsonProperty("data_abertura")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("data_resolucao")]
        public DateTime? ResulotionDate { get; set; }
    }
}
