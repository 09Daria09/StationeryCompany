using System;
using System.Collections.Generic;

namespace StationeryCompany.Model;

public partial class SalesManager
{
    public int ManagerId { get; set; }

    public string? ManagerName { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
