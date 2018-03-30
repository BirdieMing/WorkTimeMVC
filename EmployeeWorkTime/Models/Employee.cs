using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EmployeeWorkTime.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public bool IsManager { get; set; }

        public virtual ICollection<WorkTime> WorkTimes { get; set; }
    }
}