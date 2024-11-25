using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic;

public interface IBusinessRules
{
    string CheckIfContinuationIsPossible(PrintedCircuitBoard printedCircuitBoard);
}