using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Commands.Empleados
{
    public class PutEmpleado
    {
        public class PutEmpleadoCommand : IRequest<EmpleadosDTO>
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = null!;
            public string Apellido { get; set; } = null!;
            public int idCargo { get; set; }
            public int Edad { get; set; }
            public decimal Salario { get; set; }

        }
        public class PutEmpleadoCommandValidator : AbstractValidator<PutEmpleadoCommand>
        {
            private readonly ApplicationContext _context;
            public PutEmpleadoCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido es requerido");
                RuleFor(x => x.idCargo).NotEmpty().WithMessage("El cargo es requerido");
                RuleFor(x => x.Edad).NotEmpty().WithMessage("La edad es requerida");
                RuleFor(x => x.Salario).NotEmpty().WithMessage("El salario es requerido");
                RuleFor(x => x).MustAsync(EmpleadoExiste).WithMessage("El empleado no existe");
            }

            private async Task<bool> EmpleadoExiste(PutEmpleadoCommand command, CancellationToken token)
            {
                bool existe = await _context.Empleados.AnyAsync(x => x.IdEmpleado == command.Id);
                return existe;
            }
        }

        public class PutEmpleadoCommandHandler : IRequestHandler<PutEmpleadoCommand, EmpleadosDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PutEmpleadoCommand> _validator;
            public PutEmpleadoCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PutEmpleadoCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<EmpleadosDTO> Handle(PutEmpleadoCommand request, CancellationToken cancellationToken)
            {
                
                    var validationResult = await _validator.ValidateAsync(request);

                    if (!validationResult.IsValid)
                    {
                        throw new Exception(validationResult.ToString());
                    }
                    else
                    {
                        var empleadosUpd = await _context.Empleados.FirstOrDefaultAsync(p => p.IdEmpleado == request.Id);

                        if (empleadosUpd == null)
                        {
                            throw new Exception("No se encontro al empleado");
                        }
                        empleadosUpd.Nombre = request.Nombre;
                        empleadosUpd.Apellido = request.Apellido;
                        empleadosUpd.Edad = request.Edad;
                        empleadosUpd.Salario = request.Salario;
                        empleadosUpd.IdCargo = request.idCargo;
                        await _context.SaveChangesAsync();
                        var personaDTO = _mapper.Map<EmpleadosDTO>(empleadosUpd);
                        return personaDTO;
                    }                     
            }
        }
    }
}
