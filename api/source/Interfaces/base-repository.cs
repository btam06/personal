public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task<List<TEntity>> GetAllAsync();
    
    public Task<TEntity?> GetByIdAsync(int id);
    
    public TEntity Create(IDto dto);
    
    public Task<BaseRepository<TEntity>> StoreAsync(TEntity entity);
}