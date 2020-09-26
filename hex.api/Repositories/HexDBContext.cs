using hex.api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Repositories
{
    public class HexDBContext : DbContext
    {
        public DbSet<Container> Containers { get; set; }
        public DbSet<Observer> Observers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ContainerPlace> ContainerPlaces { get; set; }
        public DbSet<ContainerPlacePlan> ContainerPlacePlans { get; set; }
        public DbSet<Beacon> Beacons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseNpgsql("host=localhost;database=hex;user id=postgres;password=System64");
    }
}
