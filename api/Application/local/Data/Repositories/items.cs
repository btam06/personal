using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class Items : BaseRepository<Item>, IItems
{
    public Items(AppDbContext db, IMapper mapper) : base(db, mapper)
    {
    }
}