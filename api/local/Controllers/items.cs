using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    protected readonly Items _items;

    public ItemController(Items items)
    {
        _items = items;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _items.GetAllAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _items.GetByIdAsync(id);

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemDto dto)
    {
        var item = _items.Create(dto);
        await _items.StoreAsync(item);
        
        return CreatedAtAction(nameof(GetById), new { id = item.Id } , item);
    }
}