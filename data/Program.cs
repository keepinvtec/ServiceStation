using lab2;
using lr2ServiceStation.Migrations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("appsettings.json");
var config = builder.Build();
string connectionString = config.GetConnectionString("DefaultConnection")!;

var optionsBuilder = new DbContextOptionsBuilder<AutoServiceContext>();
var options = optionsBuilder.UseSqlServer(connectionString).Options;

object locker = new();
int x = 1;

using (AutoServiceContext db = new AutoServiceContext(options))
{
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
}


Action action = async () =>
{
    Monitor.Enter(locker);
    try
    {
        using (AutoServiceContext db = new AutoServiceContext(options))
        {
            for (int i = 0; i < 100; i++)
            {
                await db.Clients.AddAsync(new Client
                {
                    PHnumber = x,
                    FullName = "Client" +
                    x + "_" + Task.CurrentId.ToString()
                });
                x++;
            }
            db.SaveChanges();
        }
    }
    finally
    {
        Monitor.Exit(locker);
    }
};

Action reading = () =>
{
    using (AutoServiceContext db = new AutoServiceContext(options))
    {
        var clients = db.Clients.ToListAsync();
        clients.Result.ForEach(x => Console.WriteLine(x.FullName));
    }
};

Task task1 = new Task(action);
Task task2 = new Task(action);
Task task3 = new Task(reading);
task1.Start();
task2.Start();
task1.Wait();
task2.Wait();
task3.Start();
task3.Wait();



ThreadStart action1 = () =>
{
    Monitor.Enter(locker);
    try
    {
        using (AutoServiceContext db = new AutoServiceContext(options))
        {
            for (int i = 0; i < 100; i++)
            {
                db.Clients.Add(new Client
                {
                    PHnumber = x,
                    FullName = "Client" +
                    x + "_" + Thread.CurrentThread.Name
                });
                x++;
            }
            db.SaveChanges();
        }
    }
    finally
    {
        Monitor.Exit(locker);
    }
};

ThreadStart reading1 = () =>
{
    lock (locker)
    {
        using (AutoServiceContext db = new AutoServiceContext(options))
        {
            var clients = db.Clients.ToList();
            clients.ForEach(x => Console.WriteLine(x.FullName));
        }
    }
};

var thread1 = new Thread(action1);
thread1.Name = "1";
var thread2 = new Thread(action1);
thread2.Name = "2";
var thread3 = new Thread(action1);
thread3.Name = "3";
var thread4 = new Thread(action1);
thread4.Name = "4";

thread1.Start();
thread2.Start();
thread3.Start();
thread4.Start();
DateTime date = DateTime.Now;
var actionThreadsAreDone = false;
while (!actionThreadsAreDone)
{
    actionThreadsAreDone = thread1.ThreadState == ThreadState.Stopped &&
    thread2.ThreadState == ThreadState.Stopped &&
    thread3.ThreadState == ThreadState.Stopped &&
    thread4.ThreadState == ThreadState.Stopped;
};
DateTime dateafter = DateTime.Now;
Console.WriteLine(dateafter - date);

thread3 = new Thread(reading1);
thread4 = new Thread(reading1);
thread3.Start();
thread4.Start();



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

// Distinct
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Car car1 = new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 };
//    Car car2 = new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 };
//    Car car3 = new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//        EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 };

//    db.Cars.AddRange(car1, car2, car3);
//    db.SaveChanges();

//    var cars = (from Car in db.Cars
//                select Car.Manufacture).Distinct();

//    foreach (var data in cars)
//        Console.WriteLine($"{data}");
//}

// Union
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client client1 = new Client { PHnumber = 976479801, FullName = "Sherban Pavlo" };
//    Client client2 = new Client { PHnumber = 989981902, FullName = "Komar Orest" };

//    db.Clients.AddRange(client1, client2);

//    Worker worker1 = new Worker { FullName = "Worker1" };
//    Worker worker2 = new Worker { FullName = "Worker2" };

//    db.Workers.AddRange(worker1, worker2);
//    db.SaveChanges();

//    var people = (from Client in db.Clients select Client.FullName) 
//        .Union(from Worker in db.Workers select Worker.FullName);

//    foreach (var data in people)
//        Console.WriteLine($"{data}");
//}

