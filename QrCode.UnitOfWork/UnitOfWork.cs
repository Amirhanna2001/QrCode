using System.Collections;
using LimitlessCareDrPortal.Repository;
using QrCode.DB;

namespace LimitlessCareDrPortal.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
	private readonly DatabaseContext context;
	private Hashtable repositories;

	public UnitOfWork(DatabaseContext context) => this.context = context;

	public IGenericRepository<TEntity> Repository<TEntity>()
		where TEntity : class
	{
		repositories ??= new Hashtable();

		var type = typeof(TEntity).Name;

		if (!repositories.ContainsKey(type))
		{
			var repositoryType = typeof(GenericRepository<>);

			var repositoryInstance =
				Activator.CreateInstance(
					repositoryType
					.MakeGenericType(typeof(TEntity)), context);

			repositories.Add(type, repositoryInstance);
		}

		return repositories[type] as IGenericRepository<TEntity>;
	}

	public async Task SaveChanges()
	{
		_ = await context.SaveChangesAsync();
	}
}
