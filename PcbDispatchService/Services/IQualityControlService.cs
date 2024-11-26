using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

/// <summary>
/// Сервис контроля качества.
/// </summary>
public interface IQualityControlService
{
    /// <summary>
    /// Выполняет проверку качества.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Статус контроля качества.</returns>
    QualityControlStatus QualityCheck(PrintedCircuitBoard printedCircuitBoard);

    /// <summary>
    /// Выполняет попытку ремонта платы.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Новый статус контроля качества.</returns>
    /// <exception cref="BusinessException"></exception>
    QualityControlStatus TryRepair(PrintedCircuitBoard printedCircuitBoard);
}