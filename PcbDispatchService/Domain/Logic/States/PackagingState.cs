using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class PackagingState : BusinessProcessStateBase
{
    private bool PackagedAndReleased = false;
    private readonly MyCustomLoggerService _myCustomLoggerService;

    public PackagingState(MyCustomLoggerService myCustomLoggerService)
    {
        _myCustomLoggerService = myCustomLoggerService;
    }

    public PackagingState()
    {
    }


    //TODO можно еще какую-то логику внедрить.
    public override void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        if (PackagedAndReleased)
        {
            _myCustomLoggerService.LogThisSh_t("Данная плата уже была отгружена. Повторная отгрузка невозможна.");
        }
        else
        {
            _myCustomLoggerService.LogThisSh_t("Плата успешно произведена и отгружена.");
            PackagedAndReleased = true;
        }
    }

    public override BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Packaging;
    }
    public string GetCurrentStatusString()
    {
        return GetCurrentStatus().ToString();
    }
}