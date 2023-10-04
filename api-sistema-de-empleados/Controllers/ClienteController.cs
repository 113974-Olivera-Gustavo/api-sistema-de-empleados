using api_sistema_de_empleados.CQRS.Commands.Clientes;
using api_sistema_de_empleados.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.DeleteCliente;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.PostCliente;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.PutCliente;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.DeleteEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PostEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PutEmpleado;
using static api_sistema_de_empleados.CQRS.Consulta.Clientes.GetCliente;

namespace api_sistema_de_empleados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ClienteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("/getClientes")]
        public async Task<List<ClientesDTO>> GetClientes()
        {
            return await _mediator.Send(new GetClienteQuery());
        }
        [HttpPost]
        [Route("/postCliente")]
        public async Task<IActionResult> PostCliente(PostClienteCommand cmd)
        {
            try
            {
                var postCliente = await _mediator.Send(cmd);
                return CreatedAtAction(nameof(PostCliente), postCliente);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("/deleteCliente/{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var deleteCliente = await _mediator.Send(new DeleteClienteCommand { IdCliente = id });
                return Ok(deleteCliente);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("/putCliente/{id}")]
        public async Task<IActionResult> PutCliente(int id, PutClienteCommand cmd)
        {   
            try
            {
                if (id != cmd.IdCliente)
                {
                    return BadRequest();
                }
                var putCliente = await _mediator.Send(cmd);
                return Ok(putCliente);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
