using UniSystemTest.Models;
using University_System.Reposibility;

namespace University_System.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsReponsitory _reponsitory;

        public StudentsService(IStudentsReponsitory reponsitory)
        {
            _reponsitory = reponsitory;
        }

        public async Task<IEnumerable<Students>> GetAll()
        {
            return (await _reponsitory.GetAll());
        }
        public async Task<IEnumerable<Students>> GetAllIncludeDelete()
        {
            return (await _reponsitory.GetAllIncludeDelete());
        }
        public async Task<IEnumerable<Students>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
        }
        public async Task<IEnumerable<Students>> GetPagedStudents(string studentName, int pageNum, int pageSize) 
        {
            return (await _reponsitory.GetPagedStudents(studentName, pageNum, pageSize));
        }
        public async Task<int> GetCountAllStudents()
        {
            return (await _reponsitory.GetCountAllStudents());
        }
        public async Task<int> GetCountByName(string name)
        {
            return (await _reponsitory.GetCountByName(name));
        }
        public async Task<int> Add(Students student)
        {
            return (await _reponsitory.Add(student));
        }
        public async Task<int> Update(Students student) 
        {
            return (await _reponsitory.Update(student)); 
        }
        public async Task<int> Delete(int id) 
        {
            return (await _reponsitory.Delete(id)); 
        }
    }
}

