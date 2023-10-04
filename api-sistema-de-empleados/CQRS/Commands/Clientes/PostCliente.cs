using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using api_sistema_de_empleados.Models;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace api_sistema_de_empleados.CQRS.Commands.Clientes
{
    public class PostCliente
    {
        public class PostClienteCommand : IRequest<ClientesDTO>
        {
            public string Nombre { get; set; } = null!;
            public string Apellido { get; set; } = null!;
            public int IdTipoCliente { get; set; }
            public string Email { get; set; } = null!;
        }
        public class PostClienteCommandValidator : AbstractValidator<PostClienteCommand>
        {
            private readonly ApplicationContext _context;
            public PostClienteCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido");
                RuleFor(x => x.IdTipoCliente).NotEmpty().WithMessage("Tipo de cliente es requerido");
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email es requerido");
                RuleFor(x => x).MustAsync(ClienteExiste).WithMessage("El cliente ya existe");
            }

            private async Task<bool> ClienteExiste(PostClienteCommand command, CancellationToken token)
            {
                bool existe = await _context.Clientes.AnyAsync(x => x.Nombre == command.Nombre && x.Apellido == command.Apellido && x.IdCliente == command.IdTipoCliente && x.Email == command.Email);
                return !existe;
            }
        }
        public class PostClienteCommandHandler : IRequestHandler<PostClienteCommand, ClientesDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IValidator<PostClienteCommand> _validator;
            private readonly IMapper _mapper;
            public PostClienteCommandHandler(ApplicationContext context, IValidator<PostClienteCommand> validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<ClientesDTO> Handle(PostClienteCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var clienteResul = await _validator.ValidateAsync(request);
                    if (!clienteResul.IsValid)
                    {
                        throw new ValidationException(clienteResul.Errors);
                    }
                    else
                    {
                        var cliente = _mapper.Map<Cliente>(request);
                        await _context.Clientes.AddAsync(cliente);
                        await _context.SaveChangesAsync();
                        var clienteDto = _mapper.Map<ClientesDTO>(cliente);
                        return clienteDto;
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
