using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Dal;
using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbService
{
    private readonly PcbFactory _pcbFactory;
    private readonly IPcbRepository _pcbRepository;
    private readonly IComponentTypesRepository _componentTypesRepository;
    private readonly MyCustomLoggerService _myCustomLoggerService;
    private readonly BusinessRules _businessRules;

    public PcbService(PcbFactory pcbFactory, IPcbRepository pcbRepository, IComponentTypesRepository componentTypesRepository, MyCustomLoggerService myCustomLoggerService, BusinessRules businessRules)
    {
        _pcbFactory = pcbFactory;
        _pcbRepository = pcbRepository;
        _componentTypesRepository = componentTypesRepository;
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
    }

    public async Task<int> CreateCircuitBoard(string name)
    {
        var brandNewBoard = _pcbFactory.CreateCircuitBoard(name);
        _myCustomLoggerService.LogThisSh_t($"Фабрика создала новую плату с именем {name}.");
        await _pcbRepository.AddNewBoard(brandNewBoard);
        _myCustomLoggerService.LogThisSh_t($"Плата добавлена в репозиторий.");
        return brandNewBoard.Id;
    }

    public async Task<PrintedCircuitBoard?> GetCircuitBoardById(int id)
    {
        var result = await _pcbRepository.GetPcbById(id);
        if (result == null)
        {
            throw new ArgumentNullException(nameof(result));
        }
        return result;
    }

    public async Task<List<PrintedCircuitBoard>> GetALlBoards()
    {
        var result = await _pcbRepository.GetAllPcbs();
        return result;
    }

    /// <summary>
    /// Добавляет компонент к печатной плате.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="componentTypeName">Название типа компонента.</param>
    /// <param name="quantity">Количество типа компонента, добавляемого к плате.</param>
    /// <remarks>Соответственно, уменьшает количество компонентов данного типа на складе.</remarks>
    public async Task AddComponent(int boardId, string componentTypeName, int quantity)
    {
        var componentType = await _componentTypesRepository.GetComponentTypeByName(componentTypeName);

        if (componentType.AvailableSupply >= quantity)
        {
            BoardComponent newBc = new BoardComponent(componentType, quantity);
            await _pcbRepository.AddComponentToBoard(boardId, newBc);
            await _componentTypesRepository.DecreaseComponentSupplyByValue(componentTypeName, quantity);
        }
    }

    public async Task RenameBoard(int boardId, string newName)
    {
        await _pcbRepository.RenameBoard(boardId, newName);
        _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) переименована.");
    }

    /// <summary>
    /// Удаляет все компоненты с печатной платы для добавления заново.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <remarks>Возвращает удаленные компоненты на склад.</remarks>
    public async Task RemoveAllComponentsFromBoard(int boardId)
    {
        var boardToRemoveComponentsFrom = await _pcbRepository.GetPcbById(boardId);
        if(boardToRemoveComponentsFrom is not null)
        {
            var componentsToReturnToStorage = boardToRemoveComponentsFrom.Components;
            await _componentTypesRepository.IncreaseComponentSupplyByValue(componentsToReturnToStorage);
            await _pcbRepository.RemoveComponentsFromBoard(boardToRemoveComponentsFrom.Id);
            _myCustomLoggerService.LogThisSh_t($"С платы (id = {boardId}) удалены все компоненты. Компоненты вернулись на склад.");
        }
    }

    public async Task DeleteBoard(int boardId)
    {
            await _pcbRepository.DeletePcbById(boardId);
            _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) удалена из системы.");
    }

    public async Task<string> AdvanceToNextStatus(int boardId)
    {
        var pcb = await _pcbRepository.GetPcbById(boardId);
        var result = _businessRules.CheckIfContinuationIsPossible(pcb);
        if (pcb.BusinessProcessStatus != result)
        {
            pcb.SetBusinessEnum(result);
            await _pcbRepository.UpdateBoardState(pcb);
            _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) переведена в новое состояние состояние: {pcb.BusinessProcessStatus}");
            return $"Плата (id = {boardId}) переведена в новое состояние состояние: {pcb.BusinessProcessStatus}.";
        }
        _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) прошла весь процесс.");
        return $"Плата (id = {boardId}) уже прошла весь процесс.";
    }

    public BoardInfoDto2 FormatBoardDto(PrintedCircuitBoard pcb)
    {
        BoardInfoDto result = new BoardInfoDto(Id: pcb.Id, Name: pcb.Name,
            ComponentNumber: pcb.Components.Count, CurrentStatus: pcb.BusinessProcessStatus.ToString(),
            QualityControlStatus: pcb.QualityControlStatus.ToString());
        
        BoardInfoDto2 result2 = new BoardInfoDto2(Id: pcb.Id, Name: pcb.Name,
            ComponentNumber: pcb.Components.Count, pcb.Components, CurrentStatus: pcb.BusinessProcessStatus.ToString(),
            QualityControlStatus: pcb.QualityControlStatus.ToString());
        return result2;
    }
}