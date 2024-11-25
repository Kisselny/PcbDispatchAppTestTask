using System.ComponentModel;

namespace PcbDispatchService.Domain.Logic.States;

public enum BusinessProcessStatusEnum
{
    [Description("Регистрация")]
    Registration,
    [Description("Установка компонентов")]
    ComponentInstallation,
    [Description("Контроль качества")]
    QualityControl,
    [Description("Ремонт")]
    Repair,
    [Description("Упаковка")]
    Packaging
}