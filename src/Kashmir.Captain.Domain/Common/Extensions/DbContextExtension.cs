using Kashmir.Captain.Shared;
using Kashmir.Captain.Shared.Extensions;
using Kashmir.Captain.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Kashmir.Captain.Domain.Extensions
{
    public static class DbContextExtensions
    {
        internal static void TrackEntityChanges(this DbContext ctx, ICurrentUser currentUser, IDateTime dt)
        {
            if (currentUser.UserId is null)
            {
                return;
            }
            foreach (var entry in ctx.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity))
            {
                if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                if (entry.Entity is BaseEntity baseEntity && (entry.State == EntityState.Added || entry.State == EntityState.Modified))
                {
                    if (!baseEntity.CreateUserId.HasValue)
                    {
                        ((BaseEntity)entry.Entity).CreateUserId = currentUser.UserId;
                    }
                    if (string.IsNullOrEmpty(baseEntity.CreateUserName))
                    {
                        ((BaseEntity)entry.Entity).CreateUserName = currentUser.FullName;
                    }
                    if (string.IsNullOrEmpty(baseEntity.CreateSource))
                    {
                        ((BaseEntity)entry.Entity).CreateSource = currentUser.Source;
                    }
                    if (!baseEntity.CreateDateTimeUtc.HasValue)
                    {
                        ((BaseEntity)entry.Entity).CreateDateTimeUtc = dt.DateTimeNow;
                    }

                    ((BaseEntity)entry.Entity).ModifyDateTimeUtc = dt.DateTimeNow;
                    ((BaseEntity)entry.Entity).ModifyUserId = currentUser.UserId;
                    ((BaseEntity)entry.Entity).ModifyUserName = currentUser.FullName;
                    ((BaseEntity)entry.Entity).ModifySource = currentUser.Source;
                }
            }
        }
    }
}
