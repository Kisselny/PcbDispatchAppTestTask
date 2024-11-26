using System.ComponentModel;

namespace PcbDispatchService.Domain.Logic.States;

/// <summary>
/// Описание статусов бизнес-процесса.
/// </summary>
public enum BusinessProcessStatusEnum
{
    /// <summary>
    /// Статус регистрации.
    /// </summary>
    [Description("Регистрация")]
    Registration,
    /// <summary>
    /// Статус установки компонентов.
    /// </summary>
    [Description("Установка компонентов")]
    ComponentInstallation,
    /// <summary>
    /// Статус контроля качества.
    /// </summary>
    [Description("Контроль качества")]
    QualityControl,
    /// <summary>
    /// Статус ремонта.
    /// </summary>
    [Description("Ремонт")]
    Repair,
    /// <summary>
    /// Статус упаковки.
    /// </summary>
    [Description("Упаковка")]
    Packaging
}