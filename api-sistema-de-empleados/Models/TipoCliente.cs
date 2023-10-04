using System;
using System.Collections.Generic;

namespace api_sistema_de_empleados.Models;

public partial class TipoCliente
{
    public int IdTipoCliente { get; set; }

    public string TipoCliente1 { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
