using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RepairShop.Data;

public interface IApplicationContext
{
    public ChangeTracker ChangeTracker { get; }
    
    public DbSet<RepairRequest> RepairRequests { get; set; }

    public DbSet<RequestStatus> RequestStatuses { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<StatusHistory> StatusHistories { get; set; }

    public DbSet<User> Users { get; set; }

    public int SaveChanges();
}