// Except
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client client1 = new Client { PHnumber = 2, FullName = "Sherban Pavlo" };
//    Client client2 = new Client { PHnumber = 3, FullName = "Komar Orest" };

//    db.Clients.AddRange(client1, client2);

//    Worker worker1 = new Worker { FullName = "Sherban Pavlo" };
//    db.Workers.Add(worker1);

//    db.SaveChanges();

//    var people = (from Client in db.Clients select Client.FullName)
//        .Except(from Worker in db.Workers select Worker.FullName);

//    foreach (var data in people)
//        Console.WriteLine(data);
//}

// Intersect
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Client client1 = new Client { PHnumber = 2, FullName = "Sherban Pavlo" };
//    Client client2 = new Client { PHnumber = 3, FullName = "Komar Orest" };

//    db.Clients.AddRange(client1, client2);

//    Worker worker1 = new Worker { FullName = "Sherban Pavlo" };
//    db.Workers.Add(worker1);

//    db.SaveChanges();

//    var people = (from Client in db.Clients select Client.FullName)
//        .Intersect(from Worker in db.Workers select Worker.FullName);

//    foreach (var data in people)
//        Console.WriteLine(data);
//}

// Group by
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Car car1 = new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 };
//    Car car2 = new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 };
//    Car car3 = new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//        EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 };

//    db.Cars.AddRange(car1, car2, car3);
//    db.SaveChanges();

//    var cars = (from car in db.Cars
//                group car by car.Manufacture into g
//                select new { Manufacture = g.Key, ModelsCount = g.Count() });

//    foreach (var car in cars)
//    {
//        Console.WriteLine($"{car.Manufacture}, {car.ModelsCount}");
//    }
//}

// Join
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    db.Invoices.AddRange(new Invoice { ClientPHnumber = 660810569, CarVINcode = "19UUB7F02MA000899" },
//                         new Invoice { ClientPHnumber = 500756897, CarVINcode = "5J8YE1H89NL032957" },
//                         new Invoice { ClientPHnumber = 632910750, CarVINcode = "WP1AA2A2XGKA08083" },
//                         new Invoice { ClientPHnumber = 689316954, CarVINcode = "2T2HZMAAXNC228796" });
//    db.Enrollments.AddRange(new Enrollment { ClientPHnumber = 660810569 },
//                            new Enrollment { ClientPHnumber = 500756897 },
//                            new Enrollment { ClientPHnumber = 632910750 },
//                            new Enrollment { ClientPHnumber = 689316954 });
//    db.Clients.AddRange(new Client { PHnumber = 660810569, FullName = "Prokopenko Andrii" },
//                        new Client { PHnumber = 500756897, FullName = "Sternenko Anton" },
//                        new Client { PHnumber = 632910750, FullName = "Petrenko Sergii" },
//                        new Client { PHnumber = 689316954, FullName = "Plaksii Okeksii" });
//    db.Cars.AddRange(new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 },
//                     new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 },
//                     new Car { VINcode = "WP1AA2A2XGKA08083", Manufacture = "VAG", Brand = "Skoda", Model = "Octavia",
//                               EngDisplacement = 1990, YearOfProd = 2016, Mileage = 100344 },
//                     new Car { VINcode = "2T2HZMAAXNC228796", Manufacture = "Toyota", Brand = "Lexus", Model = "RX",
//                               EngDisplacement = 2394, YearOfProd = 2021, Mileage = 2315 });
//    db.SaveChanges();

//    var join = (from Invoice in db.Invoices
//                join Enrollment in db.Enrollments
//                on Invoice.ClientPHnumber equals Enrollment.ClientPHnumber
//                select new { InvoiceID = Invoice.InvoiceId, VINcode = Invoice.CarVINcode, DateOfEnroll = Enrollment.DateOfEnroll });
//    foreach (var data in join)
//        Console.WriteLine($"{data.InvoiceID}, {data.VINcode}, {data.DateOfEnroll}");
//}

// Any
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Car car1 = new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 };
//    Car car2 = new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 };
//    Car car3 = new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//        EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 };

//    db.Cars.AddRange(car1, car2, car3);
//    db.SaveChanges();

//    bool result = db.Cars.Any(x => x.EngDisplacement == 4999);
//    Console.WriteLine(result.ToString());
//}

