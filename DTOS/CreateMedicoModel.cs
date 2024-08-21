namespace APIThatSendAnEmailToUpdatePassword.DTOS
{
    public class CreateMedicoModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Especialidade { get; set; }
        public string FrontendUrl { get; set; }
        public string CRM { get; set; }
        public string CPF { get; set; }
    }
}
