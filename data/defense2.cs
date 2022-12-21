using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class defense2class
    {
        private DbContextOptions<AutoServiceContext> options;
        private SampleContextFactory dbfactory = new SampleContextFactory();

        public defense2class(DbContextOptions<AutoServiceContext> options)
        {
            this.options = options;
        }

        public void EditCarPrice(string manufacture, double price)
        {
            using (AutoServiceContext db = dbfactory.CreateDbContext(new string[] { }))
            {
                var cars = (from Car in db.Cars
                            where Car.Manufacture == manufacture
                            select Car);
                foreach (Car car in cars)
                {
                    car.Price = price * car.Price;
                }
                db.SaveChanges();
            }
        }
    }
}
