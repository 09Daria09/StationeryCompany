using System;
using System.Collections.Generic;
using StationeryCompany.Model;

namespace StationeryCompany.Model;

public partial class CustomerCompany
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? City { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
