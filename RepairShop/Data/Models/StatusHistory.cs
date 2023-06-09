﻿using System;

namespace RepairShop.Data.Models;

public class StatusHistory
{
    public int Id { get; set; }

    public int RequestId { get; set; }

    public DateTime DateChanged { get; set; }

    public int StatusId { get; set; }

    public bool IsActual { get; set; }

    public virtual RepairRequest Request { get; set; } = null!;

    public virtual RequestStatus Status { get; set; } = null!;
}
