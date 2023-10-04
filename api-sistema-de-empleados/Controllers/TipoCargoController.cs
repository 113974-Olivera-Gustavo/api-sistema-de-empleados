using api_sistema_de_empleados.CQRS.Consulta.Empleados;
using api_sistema_de_empleados.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static api_sistema_de_empleados.CQRS.Consulta.Empleados.GetCargos;
using static api_sistema_de_empleados.CQRS.Consulta.Empleados.GetEmpleados;

namespace api_sistema_de_empleados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TipoCargoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TipoCargoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("/getCargos")]
        public async Task<List<TipoCargoDTO>> GetCargos()
        {
            return await _mediator.Send(new GetAllCargosQuery());
        }
    }
}
