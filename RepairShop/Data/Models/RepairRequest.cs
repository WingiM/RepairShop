﻿namespace RepairShop.Data.Models;

public class RepairRequest
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public int? MasterId { get; set; }

    public string ShortName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual User Client { get; set; } = null!;

    public virtual User? Master { get; set; }

    public RequestStatus? ActualStatus => StatusHistories.FirstOrDefault(x => x.IsActual)?.Status;

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
