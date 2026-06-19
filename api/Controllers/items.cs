using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    protected readonly AppDbContext _db;

    public ItemController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _db.Items.ToListAsync();

        return Ok(items);
    }
}