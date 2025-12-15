using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> SearchByNameAsync(string namePart);
        Task<IEnumerable<Student>> GetSortedByNameAsync();
    }
}
