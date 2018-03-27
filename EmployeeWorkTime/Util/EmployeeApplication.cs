using EmployeeWorkTime.DAL;
using EmployeeWorkTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeWorkTime.Util
{
    public class EmployeeApplication
    {
        private DatabaseContext db = new DatabaseContext();

        public Employee GetByUsernameAndPassword(Employee user)
        {
            return db.Users.Where(u => u.UserName == user.UserName & u.Password == user.Password).FirstOrDefault();
        }
    }
}