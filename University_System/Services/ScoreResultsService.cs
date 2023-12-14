using UniSystemTest.Models;
using University_System.Reposibility;

namespace University_System.Services
{
    public class ScoreResultsService : IScoreResultsService
    {
        private readonly IScoreResultsReponsitory _reponsitory;

        public ScoreResultsService(IScoreResultsReponsitory reponsitory)
        {
            _reponsitory = reponsitory;
        }

        public async Task<IEnumerable<ScoreResults>> GetAll() 
        {
            return (await _reponsitory.GetAll()); 
        }

        public async Task<IEnumerable<ScoreResults>> GetById(int id)
        {
            return (await _reponsitory.GetById(id));
        }

        public async Task<IEnumerable<ScoreResults>> GetByStudentName(string name)
        {
            return (await _reponsitory.GetByStudentName(name));
        }

        public async Task<IEnumerable<ScoreResults>> CheckCourseSelected(int? scoreResultId, int studentId, int courseId)
        {
            return (await _reponsitory.CheckCourseSelected(scoreResultId, studentId, courseId));
        }

        public async Task<int> GetExamSelectedByStudentId(int studentId)
        {
            return (await _reponsitory.GetExamSelectedByStudentId(studentId));
        }

        public async Task<int> Add(ScoreResults result, int selectExam)
        {
            if (result.mark < 50 || result.mark == null) selectExam++;

            checkGrade(result);

            return (await _reponsitory.Add(result, selectExam));
        }

        public async Task<int> Update(ScoreResults result, int selectExam) 
        {
            // check Selected exam
            if (result.mark >= 50) selectExam--;

            //check grade
            checkGrade(result);

            return (await _reponsitory.Update(result, selectExam)); // declare next step is repository
        }

        public async Task<int> UpdateExamSelected(int studentId, int selectExam) 
        {
            return (await _reponsitory.UpdateExamSelected(studentId, selectExam)); 
        }

        public async Task<int> Delete(int id) 
        {
            return (await _reponsitory.Delete(id)); 
        }

        //Check the grade of score results
        public void checkGrade(ScoreResults result)
        {
            if (result.mark == null)
            {
                result.grade = null;
            }
            else
            {
                int mark = Convert.ToInt32(result.mark);

                switch (mark)
                {
                    case int m when m >= 50 && m < 60:
                        result.grade = "D";
                        break;

                    case int m when m >= 60 && m < 70:
                        result.grade = "C";
                        break;

                    case int m when m >= 70 && m < 80:
                        result.grade = "B";
                        break;

                    case int m when m >= 80 && m < 90:
                        result.grade = "A";
                        break;

                    case int m when m >= 90 && m <= 100:
                        result.grade = "A+";
                        break;

                    default:
                        result.grade = "E";
                        break;
                }
            }
        }
    }
}
