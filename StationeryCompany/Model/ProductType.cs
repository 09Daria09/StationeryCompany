using System;
using System.Collections.Generic;

namespace StationeryCompany.Model;

public partial class ProductType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
