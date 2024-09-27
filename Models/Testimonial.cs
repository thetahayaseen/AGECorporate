using System;
using System.Collections.Generic;

namespace AGECorporate.Models;

public partial class Testimonial
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Testimonial1 { get; set; } = null!;
}
