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

        public async Task<IEnumerable<ScoreResults>> GetScoreResultByStudentId(int id, int pageNum, int pageSize)
        {
            return (await _reponsitory.GetScoreResultByStudentId(id, pageNum, pageSize));
        }

        public async Task<IEnumerable<ScoreResults>> GetPagedScoreResults(string studentName, int pageNum, int pageSize)
        {
            return (await _reponsitory.GetPagedScoreResults(studentName, pageNum, pageSize));
        }

        public async Task<int> GetCountAllScoreResultByStudentId(int id)
        {
            return (await _reponsitory.GetCountAllScoreResultByStudentId(id));
        }

        public async Task<int> GetCountAllScoreResults()
        {
            return (await _reponsitory.GetCountAllScoreResults());
        }

        public async Task<int> GetCountByStudentName(string name)
        {
            return (await _reponsitory.GetCountByStudentName(name));
        }

        public async Task<int> GetExamSelectedByStudentId(int studentId)
        {
            return (await _reponsitory.GetExamSelectedByStudentId(studentId));
        }

        public async Task<int> Add(ScoreResults result, int examSelected)
        {
            examSelected = result.mark == null || result.mark < 50 ? examSelected + 1: examSelected;


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
            IEnumerable<ScoreResults> result = await GetById(id);

            //delete the select exam in student table
            int getExamSelected = await GetExamSelectedByStudentId(result.First().studentId);
            getExamSelected = result.First().mark == null || result.First().mark < 50 ? getExamSelected - 1 : getExamSelected;
            getExamSelected = getExamSelected == getExamSelected - 1 ? 0 : getExamSelected;

            await UpdateExamSelected(result.First().studentId, getExamSelected);

            return (await _reponsitory.Delete(id)); 
        }

        public async Task<int> DeleteByStudentId(int id)
        {
            return (await _reponsitory.DeleteByStudentId(id));
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
