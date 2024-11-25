using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PcbDispatchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PcbController : Controller
{
    private readonly PcbService _pcbService;

    public PcbController(PcbService pcbService)
    {
        _pcbService = pcbService;
    }
    
    /// <summary>
    /// Создает и сохраняет в БД новую печатную плату в статусе регистрации.
    /// </summary>
    /// <param name="name">Имя платы.</param>
    /// <returns>Адрес с информацией о плате.</returns>
    /// <response code="201">Плата добавлена в систему.</response>
    [HttpPost]
    public IActionResult CreateCircuitBoard(string name)
    {
        var idOfNewBoard = _pcbService.CreateCircuitBoard(name);
        return CreatedAtAction(nameof(GetCircuitBoardInfo), new { id = idOfNewBoard });
    }
    
    /// <summary>
    /// Получает информацию о плате по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>DTO с описанием платы.</returns>
    /// /// <response code="200">Плата найдена.</response>
    /// /// <response code="404">Плата по указанному идентификатору не найдена.</response>
    [HttpGet("{id}")]
    public ActionResult<BoardInfoDto> GetCircuitBoardInfo(int id)
    {
        var pcb = _pcbService.GetCircuitBoardById(id);
        if (pcb == null)
        {
            return NotFound();
        }

        BoardInfoDto result = new BoardInfoDto(Id: pcb.Result.Id, Name: pcb.Result.Name,
            ComponentNumber: pcb.Result.Components.Count, CurrentStatus: pcb.Result.GetBusinessState().ToString(),
            QualityControlStatus: pcb.Result.QualityControlStatus.ToString());
        return Ok(result);
    }

    /// <summary>
    /// Добавляет новый компонент к существующей печатной плате.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="dto">DTO компонента.</param>
    /// <returns>Результат добавления компонента.</returns>
    /// /// <remarks>Добавлять компоненты можно только на этапе добавления компонентов.</remarks>
    [HttpPut("{boardId}/add-single-component")]
    public IActionResult AddComponentToCircuitBoard(int boardId, [FromBody] BoardComponentDto dto)
    {
        var pcb = _pcbService.GetCircuitBoardById(boardId);
        if(pcb.Result != null)
        {
            _pcbService.AddComponent(pcb.Id, dto.ComponentTypeName, dto.Quantity);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Добавляет набор компонентов к существующей печатной плате.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="dtos">Список компонентов.</param>
    /// <returns>Результат добавления компонентов.</returns>
    /// <remarks>Добавлять компоненты можно только на этапе добавления компонентов.</remarks>
    [HttpPut("{boardId}/add-many-components")]
    public IActionResult AddComponentsToCircuitBoard(int boardId, [FromBody] List<BoardComponentDto> dtos)
    {
        _pcbService.AddComponents(boardId, dtos);
        return NoContent();
    }

    /// <summary>
    /// Выполняет переименование существующей печатной платы.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="newName">Новое имя.</param>
    /// <returns>Результат переименования.</returns>
    /// <remarks>Переименовать плату можно только на этапе регистрации.</remarks>
    [HttpPut("{boardId}/rename/")]
    public IActionResult RenameBoard(int boardId, [FromBody] string newName)
    {
        _pcbService.RenameBoard(boardId, newName);
        return NoContent();
    }
    
    /// <summary>
    /// Удаляет все добавленные на плату компоненты.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <returns>Результат удаления компонентов с платы.</returns>
    /// <remarks>Удалять компоненты можно только на этапе добавления компонентов</remarks>
    [HttpPut("{boardId}/remove-components")]
    public IActionResult RemoveAllComponentsFromBoard(int boardId)
    {
        _pcbService.RemoveAllComponentsFromBoard(boardId);
        return NoContent();
    }

    /// <summary>
    /// Удаляет существующую плату по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Результат удаления платы.</returns>
    /// <remarks>Удалять плату можно на любом этапе.</remarks>
    /// <response code="200">Плата удалена из системы.</response>
    [HttpDelete("{id}")]
    public IActionResult DeleteBoard(int id)
    {
        _pcbService.DeleteBoard(id);
        return Ok("Плата удалена.");
    }
}