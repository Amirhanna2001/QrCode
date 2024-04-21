using Microsoft.EntityFrameworkCore;

namespace LimitlessCareDrPortal.Repository;
public static class SpecificationEvaluator<TEntity>
	where TEntity : class
{
	public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
	{
		var query = inputQuery.AsQueryable().AsNoTracking();

		// modify the IQueryable using the specification's criteria expression
		if (specification.Criteria != null)
		{
			query = query.Where(specification.Criteria);
		}

		// Includes all expression-based includes
		query = specification.Includes.Aggregate(
			query, (current, include) => current.Include(include));

		// Apply ordering if expressions are set
		if (specification.OrderBy != null)
		{
			query = query.OrderBy(specification.OrderBy);
		}
		else if (specification.OrderByDescending != null)
		{
			query = query.OrderByDescending(specification.OrderByDescending);
		}

		// Apply grouping if expressions are set
		if (specification.GroupBy != null)
		{
			query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
		}

		// Apply paging if enabled
		if (specification.IsPagingEnabled)
		{
			query = query.Skip(specification.Skip)
						 .Take(specification.Take);
		}

		return query;
	}

	public static async Task<PaginationResponse<TEntity>> GetPaginatedQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
	{
		var query = inputQuery.AsQueryable().AsNoTracking();

		// modify the IQueryable using the specification's criteria expression
		if (specification.Criteria != null)
		{
			query = query.Where(specification.Criteria);
		}

		var totalCount = await query.CountAsync();

		// Includes all expression-based includes
		query = specification.Includes.Aggregate(
			query, (current, include) => current.Include(include));

		// Apply ordering if expressions are set
		if (specification.OrderBy != null)
		{
			query = query.OrderBy(specification.OrderBy);
		}
		else if (specification.OrderByDescending != null)
		{
			query = query.OrderByDescending(specification.OrderByDescending);
		}

		// Apply grouping if expressions are set
		if (specification.GroupBy != null)
		{
			query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
		}

		// Apply paging if enabled
		if (specification.IsPagingEnabled)
		{
			query = query.Skip(specification.Skip)
						 .Take(specification.Take);
		}

		var items = await query.ToListAsync();
		return new PaginationResponse<TEntity>(items, new PaginationModel()
		{
			CurrentPage = (specification.Skip / specification.Take) + 1,
			TotalPages = (int)Math.Ceiling((double)totalCount / specification.Take),
			TotalItems = totalCount,
		});
	}
}
