using System;
using System.Collections.Generic;

namespace api_sistema_de_empleados.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int IdCargo { get; set; }

    public int Edad { get; set; }

    public decimal Salario { get; set; }

    public virtual TipoCargo IdCargoNavigation { get; set; } = null!;
}
