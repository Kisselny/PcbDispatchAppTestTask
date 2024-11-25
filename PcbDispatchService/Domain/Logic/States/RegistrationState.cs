using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class RegistrationState : IBusinessProcessState
{
    private readonly IStateFactory _stateFactory;
    private readonly LoggerService _loggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public RegistrationState(IStateFactory stateFactory, LoggerService loggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _loggerService = loggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
        _stateFactory = stateFactory;
    }

    public void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        var result = _businessRules.CheckIfContinuationIsPossible(printedCircuitBoard);
        if(result == _businessRules.okMessage)
        {
            _loggerService.LogThisSh_t("Регистрация пройдена, переход на этап добавления компонентов.");
            printedCircuitBoard.SetBusinessState(_stateFactory.CreateComponentInstallationState());
        }
        else
        {
            throw new BusinessException(result);
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Registration;
    }
    public string GetCurrentStatusString()
    {
        return GetCurrentStatus().ToString();
    }
}