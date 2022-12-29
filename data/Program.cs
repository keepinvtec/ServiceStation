using lab4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();
string connectionString = config.GetConnectionString("DefaultConnection")!;

var optionsBuilder = new DbContextOptionsBuilder<AutoServiceContext>();
var options = optionsBuilder.UseSqlServer(connectionString).Options;


using (AutoServiceContext db = new AutoServiceContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}

var temp = new defense4(options);
DateTime date = DateTime.Now;

//запуск синхронного методу як асинхронного
Task task = Task.Run(() => temp.StartThreading());
task.Wait();

DateTime dateafter = DateTime.Now;
Console.WriteLine(dateafter - date);
