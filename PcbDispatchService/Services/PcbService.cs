using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Dal;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services;

public class PcbService
{
    private readonly PcbFactory _pcbFactory;
    private readonly IPcbRepository _pcbRepository;
    private readonly IComponentTypesRepository _componentTypesRepository;
    private readonly LoggerService _loggerService;

    public PcbService(PcbFactory pcbFactory, IPcbRepository pcbRepository, IComponentTypesRepository componentTypesRepository, LoggerService loggerService)
    {
        _pcbFactory = pcbFactory;
        _pcbRepository = pcbRepository;
        _componentTypesRepository = componentTypesRepository;
        _loggerService = loggerService;
    }

    public async Task<int> CreateCircuitBoard(string name)
    {
        var brandNewBoard = _pcbFactory.CreateCircuitBoard(name);
        _loggerService.LogThisSh_t($"Фабрика создала новую плату с именем {name}.");
        await _pcbRepository.AddNewBoard(brandNewBoard);
        _loggerService.LogThisSh_t($"Плата добавлена в репозиторий.");
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
    }

    public async Task RemoveAllComponentsFromBoard(int boardId)
    {
        await _pcbRepository.RemoveComponentsFromBoard(boardId);
        _loggerService.LogThisSh_t($"С платы (id = {boardId}) удалены все компоненты.");
    }

    public async Task DeleteBoard(int boardId)
    {
        await _pcbRepository.DeletePcbById(boardId);
        _loggerService.LogThisSh_t($"Плата (id = {boardId}) удалена из системы.");
    }

    public async Task GetCurrentStatus(int boardid)
    {
        
    }
}