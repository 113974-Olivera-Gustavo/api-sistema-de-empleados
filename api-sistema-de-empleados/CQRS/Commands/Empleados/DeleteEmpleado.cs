using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using api_sistema_de_empleados.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Commands.Empleados
{
    public class DeleteEmpleado
    {
        public class DeleteEmpleadoCommand : IRequest<Empleado>
        {
            public int Id { get; set; }
        }
        public class DeleteEmpleadoCommandValidator : AbstractValidator<DeleteEmpleadoCommand>
        {
            private readonly ApplicationContext _context;
            public DeleteEmpleadoCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Id).NotEmpty().WithMessage("El id es requerido");
                RuleFor(x => x).MustAsync(EmpleadoExiste).WithMessage("El empleado no existe");
            }

            private async Task<bool> EmpleadoExiste(DeleteEmpleadoCommand command, CancellationToken token)
            {
                bool existe = await _context.Empleados.AnyAsync(x => x.IdEmpleado == command.Id);
                return existe;
            }
        }
        public class DeleteEmpleadoCommandHandler : IRequestHandler<DeleteEmpleadoCommand, Empleado>
        {
            private readonly ApplicationContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<DeleteEmpleadoCommand> _validator;
            
            public DeleteEmpleadoCommandHandler(ApplicationContext context, IMapper mapper, IValidator<DeleteEmpleadoCommand> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<Empleado> Handle(DeleteEmpleadoCommand request, CancellationToken cancellationToken)
            {
                var validator = new DeleteEmpleadoCommandValidator(_context);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                else
                {
                    var empleadoDelete = await _context.Empleados.FindAsync(request.Id);
                    _context.Empleados.Remove(empleadoDelete);
                    await _context.SaveChangesAsync();
                    return empleadoDelete;
                }
            }
        }
    }
}
