using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class RepairState : IBusinessProcessState
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public RepairState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _stateFactory = stateFactory;
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }

    public void AdvanceToNextState(Pcb pcb)
    {
        pcb.QualityControlStatus = _qualityControlService.TryRepair(pcb);
        switch (pcb.QualityControlStatus)
        {
            case QualityControlStatus.QualityIsOk:
                _loggerService.LogThisSh_t("Ремонт произведён, возвращение к шагу \"Контроль качества\"");
                pcb.SetBusinessState(_stateFactory.CreateQualityControlState());
                break;
            default:
                _loggerService.LogThisSh_t("Ремонт не удалось произвести, плата признана браком.");
                pcb.QualityControlStatus = QualityControlStatus.Defective;
                break;
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Repair;
    }
}