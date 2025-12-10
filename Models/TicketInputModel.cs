using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; ////

namespace sistecDesktopRefactored.Models
{
    public class TicketInputModel
    {
        [Required(ErrorMessage = "*Informe o título")]
        public string TitleInput { get; set; }

        [Range(1, 4, ErrorMessage = "*Selecione a prioridade")]
        public int PriorityInput { get; set; }

        [Required(ErrorMessage = "*Selecione a categoria")]
        public string CategoryInput { get; set; }

        [Required(ErrorMessage = "*Selecione o problema")]
        public string ProblemInput { get; set; }

        [Required(ErrorMessage = "*Informe a descrição")]
        [MinLength(10, ErrorMessage = "*Mínimo de 10 caracteres")]
        public string DescriptionInput { get; set; }
    }
}
