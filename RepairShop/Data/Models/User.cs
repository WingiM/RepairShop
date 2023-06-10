namespace RepairShop.Data.Models;

public class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public virtual ICollection<RepairRequest> RepairRequestClients { get; set; } = new List<RepairRequest>();

    public virtual ICollection<RepairRequest> RepairRequestMasters { get; set; } = new List<RepairRequest>();

    public virtual Role Role { get; set; } = null!;
}
