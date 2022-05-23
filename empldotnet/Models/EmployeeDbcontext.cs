using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace empldotnet.Models
{
    public class EmployeeDbcontext: DbContext
    {
        public EmployeeDbcontext(): base("Default")
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}