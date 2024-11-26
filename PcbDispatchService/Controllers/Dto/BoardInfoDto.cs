using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Controllers.Dto;

/// <summary>
/// Описание печатной платы.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="Name">Название.</param>
/// <param name="ComponentNumber">Количество компонентов на плате.</param>
/// <param name="BoardComponents">Список компонентов.</param>
/// <param name="CurrentStatus">Статус бизнес-процесса.</param>
/// <param name="QualityControlStatus">Статус контроля качества.</param>
public record BoardInfoDto(int Id, string Name, int ComponentNumber, List<BoardComponent> BoardComponents, string CurrentStatus, string QualityControlStatus);