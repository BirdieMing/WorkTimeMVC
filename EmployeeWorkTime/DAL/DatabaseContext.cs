using EmployeeWorkTime.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmployeeWorkTime.DAL
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext() : base("EmployeeWorkTime")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<WorkTime> WorkTimes { get; set; }

        public DbSet<Employee> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}