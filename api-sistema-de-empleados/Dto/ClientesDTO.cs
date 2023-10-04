namespace api_sistema_de_empleados.Dto
{
    public class ClientesDTO
    {
        public int IdCliente { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public int IdTipoCliente { get; set; }

        public string Email { get; set; } = null!;
        public string TipoCliente { get; set; } = null!;
       
    }
}
