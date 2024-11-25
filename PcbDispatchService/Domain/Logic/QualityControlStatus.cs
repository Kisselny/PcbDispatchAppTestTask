using System.ComponentModel;

namespace PcbDispatchService.Domain.Logic;

public enum QualityControlStatus
{
    [Description("Качество не установлено")]
    NotSureYet,
    [Description("Качество в порядке")]
    QualityIsOk,
    [Description("Качество не в порядке")]
    QualityIsBad,
    [Description("Признано браком")]
    Defective
}