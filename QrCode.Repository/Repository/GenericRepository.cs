using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QrCode.DB;

namespace LimitlessCareDrPortal.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity>
	where TEntity : class
{
	protected readonly DatabaseContext context;
	private readonly DbSet<TEntity> dbSet;

	public GenericRepository(DatabaseContext context)
	{
		this.context = context;
		this.dbSet = this.context.Set<TEntity>();
	}

	#region Specification Pattern
	public async Task Add(TEntity entity, CancellationToken cancellationToken = default)
	{
		_ = await dbSet.AddAsync(entity, cancellationToken);
	}

	public async Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
	{
		await dbSet.AddRangeAsync(entities, cancellationToken);
	}

	public async Task<bool> Contains(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
	{
		return await Count(specification, cancellationToken) > 0;
	}

	public async Task<bool> Contains(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await Count(predicate, cancellationToken) > 0;
	}

	public async Task<int> Count(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
	{
		return await ApplySpecification(specification).CountAsync(cancellationToken);
	}

	public Task<int> Count(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return dbSet.Where(predicate).CountAsync(cancellationToken);
	}

	public async Task<IEnumerable<TEntity>> Find(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
	{
		return await ApplySpecification(specification).ToListAsync(cancellationToken);
	}

	public async Task<TEntity> FindById(int id, CancellationToken cancellationToken = default)
	{
		return await dbSet.FindAsync(id, cancellationToken);
	}

	public async Task<PaginationResponse<TEntity>> FindPagination(ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
	{
		return specification is null ? new PaginationResponse<TEntity>(new Collection<TEntity>())
			: await SpecificationEvaluator<TEntity>.GetPaginatedQuery(dbSet, specification);
	}

	public void Remove(TEntity entity)
	{
		_ = dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<TEntity> entities)
	{
		dbSet.RemoveRange(entities);
	}

	public void Update(TEntity entity)
	{
		_ = dbSet.Update(entity);
	}

	private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity>? spec)
	{
		return spec is null ? dbSet.AsQueryable() : SpecificationEvaluator<TEntity>.GetQuery(dbSet, spec);
	}
	#endregion
}
