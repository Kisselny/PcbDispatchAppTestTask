using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Domain.Logic.States;

public abstract class BusinessProcessStateBase
{
    public int Id { get; set; }
    public abstract void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard);
    public abstract BusinessProcessStatusEnum GetCurrentStatus();
}