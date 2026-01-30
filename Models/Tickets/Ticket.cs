using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sistecDesktopRefactored.Models.Tickets
{
    public class Ticket
    {
        /*public bool CanScale => Status == "Com Analista";

        public bool TicketScaled => Status == "Escalado";*/

        public bool CanResolve => (Status == "Com Analista" && App.LoggedUser.IdPerfilUsuario.Id >= 2)
                                  || (Status == "Escalado" && App.LoggedUser.IdPerfilUsuario.Id >= 4);

        public bool CanView => (App.LoggedUser.IdPerfilUsuario.Id >= 2) || (App.LoggedUser.Id == UserId);

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public string Problem { get; set; }
        public string UserOpener { get; set; }
        public string EmailUser { get; set; }
        public string ResolutionUser { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolutionDate { get; set; }

        public Ticket() { }

        public Ticket(TicketDatabase db)
        {
            Id = db.Id;
            Title = db.Title ?? "Sem título";
            Description = db.Description ?? "";
            Status = db.Status ?? "";
            Priority = db.Priority;
            Category = db.Category ?? "";
            Problem = db.Problem ?? "";
            UserOpener = db.UserOpener ?? "";
            EmailUser = db.EmailUser ?? "";
            ResolutionUser = db.ResolutionUser ?? "";
            UserId = db.UserId;
            CreatedAt = db.CreatedAt;
            ResolutionDate = db.ResulotionDate;
        }
    }
}
