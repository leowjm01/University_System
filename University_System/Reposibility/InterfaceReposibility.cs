using UniSystemTest.Models;

namespace University_System.Reposibility
{
    public interface IStudentsReponsitory
    {
        Task<IEnumerable<Students>> GetAll();                               // retrive all student data
        Task<IEnumerable<Students>> GetByName(string name);                 // get students by student name
        Task<IEnumerable<Students>> GetById(int id);                        // get students by student id
        //Task<int> GetExamSelectedByStudentId(int id);                       // get exam selected by student id
        Task<int> Add(Students student);                                    // add new student function
        Task<int> Update(Students student);                                 // update student details
        Task<int> Delete(int id);                                           // delete student (soft delete) 
    }

    public interface ITeachersReponsitory
    {
        Task<IEnumerable<Teachers>> GetAll();                               // retrive all teacher data
        Task<IEnumerable<Teachers>> GetByName(string name);                 // get students by student name
        Task<IEnumerable<Teachers>> GetById(int id);                        // get teacher details by id 
        Task<int> Add(Teachers teacher);                                    // add new teacher
        Task<int> Update(Teachers teacher);                                 // update teacher details
        Task<int> Delete(int id);                                           // delete teacher (soft delete) 

    }

    public interface ICoursesReponsitory
    {
        Task<IEnumerable<Courses>> GetAll();                                // retrive all course data
        Task<IEnumerable<Courses>> GetByName(string name);                  // get courses by course name
        Task<IEnumerable<Courses>> GetById(int id);                         // get courses by course Id
        Task<IEnumerable<Courses>> GetCourseByTeacherId(int id);            // get courses by course Id
        Task<int> Add(Courses course);                                      // add new course
        Task<int> Update(Courses course);                                   // update course details
        Task<int> Delete(int id);                                           // delete course

    }

    public interface IScoreResultsReponsitory
    {
        Task<IEnumerable<ScoreResults>> GetAll();                                                                   // retrive all score results data
        Task<IEnumerable<ScoreResults>> GetById(int id);                                                            // get score result data by score result id
        Task<IEnumerable<ScoreResults>> GetByStudentName(string name);                                              // get score result by student name
        Task<IEnumerable<ScoreResults>> CheckCourseSelected(int? scoreResultId, int studentId, int courseId);       // Check couse repeated by student selected
        Task<int> GetExamSelectedByStudentId(int id);                                                               // get exam selected by student id
        Task<int> Add(ScoreResults results, int examSelect);                                                        // add new score result 
        Task<int> Update(ScoreResults results, int examSelect);                                                     // update score result details
        Task<int> UpdateExamSelected(int studentId, int examSelect);                                                // update exame selected in student database
        Task<int> Delete(int id);                                                                                   // delete 

    }
}
