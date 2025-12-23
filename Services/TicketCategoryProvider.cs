using sistecDesktopRefactored.Models.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models
{
    public static class TicketCategoryProvider
    {
        public static IReadOnlyList<CategoryProblem> GetAll() => new List<CategoryProblem>
        {
            new CategoryProblem
            {
                Categoria = "hardware",
                Label = "Hardware",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "computador-nao-liga", Label = "Computador não liga" },
                    new ProblemItem { Value = "tela-preta", Label = "Tela preta" },
                    new ProblemItem { Value = "travamento-frequente", Label = "Travamento frequente" },
                    new ProblemItem { Value = "lentidao-equipamento", Label = "Lentidão no equipamento" },
                    new ProblemItem { Value = "problema-teclado-mouse", Label = "Problema com teclado/mouse" },
                    new ProblemItem { Value = "outros-hardware", Label = "Outros problemas de hardware" }
                }
            },
            new CategoryProblem
            {
                Categoria = "software",
                Label = "Software",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "erro-sistema", Label = "Erro no sistema" },
                    new ProblemItem { Value = "aplicativo-nao-abre", Label = "Aplicativo não abre" },
                    new ProblemItem { Value = "lentidao-sistema", Label = "Lentidão no sistema" },
                    new ProblemItem { Value = "perda-dados", Label = "Perda de dados" },
                    new ProblemItem { Value = "atualizacao-software", Label = "Problema com atualização" },
                    new ProblemItem { Value = "outros-software", Label = "Outros problemas de software" }
                }
            },
            new CategoryProblem
            {
                Categoria = "rede",
                Label = "Rede e Conectividade",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "sem-internet", Label = "Sem acesso à internet" },
                    new ProblemItem { Value = "wifi-nao-conecta", Label = "Wi-Fi não conecta" },
                    new ProblemItem { Value = "lentidao-rede", Label = "Lentidão na rede" },
                    new ProblemItem { Value = "acesso-compartilhado", Label = "Problema com acesso compartilhado" },
                    new ProblemItem { Value = "outros-rede", Label = "Outros problemas de rede" }
                }
            },
            new CategoryProblem
            {
                Categoria = "acesso",
                Label = "Acesso e Permissões",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "esqueci-senha", Label = "Esqueci minha senha" },
                    new ProblemItem { Value = "acesso-negado", Label = "Acesso negado ao sistema" },
                    new ProblemItem { Value = "criar-usuario", Label = "Criar novo usuário" },
                    new ProblemItem { Value = "alterar-permissoes", Label = "Alterar permissões" },
                    new ProblemItem { Value = "outros-acesso", Label = "Outros problemas de acesso" }
                }
            },
            new CategoryProblem
            {
                Categoria = "email",
                Label = "Email",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "nao-recebe-email", Label = "Não está recebendo emails" },
                    new ProblemItem { Value = "nao-envia-email", Label = "Não consegue enviar emails" },
                    new ProblemItem { Value = "configurar-email", Label = "Configurar cliente de email" },
                    new ProblemItem { Value = "problema-anexo", Label = "Problema com anexos" },
                    new ProblemItem { Value = "outros-email", Label = "Outros problemas de email" }
                }
            },
            new CategoryProblem
            {
                Categoria = "outros",
                Label = "Outros",
                Problemas = new List<ProblemItem>
                {
                    new ProblemItem { Value = "solicitacao-equipamento", Label = "Solicitação de equipamento" },
                    new ProblemItem { Value = "treinamento", Label = "Solicitação de treinamento" },
                    new ProblemItem { Value = "sugestao-melhoria", Label = "Sugestão de melhoria" },
                    new ProblemItem { Value = "outros-geral", Label = "Outros" }
                }
            }
        };
    }
}
