using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic;

public interface IBusinessRules
{
    BusinessProcessStatusEnum CheckIfContinuationIsPossible(PrintedCircuitBoard printedCircuitBoard);
}