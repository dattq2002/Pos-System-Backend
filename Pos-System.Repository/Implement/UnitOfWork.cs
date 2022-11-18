using Microsoft.EntityFrameworkCore;
using Pos_System.Repository.Interfaces;

namespace Pos_System.Repository.Implement;

public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
{
	public TContext Context { get; }
	private Dictionary<Type, object> _repositories;

	public UnitOfWork(TContext context)
	{
		Context = context;
	}

	public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
	{
		_repositories ??= new Dictionary<Type, object>();
		if (_repositories.TryGetValue(typeof(TEntity), out object repository))
		{
			return (IGenericRepository<TEntity>)repository;
		}

		repository = new GenericRepository<TEntity>(Context);
		_repositories.Add(typeof(TEntity), repository);
		return (IGenericRepository<TEntity>)repository;
	}

	public void Dispose()
	{
		Context?.Dispose();
	}

	public int Commit()
	{
		return Context.SaveChanges();
	}

	public async Task<int> CommitAsync()
	{
		return await Context.SaveChangesAsync();
	}
}