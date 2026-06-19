using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class Items : BaseRepository<Item>
{
    public Items(AppDbContext db, IMapper mapper) : base(db, mapper)
    {
    }
}