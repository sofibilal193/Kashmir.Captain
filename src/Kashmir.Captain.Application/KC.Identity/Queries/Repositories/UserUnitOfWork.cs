// using KC.Data;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace KC.Application.Users.Queries.Repositories
// {
// 	public class UserUnitOfWork : IUserUnitOfWork
// 	{

// 		private readonly KcIdentityDbContext _context;

// 		private bool _disposedValue;

// 		public UserUnitOfWork(KcIdentityDbContext context, IUserRepository userRepository)
// 		{
// 			_context = context;
// 			// repositories
// 			UserRepository = userRepository;
// 		}

// 		public IUserRepository UserRepository { get; private set; }

// 		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
// 		{
// 			return await _context.SaveChangesAsync(cancellationToken);
// 		}

// 		// public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
// 		// {
// 		// 	return await _context.SaveEntitiesAsync(cancellationToken);
// 		// }

// 		protected virtual void Dispose(bool disposing)
// 		{
// 			if (!_disposedValue)
// 			{
// 				if (disposing)
// 				{
// 					_context.Dispose();
// 				}
// 				_disposedValue = true;
// 			}
// 		}

// 		public void Dispose()
// 		{
// 			Dispose(disposing: true);
// 			GC.SuppressFinalize(this);
// 		}
// 	}
// }