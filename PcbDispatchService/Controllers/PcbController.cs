using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using PcbDispatchService.Controllers.Dto;
using PcbDispatchService.Domain.Models;
using PcbDispatchService.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PcbDispatchService.Controllers;

/// <summary>
/// Контроллер печатных плат.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PcbController : Controller
{
    private readonly IPcbService _pcbService;

    /// <summary>
    /// Создает контроллер печатных плат.
    /// </summary>
    /// <param name="pcbService">Сервис печатных плат.</param>
    public PcbController(IPcbService pcbService)
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCircuitBoard(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Name cannot be null or empty.");
        }
        var idOfNewBoard = await _pcbService.CreateCircuitBoard(name);
        //return Ok($"Id новой платы: {idOfNewBoard}");
        return CreatedAtAction(nameof(GetCircuitBoardInfo), new { id = idOfNewBoard });
    }
    
    /// <summary>
    /// Получает информацию о плате по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>DTO с описанием платы.</returns>
    /// <response code="200">Плата найдена.</response>
    /// <response code="404">Плата по указанному идентификатору не найдена.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrintedCircuitBoard>> GetCircuitBoardInfo(int id)
    {
        var pcb = await _pcbService.GetCircuitBoardById(id);
        if (pcb is null)
        {
            return NotFound("Плата по указанному идентификатору не найдена.");
        }
        var result = _pcbService.FormatBoardDto(pcb);
        return Ok(result);
    }

    /// <summary>
    /// Возвращает информацию обо всех платах, зарегистрированных в системе.
    /// </summary>
    /// <returns>Коллекция DTO печатных плат.</returns>
    /// <remarks>Пагинация само собой нужна.</remarks>
    /// <response code="200">Платы найдены.</response>
    /// <response code="404">Платы не найдены.</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<BoardInfoDto>>> GetAllBoards()
    {
        var boards = await _pcbService.GetAllBoards();
        if (boards.Count == 0)
        {
            return NotFound();
        }
        var result = new List<BoardInfoDto>(boards.Count);
        foreach (var board in boards)
        {
            var entry = _pcbService.FormatBoardDto(board);
            result.Add(entry);
        }

        return Ok(result);
    }

    /// <summary>
    /// Добавляет новый компонент к существующей печатной плате.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <param name="dto">DTO компонента.</param>
    /// <returns>Результат добавления компонента.</returns>
    /// <remarks>Добавлять компоненты можно только на этапе добавления компонентов.</remarks>
    /// <response code="204">Плата удалена из системы.</response>
    [HttpPut("{id}/add-single-component")]
    public async Task<IActionResult> AddComponentToCircuitBoard(int id, [FromBody] BoardComponentDto dto)
    {
        if (dto == null)
            return BadRequest("Component data is required.");

        if (string.IsNullOrWhiteSpace(dto.ComponentTypeName))
            return BadRequest("ComponentTypeName is required.");

        if (dto.Quantity <= 0)
            return BadRequest("Quantity must be greater than zero.");

        var pcb = await _pcbService.GetCircuitBoardById(id);
        if (pcb == null)
            return NotFound("Плата по указанному идентификатору не найдена.");

        await _pcbService.AddComponent(id, dto.ComponentTypeName, dto.Quantity);
        return NoContent();
    }

    /// <summary>
    /// Выполняет переименование существующей печатной платы.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <param name="newName">Новое имя.</param>
    /// <returns>Результат переименования.</returns>
    /// <remarks>Переименовать плату можно только на этапе регистрации.</remarks>
    /// <response code="204">Плата переименована.</response>
    [HttpPut("{id}/rename/")]
    public async Task<IActionResult> RenameBoard(int id, [FromBody] string newName)
    {
        await _pcbService.RenameBoard(id, newName);
        return NoContent();
    }
    
    /// <summary>
    /// Удаляет все добавленные на плату компоненты.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Результат удаления компонентов с платы.</returns>
    /// <remarks>Удалять компоненты можно только на этапе добавления компонентов</remarks>
    /// <response code="204">Плата очищена от компонентов.</response>
    [HttpPut("{id}/remove-components")]
    public async Task<IActionResult> RemoveAllComponentsFromBoard(int id)
    {
        await _pcbService.RemoveAllComponentsFromBoard(id);
        return NoContent();
    }

    /// <summary>
    /// Удаляет существующую плату по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Результат удаления платы.</returns>
    /// <remarks>Удалять плату можно на любом этапе.</remarks>
    /// <response code="204">Плата удалена из системы.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoard(int id)
    {
        await _pcbService.DeleteBoard(id);
        return NoContent();
    }

    /// <summary>
    /// Переводит плату в следующее состояние процесса.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Результат перевода платы в новое состояние.</returns>
    /// <response code="204">Плата переведена на новый этап или находится на последнем этапе.</response>
    [HttpPut("{id}/next-stage/")]
    public async Task<IActionResult> MoveToNextBusinessState(int id)
    {
        await _pcbService.AdvanceToNextStatus(id);
        return NoContent();
    }
}