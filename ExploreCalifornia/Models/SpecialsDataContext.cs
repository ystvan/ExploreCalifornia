using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExploreCalifornia.Models
{
    public class Special
    {
        public long Id { get; set; }
        public string Key { get; internal set; }
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public int Price { get; internal set; }
    }

    public class SpecialsDataContext : DbContext
    {
        public DbSet<Special> Specials { get; set; }

        public SpecialsDataContext(DbContextOptions<SpecialsDataContext> options)
            : base(options)
        {
            // With this in place Entity Framework will check to make sure that database exists.
            Database.EnsureCreated();
        }
        public IEnumerable<Special> GetMonthlySpecials()
        {
            //return new[]
            //{
            //    new Special {
            //        Key = "calm",
            //        Name = "California Smoke Tour",
            //        Type = "1 ounce weed Package",
            //        Price = 420,
            //    },
            //    new Special {
            //        Key = "desert",
            //        Name = "From Desert to Sea",
            //        Type = "2 Day Salton Sea",
            //        Price = 350,
            //    },
            //    new Special {
            //        Key = "backpack",
            //        Name = "Backpack Cali",
            //        Type = "Big Sur Retreat",
            //        Price = 620,
            //    },
            //    new Special {
            //        Key = "taste",
            //        Name = "Taste of California",
            //        Type = "Tapas & Groves",
            //        Price = 150,
            //    },
            //};

            return Specials.ToArray();
        }
    }
}
