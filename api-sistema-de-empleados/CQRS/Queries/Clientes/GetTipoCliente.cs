using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Consulta.Clientes
{
    public class GetTipoCliente
    {
        public class GetTipoClienteQuery : IRequest<List<TipoClienteDTO>>
        {

        }
        public class GetTipoClienteQueryHandler : IRequestHandler<GetTipoClienteQuery, List<TipoClienteDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            public GetTipoClienteQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TipoClienteDTO>> Handle(GetTipoClienteQuery request, CancellationToken cancellationToken)
            {
                var tipoCliente = await _context.TipoClientes.ToListAsync();
                var data = _mapper.Map<List<TipoClienteDTO>>(tipoCliente);
                return data;

            }
        }
    }
}
