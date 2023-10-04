using System;
using System.Collections.Generic;

namespace api_sistema_de_empleados.Models;

public partial class TipoCargo
{
    public int IdCargo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
