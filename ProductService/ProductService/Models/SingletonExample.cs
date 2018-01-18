using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductService.Models.SingletonExample
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public enum CompanyCategory
    {
        IT = 0,
        Communication = 1,
        Electronics = 2,
        Others = 3
    }

    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Revenue { get; set; }
        public CompanyCategory Category { get; set; }
        public List<Employee> Employees { get; set; }
    }
}