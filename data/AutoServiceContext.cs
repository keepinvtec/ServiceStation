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
            entity.Property(x => x.YearOfSale).HasDefaultValue(null);
            entity.ToTable(t => t.HasCheckConstraint("YearOfProd", "YearOfProd > 2000 AND YearOfProd < 2022"));
            entity.HasData(
                     new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368, Price = 123, YearOfSale = 2022 },
                     new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429, Price = 114, YearOfSale = 2022 },
                     new Car { VINcode = "2T2HZMAAXNC228796", Manufacture = "Toyota", Brand = "Lexus", Model = "RX",
                               EngDisplacement = 2394, YearOfProd = 2021, Mileage = 2315, Price = 125 },
                     new Car { VINcode = "WAUG8AFC1JN012500", Manufacture = "VAG", Brand = "Audi", Model = "A6",
                               EngDisplacement = 2984, YearOfProd = 2018, Mileage = 42156, Price = 190, YearOfSale = 2022 },
                     new Car { VINcode = "WP1AA2A2XGKA08083", Manufacture = "VAG", Brand = "Skoda", Model = "Octavia",
                               EngDisplacement = 1990, YearOfProd = 2016, Mileage = 100344, Price = 177, YearOfSale = 2021 },
                     new Car { VINcode = "WVGFF9BP8BD000455", Manufacture = "VAG", Brand = "Volkswagen", Model = "Touareg",
                               EngDisplacement = 2986, YearOfProd = 2014, Mileage = 156302, Price = 128, YearOfSale = 2020 },
                     new Car { VINcode = "1GYFK43519R118886", Manufacture = "GM", Brand = "Cadillac", Model = "Escalade",
                               EngDisplacement = 6162, YearOfProd = 2007, Mileage = 24571, Price = 129 },
                     new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
                               EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567, Price = 130 }
                );
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
