using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace api_sistema_de_empleados.CQRS.Consulta.Empleados
{
    /// <summary>
    /// Contains the query and handler for retrieving all employees.
    /// </summary>
    public class GetEmpleados
    {
        /// <summary>
        /// Represents the query to get all employees.
        /// This query, when handled, will return a list of <see cref="EmpleadosDTO"/>.
        /// </summary>
        public class GetEmpleadosQuery : IRequest<List<EmpleadosDTO>>
        {

        }

        /// <summary>
        /// Handles the <see cref="GetEmpleadosQuery"/> to retrieve all employees.
        /// </summary>
        public class GetEmpleadosQueryHandler : IRequestHandler<GetEmpleadosQuery, List<EmpleadosDTO>>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper; // IMapper is injected but not used in the current implementation for mapping Empleado to EmpleadosDTO.

            /// <summary>
            /// Initializes a new instance of the <see cref="GetEmpleadosQueryHandler"/> class.
            /// </summary>
            /// <param name="context">The database application context.</param>
            /// <param name="mapper">The AutoMapper instance (currently not used for the main mapping in this handler).</param>
            public GetEmpleadosQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            /// <summary>
            /// Handles the request to get all employees.
            /// Fetches all employees from the database, manually maps them to <see cref="EmpleadosDTO"/> objects,
            /// and includes the description of their respective job title (TipoCargo).
            /// Note: The injected IMapper is not utilized for the primary mapping of Empleado to EmpleadosDTO in this method.
            /// </summary>
            /// <param name="request">The <see cref="GetEmpleadosQuery"/> request object.</param>
            /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
            /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="EmpleadosDTO"/>.</returns>
            public async Task<List<EmpleadosDTO>> Handle(GetEmpleadosQuery request, CancellationToken cancellationToken)
            {
                var empleados = await _context.Empleados.ToListAsync(cancellationToken);
                var empleadosDTO = new List<EmpleadosDTO>();

                foreach (var empleado in empleados)
                {
                    // Manually mapping properties and fetching related Cargo description
                    var cargo = await _context.TipoCargos.FirstOrDefaultAsync(c => c.IdCargo == empleado.IdCargo, cancellationToken);
                    var empleadoDTO = new EmpleadosDTO
                    {
                        IdEmpleado = empleado.IdEmpleado,
                        Nombre = empleado.Nombre,
                        Apellido = empleado.Apellido,
                        IdCargo = empleado.IdCargo,
                        Edad = empleado.Edad,
                        Salario = empleado.Salario,
                        Cargo = cargo?.Descripcion // Assign the cargo name
                    };
                    empleadosDTO.Add(empleadoDTO);
                }

                return empleadosDTO;
            }
        }
    }
}
