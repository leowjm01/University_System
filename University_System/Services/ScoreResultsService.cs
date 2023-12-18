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
        public async Task<IEnumerable<ScoreResults>> GetScoreResultByStudentId(int id)
        {
            return (await _reponsitory.GetScoreResultByStudentId(id));
        }
        public async Task<int> GetExamSelectedByStudentId(int studentId)
        {
            return (await _reponsitory.GetExamSelectedByStudentId(studentId));
        }

        public async Task<int> Add(ScoreResults result, int examSelected)
        {
            examSelected += result.mark == null || result.mark < 50 ? 1: -1;


            checkGrade(result);

            return (await _reponsitory.Add(result, examSelected));
        }

        public async Task<int> Update(ScoreResults result, int examSelected) 
        {
            //check grade
            checkGrade(result);

            return (await _reponsitory.Update(result, examSelected)); // declare next step is repository
        }

        public async Task<int> UpdateExamSelected(int studentId, int examSelected) 
        {
            return (await _reponsitory.UpdateExamSelected(studentId, examSelected)); 
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
                decimal mark = Convert.ToDecimal(result.mark);

                switch (mark)
                {
                    case decimal m when m >= 50 && m < 60:
                        result.grade = "D";
                        break;

                    case decimal m when m >= 60 && m < 70:
                        result.grade = "C";
                        break;

                    case decimal m when m >= 70 && m < 80:
                        result.grade = "B";
                        break;

                    case decimal m when m >= 80 && m < 90:
                        result.grade = "A";
                        break;

                    case decimal m when m >= 90 && m <= 100:
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
