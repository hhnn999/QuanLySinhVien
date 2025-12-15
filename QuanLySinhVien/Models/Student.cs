using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Student
{
    public int StudentId { get; set; }
    public string Code { get; set; } = "";
    public string FullName { get; set; } = "";
    public DateTime? BirthDate { get; set; }
    public string Address { get; set; } = "";
    public ClassRoom? ClassRoom { get; set; }
}