using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic.States;

public interface IBusinessProcessState
{
    void AdvanceToNextState(Pcb pcb);
    BusinessProcessStatusEnum GetCurrentStatus();
}