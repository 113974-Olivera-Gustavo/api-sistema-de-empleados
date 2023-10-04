using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using api_sistema_de_empleados.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Commands.Empleados
{
    public class PostEmpleado
    {
        public class PostEmpleadoCommand : IRequest<EmpleadosDTO>
        {
            public string Nombre { get; set; } = null;
            public string Apellido { get; set; } = null;
            public int idCargo { get; set; }
            public int Edad { get; set; }
            public decimal Salario { get; set; }

        }
        public class PostEmpleadoCommandValidator : AbstractValidator<PostEmpleadoCommand>
        {
            private readonly ApplicationContext _context;
            public PostEmpleadoCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es requerido");
                RuleFor(x => x.Apellido).NotEmpty().WithMessage("El apellido es requerido");
                RuleFor(x => x.idCargo).NotEmpty().WithMessage("El cargo es requerido");
                RuleFor(x => x.Edad).NotEmpty().WithMessage("La edad es requerida");
                RuleFor(x => x.Salario).NotEmpty().WithMessage("El salario es requerido");
                RuleFor(x => x).MustAsync(EmpleadoExiste).WithMessage("El empleado ya existe");
            }

            private async Task<bool> EmpleadoExiste(PostEmpleadoCommand command, CancellationToken token)
            {
                bool existe = await _context.Empleados.AnyAsync(x => x.Nombre == command.Nombre && x.Apellido == command.Apellido && x.Edad == command.Edad && x.IdCargo == command.idCargo);
                return !existe;
            }
        }
        public class PostEmpleadoCommandHandler : IRequestHandler<PostEmpleadoCommand, EmpleadosDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<PostEmpleadoCommand> _validator;
            public PostEmpleadoCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostEmpleadoCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<EmpleadosDTO> Handle(PostEmpleadoCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var empleadoResult = await _validator.ValidateAsync(request);
                    if (!empleadoResult.IsValid)
                    {
                        throw new ValidationException(empleadoResult.Errors);
                    }
                    else
                    {
                        var empleado = _mapper.Map<Empleado>(request);
                        await _context.Empleados.AddAsync(empleado);
                        await _context.SaveChangesAsync();
                        var empleadoDto = _mapper.Map<EmpleadosDTO>(empleado);
                        return empleadoDto;
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
