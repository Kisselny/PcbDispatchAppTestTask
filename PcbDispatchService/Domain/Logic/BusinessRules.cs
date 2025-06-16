using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic;

/// <inheritdoc />
public class BusinessRules : IBusinessRules
{
    private readonly IQualityControlService _qualityControlService;
    private readonly string notOkMessageStart = "Невозможно продвинуться по бизнес-процессу: ";
    private readonly string notOkMessageRegistration = "Имя платы не должно быть пустым.";
    private readonly string notOkMessageComponents = "К плате необходимо добавить компоненты";
    private readonly string notOkMessageQuality = "Плата не прошла контроль качества. Необходимо отправить на ремонт.";
    private readonly string notOkMessageDefective = "Плата не прошла контроль качества, т.к. признана бракованной.";
    private readonly string notOkMessagePackage = "Плата уже упакована и готова к отправке.";

    
    /// <summary>
    /// Создает новый экземпляр сервиса бизнес-правил.
    /// </summary>
    /// <param name="qualityControlService">Сервис контроля качества.</param>
    public BusinessRules(IQualityControlService qualityControlService)
    {
        _qualityControlService = qualityControlService;
    }

    /// <inheritdoc />
    public BusinessProcessStatusEnum GetNextBusinessStatusForPcb(PrintedCircuitBoard printedCircuitBoard)
    {
        switch (printedCircuitBoard.BusinessProcessStatus)
        {
            case BusinessProcessStatusEnum.Registration:
            {
                /*Вообще, конечно, в конструкторах платы уже завалидировано, что её имя не будет пустым,
                 но какая-то логика должна же быть))*/
                if (printedCircuitBoard.Name != string.Empty)
                {
                    return BusinessProcessStatusEnum.ComponentInstallation;
                }
                else
                    throw new BusinessException(notOkMessageStart + notOkMessageRegistration);
            }
            case BusinessProcessStatusEnum.ComponentInstallation:
            {
                if (printedCircuitBoard.Components.Count != 0)
                    return BusinessProcessStatusEnum.QualityControl;
                else
                    throw new BusinessException(notOkMessageStart + notOkMessageQuality);
            }
            case BusinessProcessStatusEnum.QualityControl:
            {
                var result = _qualityControlService.QualityCheck(printedCircuitBoard);

                if (result == QualityControlStatus.QualityIsOk)
                {
                    printedCircuitBoard.QualityControlStatus = QualityControlStatus.QualityIsOk;
                    return BusinessProcessStatusEnum.Packaging;
                }
                else
                {
                    return BusinessProcessStatusEnum.Repair;
                }
            }
            case BusinessProcessStatusEnum.Repair:
            {
                printedCircuitBoard.QualityControlStatus = _qualityControlService.TryRepair(printedCircuitBoard);
                
                if (printedCircuitBoard.QualityControlStatus == QualityControlStatus.QualityIsOk)
                {
                    return BusinessProcessStatusEnum.QualityControl;
                }
                else
                {
                    printedCircuitBoard.QualityControlStatus = QualityControlStatus.Defective;
                    throw new BusinessException(notOkMessageStart + notOkMessageDefective);
                }
            }
            case BusinessProcessStatusEnum.Packaging:
            {
                throw new BusinessException(notOkMessageStart + notOkMessagePackage);
            }
        }
        throw new InvalidOperationException("Сюда мы не должны дойти.");
    }
}