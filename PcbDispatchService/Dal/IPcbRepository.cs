using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

/// <summary>
/// Репозиторий печатных плат.
/// </summary>
public interface IPcbRepository
{
    /// <summary>
    /// Получить плату по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Печатная плата.</returns>
    Task<PrintedCircuitBoard?> GetPcbById(int id);
    
    /// <summary>
    /// Получить все печатные платы в системе.
    /// </summary>
    /// <returns>Коллекция печатных плат.</returns>
    /// <remarks>Про пагинацию помню, времени не хватило.</remarks>
    Task<List<PrintedCircuitBoard>> GetAllPcbs();
    
    /// <summary>
    /// Добавить новую печатную плату.
    /// </summary>
    /// <param name="pcb">Печатная плата.</param>
    /// <returns></returns>
    Task AddNewBoard(PrintedCircuitBoard pcb);
    
    /// <summary>
    /// Удалить печатную плату.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns></returns>
    Task<bool> DeletePcbById(int id);
    
    /// <summary>
    /// Переименовать плату.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <param name="newName">Новое имя.</param>
    /// <returns></returns>
    Task RenameBoard(int id, string newName);
    
    /// <summary>
    /// Обновить статус бизне-процесса платы.
    /// </summary>
    /// <param name="newStatePcb">Плата с новым статусом.</param>
    /// <returns></returns>
    Task UpdateBoardState(PrintedCircuitBoard newStatePcb);
    
    /// <summary>
    /// Добавить новый компонент на плату.
    /// </summary>
    /// <param name="boardId">Идентификатор.</param>
    /// <param name="boardComponent">Экземпляр компонента.</param>
    /// <returns></returns>
    Task AddComponentToBoard(int boardId, BoardComponent boardComponent);
}