using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class ScaledTicket
    {
        [JsonProperty("id_chamado")]
        public int IdChamado { get; set; }

        [JsonProperty("descricao_categoria_chamado")]
        public string DescricaoCategoriaChamado { get; set; }

        [JsonProperty("descricao_problema_chamado")]
        public string DescricaoProblemaChamado { get; set; }

        [JsonProperty("descricao_status_chamado")]
        public string DescricaoStatusChamado { get; set; }

        [JsonProperty("prioridade_chamado")]
        public int PrioridadeChamado { get; set; }

        [JsonProperty("titulo_chamado")]
        public string TituloChamado { get; set; }

        [JsonProperty("data_abertura")]
        public DateTime DataAbertura { get; set; }

        [JsonProperty("usuario_abertura")]
        public string UsuarioAbertura { get; set; }
    }
}
