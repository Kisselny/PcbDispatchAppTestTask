using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class QualityControlState : IBusinessProcessState
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;

    public QualityControlState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules)
    {
        _stateFactory = stateFactory;
        _loggerService = loggerService;
        _businessRules = businessRules;
    }


    public void AdvanceToNextState(Pcb pcb)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(pcb);
        if (result == _businessRules.okMessage)
        {
            _loggerService.LogThisSh_t("Качество в порядке, переход к шагу \"Упаковка\"");
            pcb.SetBusinessState(_stateFactory.CreatePackagingState());
        }
        else
        {
            _loggerService.LogThisSh_t(result);
            pcb.SetBusinessState(_stateFactory.CreateRepairState());
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.QualityControl;
    }
}