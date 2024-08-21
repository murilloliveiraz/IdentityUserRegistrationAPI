using APIThatSendAnEmailToUpdatePassword.Context;
using APIThatSendAnEmailToUpdatePassword.DTOS;
using APIThatSendAnEmailToUpdatePassword.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIThatSendAnEmailToUpdatePassword.Controllers
{
    [ApiController]
    [Route("api/medicos")]
    public class MedicosController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly UsersContext _context;

        public MedicosController(UsersService usersService, UsersContext context)
        {
            _usersService = usersService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterMedico(CreateMedicoModel model)
        {
            try
            {
                var user = await _usersService.RegisterMedico(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMedicos()
        {
            var medicos = await _context.Medicos.ToListAsync();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedicoById(string id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return Ok(medico);
        }
    }
}