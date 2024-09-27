using System;
using System.Collections.Generic;

namespace AGECorporate.Models;

public partial class TimelineEntry
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Header { get; set; } = null!;

    public string Content { get; set; } = null!;
}
