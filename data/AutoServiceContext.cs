using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace lab4;

public class AutoServiceContext : DbContext
{
    public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
        : base(options)
    {

    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(x => x.DateOfEnroll).HasDefaultValueSql("GETDATE()");
            entity.HasOne(d => d.Client).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ClientPHnumber);
        });

        modelBuilder.Entity<Client>(entity =>
        {

        });
    }
}

public class SampleContextFactory : IDesignTimeDbContextFactory<AutoServiceContext>
{
    public AutoServiceContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AutoServiceContext>();

        ConfigurationBuilder builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        IConfigurationRoot config = builder.Build();

        string connectionString = config.GetConnectionString("DefaultConnection")!;
        optionsBuilder.UseSqlServer(connectionString);
        return new AutoServiceContext(optionsBuilder.Options);
    }
}
