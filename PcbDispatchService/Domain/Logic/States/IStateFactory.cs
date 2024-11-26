namespace PcbDispatchService.Domain.Logic.States;

public interface IStateFactory
{
    BusinessProcessStateBase CreateRegistrationState();
    BusinessProcessStateBase CreateComponentInstallationState();
    BusinessProcessStateBase CreateQualityControlState();
    BusinessProcessStateBase CreateRepairState();
    BusinessProcessStateBase CreatePackagingState();
}