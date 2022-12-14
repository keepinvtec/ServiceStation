using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace lab2;

public class AutoServiceContext : DbContext
{
    public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
        : base(options)
    {

    }

    public DbSet<Car> Cars { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Worker> Workers { get; set; }

    public DbSet<VIPclient> VIPs { get; set; }

    public DbSet<ObsoleteClient> ObsoleteClients { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<ListOfProvidedServices> ListsOfProvidedServices { get; set; }

    public DbSet<ListOfServices> ListsOfServices { get; set; }

    public DbSet<ListOfSpareParts> ListsOfSpareParts { get; set; }

    public IQueryable<Car> GetCarsByYearOfProd(int yearofprod) => FromExpression(() => GetCarsByYearOfProd(yearofprod));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(x => x.VINcode);
            entity.HasAlternateKey(x => new { x.VINcode, x.EngDisplacement, x.Mileage });
            entity.Property(x => x.VINcode).HasMaxLength(18);
            entity.Property(x => x.Manufacture).HasMaxLength(20);
            entity.Property(x => x.Model).HasMaxLength(20);
            entity.Property(x => x.Brand).HasMaxLength(20);
            entity.ToTable(t => t.HasCheckConstraint("YearOfProd", "YearOfProd > 2000 AND YearOfProd < 2022"));
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.Property(x => x.DateOfEnroll).HasDefaultValueSql("GETDATE()");
            entity.HasData(
                new Enrollment { EnrollmentId = 1, ClientPHnumber = 1 }
                );
            entity.HasOne(d => d.Client).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.ClientPHnumber);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasData(
                new Client { PHnumber = 1, FullName = "Tom Cruise" }
                );
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasOne(d => d.Client).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.ClientPHnumber);

            entity.HasOne(d => d.Car).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CarVINcode);
        });

        modelBuilder.Entity<ListOfProvidedServices>(entity =>
        {
            entity.HasOne(d => d.Invoice).WithMany(p => p.ListOfProvidedServices)
                .HasForeignKey(d => d.InvoiceOrderId);

            entity.HasOne(d => d.ListOfServices).WithMany(p => p.ListOfProvidedServices)
                .HasForeignKey(d => d.ListOfServicesServiceID);
        });

        modelBuilder.Entity<ObsoleteClient>(entity =>
        {
            entity.ToTable("Obsoletes");
            entity.Property(x => x.DateOfDeletion).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<ListOfSpareParts>(entity =>
        {
            entity.HasKey(x => x.SPnumber);
            entity.Property(x => x.NameOfPart).IsRequired();
            entity.HasAlternateKey(x => x.SPnumber);
            entity.HasOne(d => d.Invoice).WithMany(p => p.ListOfSpareParts)
                .HasForeignKey(d => d.InvoiceOrderId);
        });

        modelBuilder.HasDbFunction(() => GetCarsByYearOfProd(default));
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
