using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Services
{
    /// <summary>
    /// Интерфейс для сервиса операций над печатными платами.
    /// </summary>
    public interface IPcbService
    {
        /// <summary>
        /// Создает в системе новую печатную плату.
        /// </summary>
        /// <param name="name">Имя новой платы.</param>
        /// <returns>Идентификатор созданной платы.</returns>
        Task<int> CreateCircuitBoard(string name);

        /// <summary>
        /// Получить информацию о плате по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор платы.</param>
        /// <returns>Печатная плата.</returns>
        Task<PrintedCircuitBoard?> GetCircuitBoardById(int id);

        /// <summary>
        /// Получает список всех печатных плат в системе.
        /// </summary>
        /// <returns>Коллекция печатных плат.</returns>
        Task<List<PrintedCircuitBoard>> GetAllBoards();

        /// <summary>
        /// Добавляет компонент к печатной плате.
        /// </summary>
        /// <param name="boardId">Идентификатор платы.</param>
        /// <param name="componentTypeName">Название типа компонента.</param>
        /// <param name="quantity">Количество типа компонента, добавляемого к плате.</param>
        Task AddComponent(int boardId, string componentTypeName, int quantity);

        /// <summary>
        /// Переименовать существующую печатную плату.
        /// </summary>
        /// <param name="boardId">Идентификатор платы.</param>
        /// <param name="newName">Новое имя для платы.</param>
        Task RenameBoard(int boardId, string newName);

        /// <summary>
        /// Удаляет все компоненты с печатной платы для добавления заново.
        /// </summary>
        /// <param name="boardId">Идентификатор платы.</param>
        Task RemoveAllComponentsFromBoard(int boardId);

        /// <summary>
        /// Удалить  печатную плату из системы.
        /// </summary>
        /// <param name="boardId">Идентификатор удаляемой платы.</param>
        Task DeleteBoard(int boardId);

        /// <summary>
        /// Перевести плату на следующий шаг бизнес-процесса.
        /// </summary>
        /// <param name="boardId">Идентификатор платы.</param>
        /// <returns>Сообщение с результатом операции.</returns>
        Task AdvanceToNextStatus(int boardId);

        /// <summary>
        /// Форматирует выходное сообщение о плате.
        /// </summary>
        /// <param name="pcb">Объект печатной платы.</param>
        /// <returns>Отформатированный DTO.</returns>
        BoardInfoDto FormatBoardDto(PrintedCircuitBoard pcb);
    }
}
