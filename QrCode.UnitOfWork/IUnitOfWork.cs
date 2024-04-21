using LimitlessCareDrPortal.Repository;

namespace LimitlessCareDrPortal.UnitOfWork;

public interface IUnitOfWork
{
	Task SaveChanges();

	IGenericRepository<TEntity> Repository<TEntity>()
		where TEntity : class;
}
