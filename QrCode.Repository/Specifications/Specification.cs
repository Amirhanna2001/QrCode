using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace LimitlessCareDrPortal.Repository;
public abstract class Specification<TEntity> : ISpecification<TEntity>
	where TEntity : class
{
	protected Specification(Expression<Func<TEntity, bool>> criteria)
	{
		Criteria = criteria;
	}

	protected Specification()
	{
	}

	public Expression<Func<TEntity, bool>> Criteria { get; }

	public Collection<Expression<Func<TEntity, object>>> Includes { get; } = new Collection<Expression<Func<TEntity, object>>>();

	public Expression<Func<TEntity, object>> OrderBy { get; private set; }

	public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

	public Expression<Func<TEntity, object>> GroupBy { get; private set; }

	public int Take { get; private set; }

	public int Skip { get; private set; }

	public bool IsPagingEnabled { get; private set; }

	protected virtual void AddInclude(Expression<Func<TEntity, object>> includeExpression)
	{
		Includes.Add(includeExpression);
	}

	protected virtual void ApplyPaging(int skip, int take)
	{
		Skip = skip;
		Take = take;
		IsPagingEnabled = true;
	}

	protected virtual void ApplyOrderBy(Expression<Func<TEntity, object>> orderByExpression)
	{
		OrderBy = orderByExpression;
	}

	protected virtual void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
	{
		OrderByDescending = orderByDescendingExpression;
	}

	protected virtual void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
	{
		GroupBy = groupByExpression;
	}
}
