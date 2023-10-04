using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Consulta.Empleados
{
    public class GetCargos
    {
        public class GetAllCargosQuery : IRequest<List<TipoCargoDTO>>
        {
        
        }
        public class GetAllCargosQueryHandler : IRequestHandler<GetAllCargosQuery, List<TipoCargoDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            public GetAllCargosQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TipoCargoDTO>> Handle(GetAllCargosQuery request, CancellationToken cancellationToken)
            {
                var tiposCargos = await _context.TipoCargos.ToListAsync();
                var data = _mapper.Map<List<TipoCargoDTO>>(tiposCargos);
                return data;

            }
        }

    }
}
