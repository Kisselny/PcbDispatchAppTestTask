using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Dal;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbService
{
    private readonly PcbFactory _pcbFactory;
    private readonly IPcbRepository _pcbRepository;
    private readonly IComponentTypesRepository _componentTypesRepository;
    private readonly MyCustomLoggerService _myCustomLoggerService;

    public PcbService(PcbFactory pcbFactory, IPcbRepository pcbRepository, IComponentTypesRepository componentTypesRepository, MyCustomLoggerService myCustomLoggerService)
    {
        _pcbFactory = pcbFactory;
        _pcbRepository = pcbRepository;
        _componentTypesRepository = componentTypesRepository;
        _myCustomLoggerService = myCustomLoggerService;
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

    public async Task AddComponent(int boardId, string componentTypeName, int quantity)
    {
        var board = await GetCircuitBoardById(boardId);
        var componentType = await _componentTypesRepository.GetComponentTypeByName(componentTypeName);
        if (board is not null && componentType is not null)
        {
            _pcbFactory.AddComponentToBoard(board, componentType, quantity);
        }
    }
    
    public async Task AddComponents(int boardId, List<BoardComponentDto> componentDtoList)
    {
        var board = await GetCircuitBoardById(boardId);
        var componentTypes = await _componentTypesRepository.GetComponentTypesByNames(componentDtoList.Select(i => i.ComponentTypeName).ToList());
        if(componentTypes.Count == 0)
        {
            throw new ApplicationException("Компоненты не найдены.");
        }
        List<BoardComponent> componentModelList = componentDtoList
            .Select(i => new BoardComponent(componentTypes.First(t => i.ComponentTypeName == t.Name), i.Quantity)).ToList();
        if (board is not null)
        {
            _pcbFactory.AddComponentsToBoard(board, componentModelList, componentTypes);
        }
    }

    public async Task RenameBoard(int boardId, string newName)
    {
        await _pcbRepository.RenameBoard(boardId, newName);
        _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) переименована.");
    }

    public async Task RemoveAllComponentsFromBoard(int boardId)
    {
        await _pcbRepository.RemoveComponentsFromBoard(boardId);
        _myCustomLoggerService.LogThisSh_t($"С платы (id = {boardId}) удалены все компоненты.");
    }

    public async Task DeleteBoard(int boardId)
    {
        await _pcbRepository.DeletePcbById(boardId);
        _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) удалена из системы.");
    }

    public async Task AdvanceToNextStatus(int boardId)
    {
        await _pcbRepository.UpdateBoardStateById(boardId);
        _myCustomLoggerService.LogThisSh_t($"Плата (id = {boardId}) переведена в новое состояние состояние.");
    }

    public BoardInfoDto FormatBoardDto(PrintedCircuitBoard pcb)
    {
        BoardInfoDto result = new BoardInfoDto(Id: pcb.Id, Name: pcb.Name,
            ComponentNumber: pcb.Components.Count, CurrentStatus: pcb.GetBusinessState().ToString(),
            QualityControlStatus: pcb.QualityControlStatus.ToString());
        return result;
    }
}