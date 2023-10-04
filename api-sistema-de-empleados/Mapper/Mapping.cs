using api_sistema_de_empleados.Dto;
using api_sistema_de_empleados.Models;
using AutoMapper;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.PostCliente;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PostEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Login.PostLogin;

namespace api_sistema_de_empleados.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Empleado, EmpleadosDTO>().ReverseMap();
            CreateMap<TipoCargo, TipoCargoDTO>().ReverseMap();
            CreateMap<Cliente, ClientesDTO>().ReverseMap();
            CreateMap<TipoCliente, TipoClienteDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<Rol, RolDTO>().ReverseMap();
            CreateMap<PostLoginCommand, Usuario>().ReverseMap();
            CreateMap<PostEmpleadoCommand, Empleado>().ReverseMap();
            CreateMap<PostClienteCommand, Cliente>().ReverseMap();
        }
    }
}
