using Microsoft.AspNetCore.Mvc;
using PcbDispatchService.Dal;

namespace PcbDispatchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComponentController : Controller
{
    private readonly IComponentTypesRepository _componentTypesRepository;

    public ComponentController(IComponentTypesRepository componentTypesRepository)
    {
        _componentTypesRepository = componentTypesRepository;
    }

    /// <summary>
    /// Получить все имеющиеся на складе компоненты.
    /// </summary>
    /// <returns>Коллекция компонентов.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllTypes()
    {
        var result = await _componentTypesRepository.GetAllComponents();
        return Ok(result);
    }
}