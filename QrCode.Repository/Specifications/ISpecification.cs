using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace LimitlessCareDrPortal.Repository;
public interface ISpecification<T>
{
	Expression<Func<T, bool>> Criteria { get; }

	Collection<Expression<Func<T, object>>> Includes { get; }

	Expression<Func<T, object>> OrderBy { get; }

	Expression<Func<T, object>> OrderByDescending { get; }

	Expression<Func<T, object>> GroupBy { get; }

	int Take { get; }

	int Skip { get; }

	bool IsPagingEnabled { get; }
}
