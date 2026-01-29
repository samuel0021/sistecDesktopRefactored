using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class CreateTicketRequest
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("prioridade_chamado")]
        public string Priority { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("descricao_categoria")]
        public string DescricaoCategoria { get; set; }

        [JsonProperty("descricao_categoria_chamado")]
        public string DescricaoCategoriaChamado { get; set; }

        [JsonProperty("descricao_detalhada")]
        public string DescricaoDetalhada { get; set; }

        [JsonProperty("descricao_problema")]
        public string Problem { get; set; }
    }
}
