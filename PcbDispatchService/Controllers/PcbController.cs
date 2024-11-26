﻿using System.ComponentModel;
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
    private readonly PcbService _pcbService;

    /// <summary>
    /// Создает контроллер печатных плат.
    /// </summary>
    /// <param name="pcbService">Сервис печатных плат.</param>
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
    public async Task<IActionResult> CreateCircuitBoard(string name)
    {
        var idOfNewBoard = await _pcbService.CreateCircuitBoard(name);
        //return Ok($"Id новой платы: {idOfNewBoard}");
        return Ok(CreatedAtAction(nameof(GetCircuitBoardInfo), new { id = idOfNewBoard }));
    }
    
    /// <summary>
    /// Получает информацию о плате по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>DTO с описанием платы.</returns>
    /// <response code="200">Плата найдена.</response>
    /// <response code="404">Плата по указанному идентификатору не найдена.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<PrintedCircuitBoard>> GetCircuitBoardInfo(int id)
    {
        var pcb = await _pcbService.GetCircuitBoardById(id);
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
    /// <param name="boardId">Идентификатор платы.</param>
    /// <param name="dto">DTO компонента.</param>
    /// <returns>Результат добавления компонента.</returns>
    /// /// <remarks>Добавлять компоненты можно только на этапе добавления компонентов.</remarks>
    [HttpPut("{boardId}/add-single-component")]
    public async Task<IActionResult> AddComponentToCircuitBoard(int boardId, [FromBody] BoardComponentDto dto)
    {
        await _pcbService.AddComponent(boardId, dto.ComponentTypeName, dto.Quantity);
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
    public async Task<IActionResult> RenameBoard(int boardId, [FromBody] string newName)
    {
        await _pcbService.RenameBoard(boardId, newName);
        return NoContent();
    }
    
    /// <summary>
    /// Удаляет все добавленные на плату компоненты.
    /// </summary>
    /// <param name="boardId">Идентификатор платы.</param>
    /// <returns>Результат удаления компонентов с платы.</returns>
    /// <remarks>Удалять компоненты можно только на этапе добавления компонентов</remarks>
    [HttpPut("{boardId}/remove-components")]
    public async Task<IActionResult> RemoveAllComponentsFromBoard(int boardId)
    {
        await _pcbService.RemoveAllComponentsFromBoard(boardId);
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
    public async Task<IActionResult> DeleteBoard(int id)
    {
        await _pcbService.DeleteBoard(id);
        return Ok("Плата удалена.");
    }

    /// <summary>
    /// Переводит плату в следующее состояние процесса.
    /// </summary>
    /// <param name="id">Идентификатор платы.</param>
    /// <returns>Результат перевода платы в новое состояние.</returns>
    [HttpPut("{boardId}/next-stage/")]
    public async Task<IActionResult> MoveToNextBusinessState(int boardId)
    {
        string result = await _pcbService.AdvanceToNextStatus(boardId);
        return Ok(result);
    }
}