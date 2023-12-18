using UniSystemTest.Models;
using University_System.Reposibility;

namespace University_System.Services
{
    public class TeachersService: ITeachersService
    {
        private readonly ITeachersReponsitory _reponsitory;

        public TeachersService(ITeachersReponsitory reponsitory)
        {
            _reponsitory = reponsitory;
        }

        public async Task<IEnumerable<Teachers>> GetAll() 
        {
            return (await _reponsitory.GetAll()); 
        }
        public async Task<IEnumerable<Teachers>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
        }
        public async Task<IEnumerable<Teachers>> GetPagedTeachers(string teacherName, int pageNum, int pageSize)
        {
            return (await _reponsitory.GetPagedTeachers(teacherName, pageNum, pageSize));
        }
        public async Task<int> GetCountAllTeachers()
        {
            return (await _reponsitory.GetCountAllTeachers());
        }
        public async Task<int> GetCountByName(string name)
        {
            return (await _reponsitory.GetCountByName(name));
        }
        public async Task<int> Add(Teachers teacher)
        {
            return (await _reponsitory.Add(teacher));
        }
        public async Task<int> Update(Teachers teacher) 
        {
            return (await _reponsitory.Update(teacher)); 
        }
        public async Task<int> Delete(int id) 
        {
            return (await _reponsitory.Delete(id)); 
        }
    }
}
