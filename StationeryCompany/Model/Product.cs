using System;
using System.Collections.Generic;

namespace StationeryCompany.Model;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public int? TypeId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Cost { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ProductType? Type { get; set; }
}
