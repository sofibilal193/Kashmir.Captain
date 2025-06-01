using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Kashmir.Captain.Domain.Entities;
using Kashmir.Captain.Domain;

namespace KC.Domain.Context
{
    public class InventoryDbContext : DbContext
    {
        private readonly string _inventorySchema;

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _inventorySchema = configuration.GetValue<string>("InventorySchema") ?? "inventory";
        }

        //        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, ILogger<InventoryDbContext> logger,
        //     ICryptoProvider cryptoProvider, ICurrentUser currentUser, IDateTime dateTime, MediatR.IMediator mediator)
        // : base(options, logger, cryptoProvider, currentUser, dateTime, mediator)
        //     { }

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
