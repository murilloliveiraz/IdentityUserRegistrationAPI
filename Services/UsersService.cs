using APIThatSendAnEmailToUpdatePassword.Context;
using APIThatSendAnEmailToUpdatePassword.DTOS;
using APIThatSendAnEmailToUpdatePassword.Helpers;
using APIThatSendAnEmailToUpdatePassword.Models;
using APIThatSendAnEmailToUpdatePassword.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace APIThatSendAnEmailToUpdatePassword.Services
{
    public class UsersService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailService _emailSender;
        private readonly UsersContext _context;

        public UsersService(UserManager<Usuario> userManager, IEmailService emailSender, UsersContext context)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _context = context;
        }

        public async Task<Usuario> RegisterMedico(CreateMedicoModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        // O usuário já existe, você pode retorná-lo diretamente
                        Console.WriteLine("Usuário já existe: " + existingUser.Id);
                        await CreateMedicoForUser(existingUser, model);
                        return existingUser;
                    }

                    var user = new Usuario
                    {
                        Nome = model.Nome,
                        UserName = model.Email,
                        Email = model.Email,
                        CPF = model.CPF,
                        Role = "Médico"
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        Console.WriteLine("Usuário criado com sucesso: " + user.Id);
                        await CreateMedicoForUser(user, model);
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        var callbackUrl = $"{model.FrontendUrl}/reset-password?userId={user.Id}&token={token}";
                        MailRequest mailRequest = new MailRequest
                        {
                            ToEmail = user.Email,
                            Subject = "Defina sua senha",
                            Body = ChangePasswordEmail(callbackUrl)
                        };
                        await _emailSender.SendEmailAsync(mailRequest);
                        await transaction.CommitAsync();
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("Falha ao criar usuário: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro na transação: " + ex.Message);
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<Medico> CreateMedicoForUser(Usuario user, CreateMedicoModel model)
        {
            
            var medico = new Medico
            {
                Especialidade = model.Especialidade,
                CRM = model.CRM,
                DataCriacao = DateTime.UtcNow,
                UserId = user.Id,
            };
            await _context.Medicos.AddAsync(medico);
            await _context.SaveChangesAsync();
            Console.WriteLine("Médico criado com sucesso: " + medico.Id);
            return medico;
        }

        public async Task<Usuario> LoginUser(LoginUserModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return user;
            }

            throw new Exception("Credenciais inválidas.");
        }

        private string ChangePasswordEmail(string callbackUrl)
        {
            string Response = "<div style=\"width:100%;background-color:#FFAC1C;text-align:center;margin:10px\">";
            Response += "<h1>\"Defina sua senha\"</h1>";
            Response += "<img src=\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSggdch-qeLH8k6laogfDGkEPGQKVjawcTmnA&s\" />";
            Response += $"<h3>\"Por favor, defina sua senha clicando aqui: <a href='{callbackUrl}'>link</a>\"</h3>";
            Response += "<div><h1>vitaltech@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
