namespace api_sistema_de_empleados.Dto
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public string NombreUsuario { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int IdRol { get; set; }
    }
}
