using UniSystemTest.Models;
using University_System.Reposibility;

namespace University_System.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesReponsitory _reponsitory;

        public CoursesService(ICoursesReponsitory reponsitory)
        {
            _reponsitory = reponsitory;
        }

        public async Task<IEnumerable<Courses>> GetAll()
        {
            return (await _reponsitory.GetAll());
        }
        public async Task<IEnumerable<Courses>> GetByName(string name)
        {
            return (await _reponsitory.GetByName(name));
        }
        public async Task<IEnumerable<Courses>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
        }
        public async Task<IEnumerable<Courses>> GetCourseByTeacherId(int id)
        {
            return (await _reponsitory.GetCourseByTeacherId(id));
        }
        public async Task<int> Add(Courses course)
        {
            return (await _reponsitory.Add(course));
        }
        public async Task<int> Update(Courses course) 
        {
            return (await _reponsitory.Update(course)); 
        }
        public async Task<int> Delete(int id) 
        {
            return (await _reponsitory.Delete(id)); 
        }
    }
}
