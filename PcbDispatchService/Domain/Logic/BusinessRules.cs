using System.Runtime.Intrinsics.X86;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;

namespace PcbDispatchService.Domain.Logic;

public class BusinessRules : IBusinessRules
{
    private readonly QualityControlService _qualityControlService;
    public readonly string okMessage = "Ok";
    private readonly string notOkMessageStart = "Невозможно продвинуться по бизнес-процессу: ";
    private readonly string notOkMessageRegistration = "Имя платы не должно быть пустым.";
    private readonly string notOkMessageComponents = "К плате необходимо добавить компоненты";
    private readonly string notOkMessageQuality = "Плата не прошла контроль качества. Необходимо отправить на ремонт.";
    private readonly string notOkMessageDefective = "Плата не прошла контроль качества, т.к. признана бракованной.";

    
    public BusinessRules(QualityControlService qualityControlService)
    {
        _qualityControlService = qualityControlService;
    }

    /// <summary>
    /// Проверяет, возможно ли перевести плату в следующее состояние безнес-процесса.
    /// </summary>
    /// <param name="printedCircuitBoard">Экземпляр платы.</param>
    /// <returns>Сообщение-статус проверки бизнес-правил.</returns>
    /// <exception cref="InvalidOperationException">Невозможно осуществить проверку.</exception>
    public string CheckIfContinuationIsPossible(PrintedCircuitBoard printedCircuitBoard)
    {
        switch (printedCircuitBoard.GetBusinessState())
        {
            case BusinessProcessStatusEnum.Registration:
            {
                /*Вообще, конечно, в конструкторах платы уже завалидировано, что её имя не будет пустым,
                 но какая-то логика должна же быть))*/
                if (printedCircuitBoard.Name != string.Empty)
                {
                    return okMessage;
                }
                else
                    return notOkMessageStart + notOkMessageRegistration;
            }
            case BusinessProcessStatusEnum.ComponentInstallation:
            {
                return printedCircuitBoard.Components.Count != 0 ? okMessage : notOkMessageStart + notOkMessageComponents;
            }
            case BusinessProcessStatusEnum.QualityControl:
            {
                var result = _qualityControlService.QualityCheck(printedCircuitBoard);

                if (result == QualityControlStatus.QualityIsOk)
                {
                    return okMessage;
                }
                else
                {
                    return notOkMessageStart + notOkMessageQuality;
                }
            }
            case BusinessProcessStatusEnum.Repair:
            {
                printedCircuitBoard.QualityControlStatus = _qualityControlService.TryRepair(printedCircuitBoard);
                
                if (printedCircuitBoard.QualityControlStatus == QualityControlStatus.QualityIsOk)
                {
                    return okMessage;
                }
                else
                {
                    printedCircuitBoard.QualityControlStatus = QualityControlStatus.Defective;
                    return notOkMessageStart + notOkMessageDefective;
                }
            }
            case BusinessProcessStatusEnum.Packaging:
            {
                break;
            }
        }
        throw new InvalidOperationException();
    }
}