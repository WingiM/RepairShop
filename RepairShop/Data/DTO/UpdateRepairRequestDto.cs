using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.DTO;

public class UpdateRepairRequestDto
{
    public int RequestId { get; set; }

    public string ShortName { get; set; } = null!;

    public string Description { get; set; } = null!;
}
