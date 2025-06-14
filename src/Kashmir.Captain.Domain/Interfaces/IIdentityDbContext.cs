using Kashmir.Captain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kashmir.Captain.Domain.Interfaces
{
    public interface IIdentityDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}