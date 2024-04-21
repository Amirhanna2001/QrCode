using System.Linq.Expressions;

namespace LimitlessCareDrPortal.Repository;

public interface IGenericRepository<TEntity>
	where TEntity : class
{
	#region Specification Pattern
	Task<TEntity> FindById(int id, CancellationToken cancellationToken = default);

	Task<IEnumerable<TEntity>> Find(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default);

	Task<PaginationResponse<TEntity>> FindPagination(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default);

	Task Add(TEntity entity, CancellationToken cancellationToken = default);

	Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

	void Remove(TEntity entity);

	void RemoveRange(IEnumerable<TEntity> entities);

	void Update(TEntity entity);

	Task<bool> Contains(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default);

	Task<bool> Contains(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

	Task<int> Count(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default);

	Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
	#endregion
}
