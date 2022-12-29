using Microsoft.EntityFrameworkCore;

namespace lab4
{
    public class defense4
    {
        private DbContextOptions<AutoServiceContext> options;
        public int x = 1;
        object locker = new();

        public defense4(DbContextOptions<AutoServiceContext> options)
        {
            this.options = options;
        }
        public async Task GetClients()
        {
            await using (AutoServiceContext db = new AutoServiceContext(options))
            {
                for (int i = 0; i < 100; i++)
                {
                    lock (locker)
                    {
                        db.Clients.AddAsync(new Client
                        {
                            PHnumber = x,
                            FullName = "Client" + x + " " + Thread.CurrentThread.Name
                        });
                        x++;
                    }
                }
                db.SaveChanges();
            }
        }

        List<Thread> threads = new List<Thread>();
        public void StartThreading()
        {
            for (int i = 0; i < 100; i++)
            {
                //запуск асинхронного методу як синхронного
                //Thread myThread = new Thread(() => AsyncHelper.RunSync(() => GetClients()));

                Thread myThread = new Thread(async () => await GetClients());
                myThread.Name = $"Thread {i}";
                threads.Add(myThread);
                myThread.Start();
            }

            foreach (Thread thread in threads)
                thread.Join();
        }
    }

    public static class AsyncHelper
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskContinuationOptions.None,
                        TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        public static void RunSync(Func<Task> func)
            => _taskFactory
                .StartNew(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}
