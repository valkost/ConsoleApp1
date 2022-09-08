using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    internal class DepartmentClass
    {
        public int id= 0 ;
        public string? name = "";
    }

    internal class EmployeeClass
    {
        public int id = 0;
        public int department_id = 0;
        public string? chief_id = "";
        public string? name = "";
        public int salary = 0;
    }

    internal class ChiefClass
    {
        public int id = 0;
        public int dep_id = 0;
        public int salary = 0;
    }
}
