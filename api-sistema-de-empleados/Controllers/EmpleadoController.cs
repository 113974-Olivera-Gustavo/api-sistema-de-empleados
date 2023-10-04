using api_sistema_de_empleados.CQRS.Consulta.Empleados;
using api_sistema_de_empleados.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.DeleteEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PostEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PutEmpleado;
using static api_sistema_de_empleados.CQRS.Consulta.Empleados.GetEmpleados;

namespace api_sistema_de_empleados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmpleadoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("/getEmpleados")]
        public async Task<List<EmpleadosDTO>> GetPersonas()
        {
            return await _mediator.Send(new GetEmpleadosQuery());
        }
        [HttpPost]
        [Route("/postEmpleado")]
        public async Task<IActionResult> PostEmpleado(PostEmpleadoCommand cmd)
        {
            try
            {
                var postEmpleado = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(PostEmpleado), postEmpleado);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("/deleteEmpleado/{id}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
            try
            {
                var deleteEmpleado = await _mediator.Send(new DeleteEmpleadoCommand { Id = id });
                return Ok(deleteEmpleado);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("/putEmpleado/{id}")]
        public async Task<IActionResult> PutEmpleado(int id, PutEmpleadoCommand cmd)
        {
            try
            {
                if (id != cmd.Id)
                {
                    return BadRequest();
                }
                var putEmpleado = await _mediator.Send(cmd);
                return Ok(putEmpleado);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
