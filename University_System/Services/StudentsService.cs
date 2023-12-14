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
        public async Task<IEnumerable<Students>> GetByName(string name)
        {
            return (await _reponsitory.GetByName(name));
        }
        public async Task<IEnumerable<Students>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
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

