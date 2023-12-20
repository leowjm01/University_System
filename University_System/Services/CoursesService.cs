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
        public async Task<IEnumerable<Courses>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
        }
        public async Task<IEnumerable<Courses>> GetCourseByStudentId(int id)
        {
            return (await _reponsitory.GetCourseByStudentId(id));
        }
        public async Task<IEnumerable<Courses>> GetCourseByTeacherId(int id, int pageNum, int pageSize)
        {
            return (await _reponsitory.GetCourseByTeacherId(id, pageNum, pageSize));
        }
        public async Task<IEnumerable<Courses>> GetPagedCourses(string courseName, int pageNum, int pageSize)
        {
            return (await _reponsitory.GetPagedCourses(courseName, pageNum, pageSize));
        }
        public async Task<int> GetCountAllCourses()
        {
            return (await _reponsitory.GetCountAllCourses());
        }
        public async Task<int> GetCountAllCoursesByTeacherId(int id)
        {
            return (await _reponsitory.GetCountAllCoursesByTeacherId(id));
        }
        public async Task<int> GetCountByName(string name)
        {
            return (await _reponsitory.GetCountByName(name));
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
