using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Consulta.Empleados
{
    public class GetEmpleados
    {
        public class GetEmpleadosQuery : IRequest<List<EmpleadosDTO>>
        {

        }
        public class GetEmpleadosQueryHandler : IRequestHandler<GetEmpleadosQuery, List<EmpleadosDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            public GetEmpleadosQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<EmpleadosDTO>> Handle(GetEmpleadosQuery request, CancellationToken cancellationToken)
            {
                var empleados = await _context.Empleados.ToListAsync();
                var empleadosDTO = new List<EmpleadosDTO>();

                foreach (var empleado in empleados)
                {
                    var cargo = await _context.TipoCargos.FirstOrDefaultAsync(c => c.IdCargo == empleado.IdCargo);
                    var empleadoDTO = new EmpleadosDTO
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        Nombre = empleado.Nombre,
                        Apellido = empleado.Apellido,
                        IdCargo = empleado.IdCargo,
                        Edad = empleado.Edad,
                        Salario = empleado.Salario,
                        Cargo = cargo?.Descripcion // Asignar el nombre del cargo
                    };
                    empleadosDTO.Add(empleadoDTO);
                }

                return empleadosDTO;

            }
        }


    }
}
