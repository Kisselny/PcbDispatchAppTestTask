using System.ComponentModel;

namespace PcbDispatchService.Domain.Logic;

/// <summary>
/// Описание статусов контроля качества.
/// </summary>
public enum QualityControlStatus
{
    /// <summary>
    /// Качество не установлено.
    /// </summary>
    [Description("Качество не установлено")]
    NotSureYet,
    /// <summary>
    /// Качество в порядке.
    /// </summary>
    [Description("Качество в порядке")]
    QualityIsOk,
    /// <summary>
    /// Качество не в порядке.
    /// </summary>
    [Description("Качество не в порядке")]
    QualityIsBad,
    /// <summary>
    /// Признано браком.
    /// </summary>
    [Description("Признано браком")]
    Defective
}