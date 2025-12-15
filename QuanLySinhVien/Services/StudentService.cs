using QuanLySinhVien.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _repo;
        public StudentService(IStudentRepository repo) => _repo = repo;

        public Task<IEnumerable<Student>> GetAllAsync() => _repo.GetAllAsync();
        public Task AddAsync(Student s) => _repo.AddAsync(s);
        public Task UpdateAsync(Student s) => _repo.UpdateAsync(s);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
        public Task<IEnumerable<Student>> SearchByNameAsync(string name) => _repo.SearchByNameAsync(name);
        public Task<IEnumerable<Student>> GetSortedByNameAsync() => _repo.GetSortedByNameAsync();
    }

}
