using api_sistema_de_empleados.Data;
using api_sistema_de_empleados.Dto;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace api_sistema_de_empleados.CQRS.Commands.Login
{
    public class PostLogin
    {
      public class PostLoginCommand : IRequest<UsuarioDTO>
        {
            public string Usuario { get; set; }
            public string Password { get; set; }
            public int IdRol { get; set; }
        }
        public class PostLoginCommandValidator : AbstractValidator<PostLoginCommand>
        {
            private readonly ApplicationContext _context;
            public PostLoginCommandValidator(ApplicationContext context)
            {
                _context = context;
                RuleFor(x => x.Usuario).NotEmpty().WithMessage("El usuario no puede estar vacio");
                RuleFor(x => x.Password).NotEmpty().WithMessage("El password no puede estar vacio");
                RuleFor(x => x.IdRol).NotEmpty().WithMessage("El rol no puede estar vacio");
                RuleFor(x => x).MustAsync(UsuarioExiste).WithMessage("El usuario no existe");
            }

            private async Task<bool> UsuarioExiste(PostLoginCommand command, CancellationToken token)
            {
                bool existe = await _context.Usuarios.AnyAsync(x => x.NombreUsuario != command.Usuario
                                                                && x.Password != command.Password
                                                                && x.IdRol != command.IdRol);
                return !existe;
            }
            public class PostLoginCommandHandler : IRequestHandler<PostLoginCommand, UsuarioDTO>
            {
                private readonly ApplicationContext _context;
                private readonly IMapper _mapper;
                private readonly IValidator<PostLoginCommand> _validator;
                public PostLoginCommandHandler(ApplicationContext context, IMapper mapper, IValidator<PostLoginCommand> validator)
                {
                    _context = context;
                    _mapper = mapper;
                    _validator = validator;
                }

                public async Task<UsuarioDTO> Handle(PostLoginCommand request, CancellationToken cancellationToken)
                {
                    try
                    {
                        var usuarioResult = await _validator.ValidateAsync(request, cancellationToken);
                        if(!usuarioResult.IsValid)
                        {
                            throw new ValidationException(usuarioResult.Errors);
                        }
                        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.NombreUsuario == request.Usuario
                                                                                   && x.Password == request.Password && x.IdRol == request.IdRol);
                          if(usuario == null)
                        {
                              throw new Exception("Credenciales de inicio de sesion no validas");
                        }          
                        var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);
                        return usuarioDto;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }

        }
    }
}
