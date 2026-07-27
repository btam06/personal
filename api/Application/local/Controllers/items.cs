using System;
using System.Linq;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    private readonly IItems _items;
    private readonly IMapper _mapper;


    public ItemController(IItems items, IMapper mapper)
    {
        _items  = items;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _items.GetAllAsync();

        return Ok(items.Select(_mapper.Map<ItemResponseDto>));
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _items.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ItemResponseDto>(item));
    }


    [HttpPost]
    [Authorize(Policy = "ItemsWrite")]
    public async Task<IActionResult> Create(ItemRequestDto dto)
    {
        var item = _items.Create(dto);
        await _items.FlushAsync();

        return Ok(_mapper.Map<ItemResponseDto>(item));
    }


    [HttpPut("{id}")]
    [Authorize(Policy = "ItemsWrite")]
    public async Task<IActionResult> Update(Guid id, ItemRequestDto dto)
    {
        var item = await _items.GetByIdAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _items.Update(item, dto);

        await _items.FlushAsync();

        return Ok(_mapper.Map<ItemResponseDto>(item));
    }


    [HttpDelete("{id}")]
    [Authorize(Policy = "ItemsWrite")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _items.GetByIdAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        _items.Delete(item);

        await _items.FlushAsync();

        return Ok();
    }
}
