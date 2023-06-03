using Microsoft.EntityFrameworkCore;
using RepairShop.Data.Models;

namespace RepairShop.Data;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RepairRequest> RepairRequests { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StatusHistory> StatusHistories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RepairRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("repair_request_pkey");

            entity.ToTable("repair_request");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.MasterId).HasColumnName("master_id");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .HasColumnName("short_name");

            entity.HasOne(d => d.Client).WithMany(p => p.RepairRequestClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("repair_request_client_id_fkey");

            entity.HasOne(d => d.Master).WithMany(p => p.RepairRequestMasters)
                .HasForeignKey(d => d.MasterId)
                .HasConstraintName("repair_request_master_id_fkey");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("request_status_pkey");

            entity.ToTable("request_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_history_pkey");

            entity.ToTable("status_history");

            entity.HasIndex(e => new { e.Id, e.IsActual }, "status_history_id_is_actual_idx")
                .IsUnique()
                .HasFilter("(is_actual = true)");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateChanged).HasColumnName("date_changed");
            entity.Property(e => e.IsActual).HasColumnName("is_actual");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.Request).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("status_history_request_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("status_history_status_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_id_fkey");
        });
    }
}
