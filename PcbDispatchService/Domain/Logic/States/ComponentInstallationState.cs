using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class ComponentInstallationState : IBusinessProcessState
{
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public ComponentInstallationState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }

    public void AdvanceToNextState(Pcb pcb)
    {
        pcb.SetBusinessState(new QualityControlState(_loggerService, _businessRules, _qualityControlService));
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.ComponentInstallation;
    }
}