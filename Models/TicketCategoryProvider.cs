using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models
{
    public static class TicketCategoryProvider
    {
        public static IReadOnlyList<CategoriaProblema> GetAll() => new List<CategoriaProblema>
        {
            new CategoriaProblema
                {
                    Categoria = "hardware",
                    Label = "Hardware",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "computador-nao-liga", Label = "Computador não liga" },
                        new ProblemaItem { Value = "tela-preta", Label = "Tela preta" },
                        new ProblemaItem { Value = "travamento-frequente", Label = "Travamento frequente" },
                        new ProblemaItem { Value = "lentidao-equipamento", Label = "Lentidão no equipamento" },
                        new ProblemaItem { Value = "problema-teclado-mouse", Label = "Problema com teclado/mouse" },
                        new ProblemaItem { Value = "outros-hardware", Label = "Outros problemas de hardware" }
                    }
                },
                new CategoriaProblema
                {
                    Categoria = "software",
                    Label = "Software",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "erro-sistema", Label = "Erro no sistema" },
                        new ProblemaItem { Value = "aplicativo-nao-abre", Label = "Aplicativo não abre" },
                        new ProblemaItem { Value = "lentidao-sistema", Label = "Lentidão no sistema" },
                        new ProblemaItem { Value = "perda-dados", Label = "Perda de dados" },
                        new ProblemaItem { Value = "atualizacao-software", Label = "Problema com atualização" },
                        new ProblemaItem { Value = "outros-software", Label = "Outros problemas de software" }
                    }
                },
                new CategoriaProblema
                {
                    Categoria = "rede",
                    Label = "Rede e Conectividade",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "sem-internet", Label = "Sem acesso à internet" },
                        new ProblemaItem { Value = "wifi-nao-conecta", Label = "Wi-Fi não conecta" },
                        new ProblemaItem { Value = "lentidao-rede", Label = "Lentidão na rede" },
                        new ProblemaItem { Value = "acesso-compartilhado", Label = "Problema com acesso compartilhado" },
                        new ProblemaItem { Value = "outros-rede", Label = "Outros problemas de rede" }
                    }
                },
                new CategoriaProblema
                {
                    Categoria = "acesso",
                    Label = "Acesso e Permissões",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "esqueci-senha", Label = "Esqueci minha senha" },
                        new ProblemaItem { Value = "acesso-negado", Label = "Acesso negado ao sistema" },
                        new ProblemaItem { Value = "criar-usuario", Label = "Criar novo usuário" },
                        new ProblemaItem { Value = "alterar-permissoes", Label = "Alterar permissões" },
                        new ProblemaItem { Value = "outros-acesso", Label = "Outros problemas de acesso" }
                    }
                },
                new CategoriaProblema
                {
                    Categoria = "email",
                    Label = "Email",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "nao-recebe-email", Label = "Não está recebendo emails" },
                        new ProblemaItem { Value = "nao-envia-email", Label = "Não consegue enviar emails" },
                        new ProblemaItem { Value = "configurar-email", Label = "Configurar cliente de email" },
                        new ProblemaItem { Value = "problema-anexo", Label = "Problema com anexos" },
                        new ProblemaItem { Value = "outros-email", Label = "Outros problemas de email" }
                    }
                },
                new CategoriaProblema
                {
                    Categoria = "outros",
                    Label = "Outros",
                    Problemas = new List<ProblemaItem>
                    {
                        new ProblemaItem { Value = "solicitacao-equipamento", Label = "Solicitação de equipamento" },
                        new ProblemaItem { Value = "treinamento", Label = "Solicitação de treinamento" },
                        new ProblemaItem { Value = "sugestao-melhoria", Label = "Sugestão de melhoria" },
                        new ProblemaItem { Value = "outros-geral", Label = "Outros" }
                    }
                }
        };
    }
}
