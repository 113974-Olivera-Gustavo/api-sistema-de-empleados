using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using api_sistema_de_empleados.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.DeleteEmpleado;

namespace api_sistema_de_empleados.CQRS.Commands.Clientes
{
    public class DeleteCliente
    {
        public class DeleteClienteCommand : IRequest<Cliente>
        {
            public int IdCliente { get; set; }
        }
        public class DeleteClienteValidator : AbstractValidator<DeleteClienteCommand>
        {
            private readonly ApplicationContext _context;
            public DeleteClienteValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.IdCliente).NotEmpty();
                RuleFor(x => x).MustAsync(ClienteExiste).WithMessage("El cliente no existe");
            }

            private async Task<bool> ClienteExiste(DeleteClienteCommand command, CancellationToken token)
            {
                bool existe = await _context.Clientes.AnyAsync(x => x.IdCliente == command.IdCliente);
                return existe;
            }
        }
        public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, Cliente>
        {
            private readonly ApplicationContext _context;
            private readonly IValidator<DeleteClienteCommand> _validator;
            private readonly IMapper _mapper;
            public DeleteClienteCommandHandler(ApplicationContext context, IValidator<DeleteClienteCommand> validator, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
                _validator = validator;
            }

            public async Task<Cliente> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
            {
                var validator = new DeleteClienteValidator(_context);
                var validationResult = await validator.ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }
                else
                {
                    var clienteDelete = await _context.Clientes.FindAsync(request.IdCliente);
                    _context.Clientes.Remove(clienteDelete);
                    await _context.SaveChangesAsync();
                    return clienteDelete;
                }
            }
        }
    }
}
