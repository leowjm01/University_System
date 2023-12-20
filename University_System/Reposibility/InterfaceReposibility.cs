using System.Threading.Tasks;
using UniSystemTest.Models;

namespace University_System.Reposibility
{
    public interface IStudentsReponsitory
    {
        Task<IEnumerable<Students>> GetAll();                                                                   // retrive all students data
        Task<IEnumerable<Students>> GetById(int id);                                                            // get students by student id
        Task<IEnumerable<Students>> GetPagedStudents(string studentName, int pageNum, int pageSize);            // pagination and search function
        Task<int> GetCountAllStudents();                                                                        // count all students
        Task<int> GetCountByName(string name);                                                                  // cont total student by name
        Task<int> Add(Students student);                                                                        // add new student function
        Task<int> Update(Students student);                                                                     // update student details
        Task<int> Delete(int id);                                                                               // delete student (soft delete) 
    }

    public interface ITeachersReponsitory
    {
        Task<IEnumerable<Teachers>> GetAll();                                                                   // retrive all teacher data
        Task<IEnumerable<Teachers>> GetById(int id);                                                            // get teacher details by id 
        Task<IEnumerable<Teachers>> GetPagedTeachers(string studentName, int pageNum, int pageSize);            // pagination and search function
        Task<int> GetCountAllTeachers();                                                                        // count all teachers
        Task<int> GetCountByName(string name);                                                                  // cont total teachers by name
        Task<int> Add(Teachers teacher);                                                                        // add new teacher
        Task<int> Update(Teachers teacher);                                                                     // update teacher details
        Task<int> Delete(int id);                                                                               // delete teacher (soft delete) 

    }

    public interface ICoursesReponsitory
    {
        Task<IEnumerable<Courses>> GetAll();                                                                    // retrive all course data
        Task<IEnumerable<Courses>> GetById(int id);                                                             // get courses by course Id
        Task<IEnumerable<Courses>> GetCourseByStudentId(int id);                                                // get where course No exit in score result
        Task<IEnumerable<Courses>> GetCourseByTeacherId(int id, int pageNum, int pageSize);                     // get courses by teacher Id
        Task<IEnumerable<Courses>> GetPagedCourses(string courseName, int pageNum, int pageSize);               // pagination and search function
        Task<int> GetCountAllCoursesByTeacherId(int id);                                                        // count all course by teacher id
        Task<int> GetCountAllCourses();                                                                         // count all course
        Task<int> GetCountByName(string name);                                                                  // cont total course by name
        Task<int> Add(Courses course);                                                                          // add new course
        Task<int> Update(Courses course);                                                                       // update course details
        Task<int> Delete(int id);                                                                               // delete course

    }

    public interface IScoreResultsReponsitory
    {
        Task<IEnumerable<ScoreResults>> GetAll();                                                                   // retrive all score results data
        Task<IEnumerable<ScoreResults>> GetById(int id);                                                            // get score result data by score result id
        Task<IEnumerable<ScoreResults>> CheckCourseSelected(int? scoreResultId, int studentId, int courseId);       // Check couse repeated by student selected
        Task<IEnumerable<ScoreResults>> GetScoreResultByStudentId(int id, int pageNum, int pageSize);               // get score result by student Id
        Task<IEnumerable<ScoreResults>> GetPagedScoreResults(string studentName, int pageNum, int pageSize);        // pagination and search function
        Task<int> GetCountAllScoreResultByStudentId(int id);                                                        // count all score result by student id
        Task<int> GetCountAllScoreResults();                                                                        // count all score result
        Task<int> GetCountByStudentName(string name);                                                               // cont total score result by student name
        Task<int> GetExamSelectedByStudentId(int id);                                                               // get exam selected by student id
        Task<int> Add(ScoreResults results, int examSelect);                                                        // add new score result 
        Task<int> Update(ScoreResults results, int examSelect);                                                     // update score result details
        Task<int> UpdateExamSelected(int studentId, int examSelect);                                                // update exame selected in student database
        Task<int> Delete(int id);                                                                                   // delete by score result id
        Task<int> DeleteByStudentId(int id);                                                                        // delete by student id

    }
}
