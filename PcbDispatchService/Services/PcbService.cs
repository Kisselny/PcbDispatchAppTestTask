using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Dal;
using PcbDispatchService.Domain.Logic;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

/// <summary>
/// Сервис операций над печатными платами.
/// </summary>
public class PcbService : IPcbService
{
    private readonly IPcbFactory _pcbFactory;
    private readonly IPcbRepository _pcbRepository;
    private readonly IComponentTypesRepository _componentTypesRepository;
    private readonly IMyCustomLoggerService _myCustomLoggerService;
    private readonly IBusinessRules _businessRules;

    /// <summary>
    /// Создает экземпляр сервиса <see cref="PcbService"/>
    /// </summary>
    /// <param name="pcbFactory">Фабрика печатных плат.</param>
    /// <param name="pcbRepository">Репозиторий печатных плат.</param>
    /// <param name="componentTypesRepository">Репозиторий компонентов плат.</param>
    /// <param name="myCustomLoggerService">Сервис логирования.</param>
    /// <param name="businessRules">Сервис бизнес-правил.</param>
    public PcbService(IPcbFactory pcbFactory, IPcbRepository pcbRepository, IComponentTypesRepository componentTypesRepository, IMyCustomLoggerService myCustomLoggerService, IBusinessRules businessRules)
    {
        _pcbFactory = pcbFactory;
        _pcbRepository = pcbRepository;
        _componentTypesRepository = componentTypesRepository;
        _myCustomLoggerService = myCustomLoggerService;
        _businessRules = businessRules;
    }

    /// <summary>
    /// Создает в системе новую печатную плату.
    /// </summary>
    /// <param name="name">Имя новой платы.</param>
    /// <returns>Идентификатор созданной платы.</returns>
    public async Task<int> CreateCircuitBoard(string name)
    {
        var brandNewBoard = _pcbFactory.CreateCircuitBoard(name);
        _myCustomLoggerService.LogThisSh_t($"Фабрика создала новую плату с именем {name}.");
        await _pcbRepository.AddNewBoard(brandNewBoard);
        _myCustomLoggerService.LogThisSh_t($"Плата добавлена в репозиторий.");
        return brandNewBoard.Id;
    }

    /// <summary>
    /// Получить информацию о плате по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Печатная плата.</returns>
    /// <exception cref="ArgumentNullException">Плата по указанному идентификатору не найдена.</exception>
    public async Task<PrintedCircuitBoard?> GetCircuitBoardById(int id)
    {
        var result = await _pcbRepository.GetPcbById(id);
        if (result == null)
        {
            throw new ApplicationException(nameof(result));
        }
        return result;
    }

    /// <summary>
    /// Получает список всех печатных плат в системе.
    /// </summary>
    /// <returns>Коллекция печатных плат.</returns>
    /// <remarks>Пагинация нужна и все дела, но чето времени мало оставалось. Пусть она тут как будто есть.</remarks>
    public async Task<List<PrintedCircuitBoard>> GetAllBoards()
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

    /// <summary>
    /// Переименовать существующую печатную плату.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="newName">Новое имя для платы.</param>
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

    /// <summary>
    /// Удалить  печатную плату из системы.
    /// </summary>
    /// <param name="boardId">Идентификатор удаляемой платы.</param>
    public async Task DeleteBoard(int boardId)
    {
            await _pcbRepository.DeletePcbById(boardId);
            _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) удалена из системы.");
    }

    /// <summary>
    /// Перевести плату на следующий шаг бизнес-процесса.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <returns>Сообщение с результатом операции.</returns>
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

    /// <summary>
    /// Форматирует выходное сообщение о плате.
    /// </summary>
    /// <param name="pcb">Объект печатной платы.</param>
    /// <returns>Отформатированный DTO.</returns>
    public BoardInfoDto FormatBoardDto(PrintedCircuitBoard pcb)
    {
        BoardInfoDto result2 = new BoardInfoDto(Id: pcb.Id, Name: pcb.Name,
            ComponentNumber: pcb.Components.Count, pcb.Components, CurrentStatus: pcb.BusinessProcessStatus.ToString(),
            QualityControlStatus: pcb.QualityControlStatus.ToString());
        return result2;
    }
}