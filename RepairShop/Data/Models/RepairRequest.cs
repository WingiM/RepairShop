using System;
using System.Collections.Generic;

namespace RepairShop.Data.Models;

public partial class RepairRequest
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int? MasterId { get; set; }

    public string ShortName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual User Client { get; set; } = null!;

    public virtual User? Master { get; set; }

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
