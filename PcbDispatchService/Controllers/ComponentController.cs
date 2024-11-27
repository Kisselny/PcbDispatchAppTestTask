using Microsoft.AspNetCore.Mvc;
using PcbDispatchService.Dal;

namespace PcbDispatchService.Controllers;

/// <summary>
/// Контроллер компонентов плат.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ComponentController : Controller
{
    private readonly IComponentTypesRepository _componentTypesRepository;

    /// <summary>
    /// Создает контроллер.
    /// </summary>
    /// <param name="componentTypesRepository">Репозиторий компонентов.</param>
    /// <remarks>Не успел в отдельный сервис вынести.</remarks>
    public ComponentController(IComponentTypesRepository componentTypesRepository)
    {
        _componentTypesRepository = componentTypesRepository;
    }

    /// <summary>
    /// Получить все имеющиеся на складе компоненты.
    /// </summary>
    /// <returns>Коллекция компонентов.</returns>
    /// <response code="200">Компоненты найдены.</response>
    [HttpGet]
    public async Task<IActionResult> GetAllTypes()
    {
        var result = await _componentTypesRepository.GetAllComponents();
        return Ok(result);
    }
}