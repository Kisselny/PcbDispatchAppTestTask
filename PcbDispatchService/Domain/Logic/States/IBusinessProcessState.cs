using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic.States;

public interface IBusinessProcessState
{
    void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard);
    BusinessProcessStatusEnum GetCurrentStatus();
    string GetCurrentStatusString();
}