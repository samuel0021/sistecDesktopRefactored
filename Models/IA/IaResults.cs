using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.IA
{
    public class IaResults
    {
        [JsonProperty("id_resposta_ia")]
        public int IdRespostaIa { get; set; }

        [JsonProperty("fk_chamados_id_chamado")]
        public int ChamadoId { get; set; }

        [JsonProperty("tipo_resposta")]
        public string TipoResposta { get; set; }

        [JsonProperty("analise_triagem")]
        public object AnaliseTriagem { get; set; } // pode criar modelo forte depois

        [JsonProperty("solucao_ia")]
        public string SolucaoIa { get; set; }

        [JsonProperty("feedback_usuario")]
        public string FeedbackUsuario { get; set; }

        [JsonProperty("data_resposta")]
        public DateTime DataResposta { get; set; }

        [JsonProperty("data_feedback")]
        public DateTime? DataFeedback { get; set; }
    }
}
