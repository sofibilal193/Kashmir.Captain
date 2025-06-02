using Microsoft.EntityFrameworkCore;
using Kashmir.Captain.Domain.Entities;
using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Common;
using Kashmir.Captain.Common;
using Kashmir.Captain.Common.Extensions;
using Microsoft.Extensions.Logging;
using ICryptoProvider = Kashmir.Captain.Common.Crypto.ICryptoProvider;

namespace KC.Domain.Context
{
    public class InventoryDbContext : BaseSqlDbContext<InventoryDbContext>
    {
        private readonly string _inventorySchema = "inventory";

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, ILogger<InventoryDbContext> logger,
        ICryptoProvider cryptoProvider, ICurrentUser currentUser, IDateTime dateTime, MediatR.IMediator mediator)
        : base(options, logger, cryptoProvider, currentUser, dateTime, mediator)
        { }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Tool> Tools { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new WorkerConfiguration(_inventorySchema));
            builder.ApplyConfiguration(new MachineConfiguration(_inventorySchema));
            builder.ApplyConfiguration(new ToolConfiguration(_inventorySchema));
        }
    }
}
