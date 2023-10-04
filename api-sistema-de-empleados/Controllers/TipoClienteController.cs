using api_sistema_de_empleados.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static api_sistema_de_empleados.CQRS.Consulta.Clientes.GetTipoCliente;

namespace api_sistema_de_empleados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoClienteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TipoClienteController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("/getTipoClientes")]
        public async Task<List<TipoClienteDTO>> GetTipoClientes()
        {
            return await _mediator.Send(new GetTipoClienteQuery());
        }
   
    }
}
