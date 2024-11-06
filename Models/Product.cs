using System;
using System.Collections.Generic;

namespace AGECorporate.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public int? ProductCategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }
}
