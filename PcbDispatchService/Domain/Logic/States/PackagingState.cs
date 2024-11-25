using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic.States;

public class PackagingState : IBusinessProcessState
{
    private readonly LoggerService _loggerService;

    public PackagingState(LoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    private bool PackagedAndReleased = false;
    
    //TODO можно еще какую-то логику внедрить.
    public void AdvanceToNextState(PrintedCircuitBoard printedCircuitBoard)
    {
        if (PackagedAndReleased)
        {
            _loggerService.LogThisSh_t("Данная плата уже была отгружена. Повторная отгрузка невозможна.");
        }
        else
        {
            _loggerService.LogThisSh_t("Плата успешно произведена и отгружена.");
            PackagedAndReleased = true;
        }
    }

    public BusinessProcessStatusEnum GetCurrentStatus()
    {
        return BusinessProcessStatusEnum.Packaging;
    }
    public string GetCurrentStatusString()
    {
        return GetCurrentStatus().ToString();
    }
}