// All
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Car car1 = new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 };
//    Car car2 = new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 };
//    Car car3 = new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//        EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 };

//    db.Cars.AddRange(car1, car2, car3);
//    db.SaveChanges();

//    bool result = db.Cars.All(x => x.Brand == "Acura");
//    Console.WriteLine(result.ToString());
//}

// Average, Min, Max, Sum, Count
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    Car car1 = new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 };
//    Car car2 = new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//        EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 };
//    Car car3 = new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//        EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 };

//    db.Cars.AddRange(car1, car2, car3);
//    db.SaveChanges();

//    int minMileage = db.Cars.Min(x => x.Mileage);
//    int maxMileage = db.Cars.Max(x => x.Mileage);
//    double avgMileage = db.Cars.Average(x => x.Mileage);
//    int countCars = db.Cars.Count();
//    double totalMileage = db.Cars.Sum(x => x.Mileage);
//    avgMileage = Math.Round(avgMileage, 2);
//    totalMileage = Math.Round(totalMileage, 2);

//    Console.WriteLine($"Avg: {avgMileage}; Min: {minMileage}; Max: {maxMileage};\n" +
//        $"{countCars} cars presented in table; Total mileage = {totalMileage}");
//}

// Eager loading
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    db.Clients.Add(new Client { PHnumber = 2, FullName = "Marta Cruise" });

//    db.Enrollments.Add(new Enrollment { ClientPHnumber = 2 });

//    db.SaveChanges();

//    var clients = db.Clients.Include(x => x.Enrollments).ToList();

//    foreach (var client in clients)
//        Console.WriteLine($"{client.FullName}, {client.Enrollments[0].DateOfEnroll}");
//}

// Explicit loading
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    db.Enrollments.Add(new Enrollment { ClientPHnumber = 1 });

//    db.SaveChanges();

//    Client? client = db.Clients.FirstOrDefault();
//    if (client != null)
//    {
//        db.Entry(client).Collection(x => x.Enrollments).Load();

//        foreach (var user in client.Enrollments)
//            Console.WriteLine($"{user.ClientPHnumber}, {user.EnrollmentId}, {user.DateOfEnroll}");
//    }
//}

// Complicated Eager loading
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    db.Clients.Add(new Client { PHnumber = 2, FullName = "Marta Cruise" });
//    db.Invoices.AddRange(new Invoice { ClientPHnumber = 2, CarVINcode = "19UUB7F02MA000899" },
//                         new Invoice { ClientPHnumber = 1, CarVINcode = "vinkod" });
//    db.Cars.AddRange(new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//                          EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 },
//                     new Car { VINcode = "vinkod", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//                          EngDisplacement = 2986, YearOfProd = 2018, Mileage = 12344 });

//    db.Enrollments.Add(new Enrollment { ClientPHnumber = 2 });

//    db.SaveChanges();

//    var enrollments = db.Enrollments.Include(x => x.Client).ThenInclude(s => s.Invoices).ToList();

//    foreach (var data in enrollments)
//        Console.WriteLine($"{data.DateOfEnroll}, {data.Client.PHnumber}, {data.Client.FullName}, {data.Client.Invoices[0].CarVINcode}");
//}

// Lazy loading
//using (AutoServiceContext db = new AutoServiceContext(optionsBuilder.UseLazyLoadingProxies()
//    .UseSqlServer(connectionString).Options))
//{
//    db.Clients.Add(new Client { PHnumber = 2, FullName = "Marta Cruise" });

//    db.Enrollments.Add(new Enrollment { ClientPHnumber = 2 });

//    db.SaveChanges();
//}
//using (AutoServiceContext db = new AutoServiceContext(optionsBuilder.UseLazyLoadingProxies()
//    .UseSqlServer(connectionString).Options))
//{
//    var clients = db.Clients.ToList();
//    foreach (var client in clients)
//        Console.WriteLine($"{client.FullName}, {client.Enrollments[0].DateOfEnroll}");
//}

// AsNoTracking
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    var client = db.Clients.AsNoTracking().FirstOrDefault();
//    if (client != null)
//    {
//        client.FullName = "Marta Cruise";
//        db.SaveChanges();
//    }

