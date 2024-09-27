using System;
using System.Collections.Generic;

namespace AGECorporate.Models;

public partial class Query
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Query1 { get; set; } = null!;

    public string? Reply { get; set; }
}
