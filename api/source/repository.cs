using MapsterMapper;
using Microsoft.EntityFrameworkCore;

public class Repository<TEntity> where TEntity : class
{
    private readonly AppDbContext _db;

    private readonly IMapper _mapper;

    public Repository(AppDbContext db, IMapper mapper)
    {
        _db     = db;
        _mapper = mapper;
    }

    public Task<List<TEntity>> GetAllAsync()
    {
        return _db.Set<TEntity>().ToListAsync();
    }

    public Task<TEntity?> GetByIdAsync(int id)
    {
        return _db.FindAsync<TEntity>(id).AsTask();
    }

    public TEntity Create(IDto dto)
    {
        var record = _mapper.Map<TEntity>(dto);
        _db.Add(record);

        return record;
    }


    public async Task<Repository<TEntity>> StoreAsync(TEntity entity)
    {
        await _db.SaveChangesAsync();

        return this;
    }
}