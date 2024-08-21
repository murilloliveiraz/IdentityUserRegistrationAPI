using Microsoft.AspNetCore.Identity;

namespace APIThatSendAnEmailToUpdatePassword.Models
{
    public class Medico
    {
        public int Id { get; set; }
        public string Especialidade { get; set; }
        public string CRM { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataInativacao { get; set; }
        public string UserId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
