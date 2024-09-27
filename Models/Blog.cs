using System;
using System.Collections.Generic;

namespace AGECorporate.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string Header { get; set; } = null!;

    public string? AssociatedImages { get; set; }

    public string Content { get; set; } = null!;
}
