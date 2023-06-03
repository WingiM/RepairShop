using System.ComponentModel.DataAnnotations;

namespace RepairShop.Data.Enums;

public enum RequestStatuses
{
    [Display(Name = "Ожидает подтверждения")]
    AwaitsConfirmation = 1,

    [Display(Name = "Передано мастеру")]
    TransferedToMaster = 2,

    [Display(Name = "В ремонте")]
    Repairing = 3,

    [Display(Name = "Готово к получению")]
    ReadyToTake = 4,

    [Display(Name = "Завершено")]
    Finished = 5
}
