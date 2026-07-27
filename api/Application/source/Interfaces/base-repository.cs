using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IBaseRepository<TEntity> where TEntity : class
{
    public Task<List<TEntity>> GetAllAsync();

    public Task<TEntity?> GetByIdAsync(Guid id);

    public TEntity Create(IDto dto);

    public BaseRepository<TEntity> Update(TEntity entity, IDto dto);

    public BaseRepository<TEntity> Delete(TEntity entity);

    public Task<BaseRepository<TEntity>> FlushAsync();
}
