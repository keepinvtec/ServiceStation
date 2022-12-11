using lab2;
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
    db.Database.Migrate();
    //db.Database.EnsureDeleted();
}

//CRUD

// Додавання
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client tom = new Client { FullName = "Tom Cooper" };
//    Client alice = new Client { FullName = "Alice Cooper" };

//    db.Clients.Add(tom);
//    db.Clients.Add(alice);
//    db.SaveChanges();
//}

//Читання
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    var clients = db.Clients.ToList();
//    foreach (Client acc in clients)
//    {
//        Console.WriteLine($"{acc.FullName} | {acc.PHnumber}");
//    }
//}

//Редагування
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client? client = db.Clients.FirstOrDefault();
//    if (client != null)
//    {
//        client.FullName = "Bob Hulk";
//        db.SaveChanges();
//    }
//}

//Видалення
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client? client = db.Clients.FirstOrDefault();
//    if (client != null)
//    {
//        db.Clients.Remove(client);
//        db.SaveChanges();
//    }
//}

// TPH
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client tom = new Client { FullName = "Tom Cooper" };
//    Client alice = new Client { FullName = "Alice Cooper" };

//    db.Clients.Add(tom);
//    db.Clients.Add(alice);

//    VIPclient Pitt = new VIPclient { FullName = "Brad Pitt", YearsOfService = 2 };
//    db.VIPs.Add(Pitt);

//    db.SaveChanges();


//    Console.WriteLine("\nКлiєнти");
//    foreach (var acc in db.Clients.ToList())
//    {
//        Console.WriteLine(acc.FullName);
//    }

//    Console.WriteLine("\nVIP клiєнти");
//    foreach (var acc in db.VIPs.ToList())
//    {
//        Console.WriteLine(acc.FullName);
//    }
//}

// TPT
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client tom = new Client { FullName = "Tom Cooper" };
//    Client alice = new Client { FullName = "Alice Cooper" };

//    db.Clients.Add(tom);
//    db.Clients.Add(alice);

//    ObsoleteClient Pitt = new ObsoleteClient { FullName = "Brad Pitt" };
//    db.ObsoleteClients.Add(Pitt);

//    db.SaveChanges();


//    Console.WriteLine("\nКлiєнти");
//    foreach (var acc in db.Clients.ToList())
//    {
//        Console.WriteLine(acc.FullName);
//    }

//    Console.WriteLine("\nВидалені клiєнти");
//    foreach (var acc in db.ObsoleteClients.ToList())
//    {
//        Console.WriteLine(acc.FullName);
//    }
//}
