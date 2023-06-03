using RepairShop.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.DTO;

public class ChangeRequestStatusDto
{
    public int RequestId { get; set; }
    public RequestStatuses Status { get; set; }
}