//    var clients = db.Clients.ToList();
//    foreach (var u in clients)
//        Console.WriteLine($"{u.FullName} ({u.PHnumber})");
//}
//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    var client1 = db.Clients.FirstOrDefault();
//    var client2 = db.Clients.AsNoTracking().FirstOrDefault();

//    if (client1 != null && client2 != null)
//    {
//        Console.WriteLine($"Before User1: {client1.FullName}   User2: {client2.FullName}");

//        client1.FullName = "Bob";

//        Console.WriteLine($"After User1: {client1.FullName}   User2: {client2.FullName}");
//    }
//}


// Приклади виклику збережених процедур та функцій за допомогою Entity Framework

//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    db.Cars.AddRange(new Car { VINcode = "19UUB7F02MA000899", Manufacture = "Honda", Brand = "Acura", Model = "TLX",
//                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 12368 },
//                     new Car { VINcode = "5J8YE1H89NL032957", Manufacture = "Honda", Brand = "Acura", Model = "MDX",
//                               EngDisplacement = 2986, YearOfProd = 2021, Mileage = 5429 },
//                     new Car { VINcode = "2T2HZMAAXNC228796", Manufacture = "Toyota", Brand = "Lexus", Model = "RX",
//                               EngDisplacement = 2394, YearOfProd = 2021, Mileage = 2315 },
//                     new Car { VINcode = "WAUG8AFC1JN012500", Manufacture = "VAG", Brand = "Audi", Model = "A6",
//                               EngDisplacement = 2984, YearOfProd = 2018, Mileage = 42156 },
//                     new Car { VINcode = "WP1AA2A2XGKA08083", Manufacture = "VAG", Brand = "Skoda", Model = "Octavia",
//                               EngDisplacement = 1990, YearOfProd = 2016, Mileage = 100344 },
//                     new Car { VINcode = "WVGFF9BP8BD000455", Manufacture = "VAG", Brand = "Volkswagen", Model = "Touareg",
//                               EngDisplacement = 2986, YearOfProd = 2014, Mileage = 156302 },
//                     new Car { VINcode = "1GYFK43519R118886", Manufacture = "GM", Brand = "Cadillac", Model = "Escalade",
//                               EngDisplacement = 6162, YearOfProd = 2007, Mileage = 24571 },
//                     new Car { VINcode = "SALGS2TF9FA225873", Manufacture = "JLR", Brand = "Land Rover", Model = "Range Rover",
//                               EngDisplacement = 4999, YearOfProd = 2013, Mileage = 111567 });
//    db.SaveChanges();

//    var cars = db.GetCarsByYearOfProd(2014);
//    foreach (var car in cars)
//        Console.WriteLine($"{car.Manufacture}, {car.Model}, {car.YearOfProd}");
//}

//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    SqlParameter param = new()
//    {
//        ParameterName = "@VINcode",
//        SqlDbType = System.Data.SqlDbType.VarChar,
//        Direction = System.Data.ParameterDirection.Output,
//        Size = 18
//    };
//    db.Database.ExecuteSqlRaw("GetCarWithMaxMileage @VINcode OUT", param);
//    Console.WriteLine(param.Value);
//}

//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    var manufacturetochange = "VAG";
//    var pricetochange = 1.25;

//    var cars = db.Cars.ToList();
//    foreach (Car car in cars)
//        Console.WriteLine($"Car manufacture: {car.Manufacture} | Price before: {car.Price}");

//    var test = new defense2class(options);
//    test.EditCarPrice(manufacturetochange, pricetochange);
//    foreach (Car car in cars)
//        db.Entry(car).Reload();

//    Console.WriteLine("\n");
//    foreach (Car car in cars)
//        Console.WriteLine($"Car manufacture: {car.Manufacture} | Price after: {car.Price}");
//}

//using (AutoServiceContext db = new AutoServiceContext(options))
//{
//    var cars = (from Car in db.Cars
//                where Car.YearOfSale >= (DateTime.Now.Year - 1)
//                group Car by Car.Manufacture into table
//                select new { Manufacture = table.Key, AvgPrice = table.Average(x => x.Price),
//                             SoldAutos = table.Count()});

//    foreach (var car in cars)
//        Console.WriteLine($"Manufacture: {car.Manufacture} | SoldAutos: {car.SoldAutos} | AvgPrice: {car.AvgPrice}");
//}
