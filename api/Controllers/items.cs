using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var items = new List<int> {1, 2, 3};
        
        return Ok(items);
    }
}