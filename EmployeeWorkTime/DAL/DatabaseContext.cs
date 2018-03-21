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

        public DatabaseContext() : base("DatabaseContext")
        {
        }

        public DbSet<WorkTime> WorkTimes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}