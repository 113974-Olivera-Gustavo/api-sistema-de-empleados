using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace api_sistema_de_empleados.CQRS.Commands.Clientes
{
    public class PutCliente
    {
        public class PutClienteCommand : IRequest<ClientesDTO>
        {
            public int IdCliente { get; set; }
            public string Nombre { get; set; } = null!;
            public string? Apellido { get; set; }
            public int IdTipoCliente { get; set; }
            public string Email { get; set; } = null!;

        }
        public class PutClienteCommandValidator : AbstractValidator<PutClienteCommand>
        {
            private readonly ApplicationContext _context;
            public PutClienteCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Nombre).NotEmpty().WithMessage("Nombre es requerido");
                RuleFor(x => x.IdTipoCliente).NotEmpty().WithMessage("Tipo de cliente es requerido");
                RuleFor(x => x.Email).NotEmpty().WithMessage("Email es requerido");
                RuleFor(x => x).MustAsync(ClienteExiste).WithMessage("El cliente ya existe");
               
            }

            private async Task<bool> ClienteExiste(PutClienteCommand command, CancellationToken token)
            {
                bool existe= await _context.Clientes.AnyAsync(x => x.IdCliente == command.IdCliente);
                return !existe;
            }
        }
        public class PutClienteCommandHandler : IRequestHandler<PutClienteCommand, ClientesDTO>
        {
            private readonly ApplicationContext _context;
            private readonly IValidator<PutClienteCommand> _validator;
            private readonly IMapper _mapper;
            public PutClienteCommandHandler(ApplicationContext context, IValidator<PutClienteCommand> validator, IMapper mapper)
            {
                _context = context;
                _validator = validator;
                _mapper = mapper;
            }

            public async Task<ClientesDTO> Handle(PutClienteCommand request, CancellationToken cancellationToken)
            {

                var validationResult = await _validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    throw new Exception(validationResult.ToString());
                }
                else
                {
                    var clientesUpd = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == request.IdCliente);

                    if (clientesUpd == null)
                    {
                        throw new Exception("No se encontro al cliente");
                    }
                    clientesUpd.Nombre = request.Nombre;
                    clientesUpd.Apellido = request.Apellido;
                    clientesUpd.Email = request.Email;
                    clientesUpd.IdTipoCliente = request.IdTipoCliente;
                    _context.Clientes.Update(clientesUpd);
                    await _context.SaveChangesAsync();
                    var personaDTO = _mapper.Map<ClientesDTO>(clientesUpd);
                    return personaDTO;
                }
            }
        }
    }
}
