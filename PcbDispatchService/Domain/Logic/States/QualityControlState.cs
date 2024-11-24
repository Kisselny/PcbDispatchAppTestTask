using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class QualityControlState : IBusinessProcessState
{
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public QualityControlState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }


    public void AdvanceToNextState(Pcb pcb)
    {
        if (_qualityControlService.QualityCheck(pcb) == QualityControlStatus.QualityIsOk)
        {
            _loggerService.LogThisSh_t("Качество в порядке, переход к шагу \"Упаковка\"");
            pcb.SetBusinessState(new PackagingState(_loggerService, _businessRules, _qualityControlService));
        }
        else
        {
            _loggerService.LogThisSh_t("Качество не в порядке, переход к шагу \"Ремонт\"");
            pcb.SetBusinessState(new RepairState(_loggerService, _businessRules, _qualityControlService));
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.QualityControl;
    }
}