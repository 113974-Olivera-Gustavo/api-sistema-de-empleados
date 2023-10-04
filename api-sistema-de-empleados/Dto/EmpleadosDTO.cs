namespace api_sistema_de_empleados.Dto
{
    public class EmpleadosDTO
    {
        public int IdEmpleado { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public int IdCargo { get; set; }

        public int Edad { get; set; }

        public decimal Salario { get; set; }
        public string Cargo { get; set; }

    }
}
