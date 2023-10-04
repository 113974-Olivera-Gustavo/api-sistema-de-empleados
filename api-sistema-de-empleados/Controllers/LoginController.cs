using MediatR;
using Microsoft.AspNetCore.Mvc;
using static api_sistema_de_empleados.CQRS.Commands.Login.PostLogin;

namespace api_sistema_de_empleados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator _mediator;
        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(PostLoginCommand cmd)
        {
            try
            {
                var postUsuario = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(Login), postUsuario);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
