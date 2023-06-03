using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepairShop.Data.Enums;

public enum RequestStatuses
{
    AwaitsConfirmation = 1,
    TransferedToMaster = 2,
    Repairing = 3,
    ReadyToTake = 4,
    Finished = 5
}
