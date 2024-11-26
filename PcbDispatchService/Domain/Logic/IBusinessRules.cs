using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic;

public interface IBusinessRules
{
    /// <summary>
    /// Проверяет, возможно ли перевести плату в следующее состояние безнес-процесса.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Сообщение-статус проверки бизнес-правил.</returns>
    /// <exception cref="InvalidOperationException">Невозможно осуществить проверку.</exception>
    BusinessProcessStatusEnum CheckIfContinuationIsPossible(PrintedCircuitBoard printedCircuitBoard);
}