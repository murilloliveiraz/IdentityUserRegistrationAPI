using Microsoft.AspNetCore.Identity;

namespace APIThatSendAnEmailToUpdatePassword.Models
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Role {  get; set; }
    }
}
