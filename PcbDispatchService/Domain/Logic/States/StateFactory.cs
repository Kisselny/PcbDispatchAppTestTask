using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class StateFactory : IStateFactory
{
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;
    private readonly QualityControlService _qualityControlService;

    public StateFactory(MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules, QualityControlService qualityControlService)
    {
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
        _qualityControlService = qualityControlService;
    }

    public BusinessProcessStateBase CreateRegistrationState()
    {
        return new RegistrationState(this, _myCustomLoggerService, _businessRules);
    }

    public BusinessProcessStateBase CreateComponentInstallationState()
    {
        return new ComponentInstallationState(this, _myCustomLoggerService, _businessRules);
    }

    public BusinessProcessStateBase CreateQualityControlState()
    {
        return new QualityControlState(this, _myCustomLoggerService, _businessRules);
    }

    public BusinessProcessStateBase CreateRepairState()
    {
        return new RepairState(this, _myCustomLoggerService, _businessRules);
    }

    public BusinessProcessStateBase CreatePackagingState()
    {
        return new PackagingState(_myCustomLoggerService);
    }
}