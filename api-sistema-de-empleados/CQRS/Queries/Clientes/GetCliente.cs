using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Consulta.Clientes
{
    public class GetCliente
    {
        public class GetClienteQuery : IRequest<List<ClientesDTO>>
        {

        }
        public class GetClienteQueryHandler : IRequestHandler<GetClienteQuery, List<ClientesDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            public GetClienteQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ClientesDTO>> Handle(GetClienteQuery request, CancellationToken cancellationToken)
            {
                var clientes = await _context.Clientes.ToListAsync();
                var tiposCliente = await _context.TipoClientes.ToListAsync();
                var clientesDTO = new List<ClientesDTO>();

                foreach (var cliente in clientes)
                {
                    var tipoCliente = tiposCliente.FirstOrDefault(tc => tc.IdTipoCliente == cliente.IdTipoCliente);

                    var clienteDTO = new ClientesDTO
                    {
                        IdCliente = cliente.IdCliente,
                        Nombre = cliente.Nombre,
                        Apellido = cliente.Apellido,
                        IdTipoCliente = cliente.IdTipoCliente,
                        TipoCliente = tipoCliente?.TipoCliente1,
                        Email = cliente.Email
                    };

                    clientesDTO.Add(clienteDTO);
                }

                return clientesDTO;

            }
        }
    }
}
