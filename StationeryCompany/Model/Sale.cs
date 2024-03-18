using System;
using System.Collections.Generic;

namespace StationeryCompany.Model;

public partial class Sale
{
    public int SaleId { get; set; }

    public int? ProductId { get; set; }

    public int? ManagerId { get; set; }

    public int? CompanyId { get; set; }

    public int? QuantitySold { get; set; }

    public decimal? PricePerUnit { get; set; }

    public DateOnly? SaleDate { get; set; }

    public virtual CustomerCompany? Company { get; set; }

    public virtual SalesManager? Manager { get; set; }

    public virtual Product? Product { get; set; }
}
