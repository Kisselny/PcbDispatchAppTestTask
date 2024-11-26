using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Controllers.Dto;

public record BoardInfoDto(int Id, string Name, int ComponentNumber, string CurrentStatus, string QualityControlStatus);

public record BoardInfoDto2(int Id, string Name, int ComponentNumber, List<BoardComponent> BoardComponents, string CurrentStatus, string QualityControlStatus);