using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class Items : Repository<Item>
{
    public Items(AppDbContext db, IMapper mapper) : base(db, mapper)
    {
    }
}