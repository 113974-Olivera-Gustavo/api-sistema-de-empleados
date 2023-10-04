using System;
using System.Collections.Generic;

namespace api_sistema_de_empleados.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int IdTipoCliente { get; set; }

    public string Email { get; set; } = null!;

    public virtual TipoCliente IdTipoClienteNavigation { get; set; } = null!;
}
