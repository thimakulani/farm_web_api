﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farm_web_api.models
{
    public class Employee
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Id#")]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }


    }
}
