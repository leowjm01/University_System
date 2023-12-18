using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using UniSystemTest.Models;
using University_System.Data;

namespace University_System.Reposibility
{
    public class ScoreResultsRepo : IScoreResultsReponsitory
    {
        private readonly UniDBContext _dbContext;

        public ScoreResultsRepo(UniDBContext dbContext)
        {
            _dbContext = dbContext;  
        }

        public async Task<IEnumerable<ScoreResults>> GetAll()
        {
            var resultStudentCourse = await _dbContext.ResultStudentCourse
                .FromSqlRaw<ResultStudentCourse>("GetResultList").ToListAsync();

            var results = new List<ScoreResults>();

            foreach (var RSC in resultStudentCourse)
            {
                var r = new ScoreResults()
                {
                    scoreResultId = RSC.scoreResultId,
                    grade = RSC.grade,
                    mark = RSC.mark,
                    Courses = new Courses
                    {
                        courseId = RSC.courseId,
                        courseName = RSC.courseName
                    },
                    Students = new Students
                    {
                        studentId = RSC.studentId,
                        studentName = RSC.studentName,
                        gender = RSC.gender,
                    }
                };
                results.Add(r);
            }

            return results;
        }

        public async Task<IEnumerable<ScoreResults>> GetById(int id)
        {
            var param = new SqlParameter("@scoreResultId", id);

            var resultStudentCourse = await _dbContext.ResultStudentCourse
                .FromSqlRaw(@"exec GetScoreResultByID @scoreResultId", param).ToListAsync();


            var results = new List<ScoreResults>();

            foreach (var RSC in resultStudentCourse)
            {
                var r = new ScoreResults()
                {
                    scoreResultId = RSC.scoreResultId,
                    grade = RSC.grade,
                    mark = RSC.mark,
                    courseId = RSC.courseId,
                    studentId = RSC.studentId,
                    Courses = new Courses
                    {
                        courseId = RSC.courseId,
                        courseName = RSC.courseName
                    },
                    Students = new Students
                    {
                        studentId = RSC.studentId,
                        studentName = RSC.studentName,
                        email = RSC.email,
                        gender = RSC.gender
                    }
                };
                results.Add(r);
            }

            return results;
        }

        public async Task<IEnumerable<ScoreResults>> GetByStudentName(string name)
        {
            var param = new SqlParameter("@studentName", name);

            var resultStudentCourse = await Task.Run(() => _dbContext.ResultStudentCourse
            .FromSqlRaw(@"exec GetScoreResultByStudentName @studentName", param)
            .ToListAsync());


        var results = new List<ScoreResults>();

            foreach (var RSC in resultStudentCourse)
            {
                var r = new ScoreResults()
                {
                    scoreResultId = RSC.scoreResultId,
                    grade = RSC.grade,
                    mark = RSC.mark,
                    courseId = RSC.courseId,
                    studentId = RSC.studentId,
                    Courses = new Courses
                    {
                        courseId = RSC.courseId,
                        courseName = RSC.courseName
                    },
                    Students = new Students
                    {
                        studentId = RSC.studentId,
                        studentName = RSC.studentName
                    }
                };
                results.Add(r);
            }

            return results;
        }

        public async Task<IEnumerable<ScoreResults>> CheckCourseSelected(int? scoreResultId, int studentId, int courseId)
        {
            var paramS = new SqlParameter("@studentId", studentId);
            var paramC = new SqlParameter("@courseId", courseId);
            var paramR = new SqlParameter("@scoreResultId", scoreResultId == null ? -1 : scoreResultId);

            var result = await Task.Run(() => _dbContext.ScoreResults
                .FromSqlRaw(@"exec CheckCourseSelected @studentId , @courseId, @scoreResultId", paramS, paramC, paramR)
                .ToListAsync());

            return result;
        }

        public async Task<IEnumerable<ScoreResults>> GetScoreResultByStudentId(int id)
        {
            var param = new SqlParameter("@studentId", id);

            var resultStudentCourse = await _dbContext.ResultStudentCourse
                .FromSqlRaw(@"exec GetScoreResultByStudentId @studentId", param).ToListAsync();

            var results = new List<ScoreResults>();

            foreach (var RSC in resultStudentCourse)
            {
                var r = new ScoreResults()
                {
                    scoreResultId = RSC.scoreResultId,
                    grade = RSC.grade,
                    mark = RSC.mark,
                    courseId = RSC.courseId,
                    studentId = RSC.studentId,
                    Courses = new Courses
                    {
                        courseId = RSC.courseId,
                        courseName = RSC.courseName
                    },
                    Students = new Students
                    {
                        studentId = RSC.studentId,
                        studentName = RSC.studentName,
                        email = RSC.email,
                        gender = RSC.gender
                    }
                };
                results.Add(r);
            }

            return results;
        }

        public async Task<int> GetExamSelectedByStudentId(int studentId)
        {
            var param = new SqlParameter("@studentId", studentId);

            var result = await Task.Run(() => _dbContext.ScoreResults
                .FromSqlRaw(@"exec CheckExamSelected @studentId", param)
                .ToListAsync());

            return result.Count();
        }

        public async Task<int> Add(ScoreResults results, int examSelect)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@mark", results.mark == null ? DBNull.Value : results.mark));
            parameter.Add(new SqlParameter("@grade", results.grade == null ? DBNull.Value : results.grade));
            parameter.Add(new SqlParameter("@courseId", results.courseId));
            parameter.Add(new SqlParameter("@studentId", results.studentId));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec AddNewScoreResult @mark,@grade,@courseId,@studentId", parameter.ToArray()));

            await UpdateExamSelected(results.studentId, examSelect);

            return result;
        }

        public async Task<int> Update(ScoreResults results, int examSelect)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@scoreResultId", results.scoreResultId));
            parameter.Add(new SqlParameter("@mark", results.mark == null ? DBNull.Value : results.mark));
            parameter.Add(new SqlParameter("@grade", results.grade == null ? DBNull.Value : results.grade));
            parameter.Add(new SqlParameter("@courseId", results.courseId));
            parameter.Add(new SqlParameter("@studentId", results.studentId));

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec UpdateScoreResult @scoreResultId, @mark, @grade, @courseId, @studentId", parameter.ToArray()));

            await UpdateExamSelected(results.studentId, examSelect);

            return result;
        }

        public async Task<int> UpdateExamSelected(int studentId, int examSelect)
        {

            var parameterS = new List<SqlParameter>();
            parameterS.Add(new SqlParameter("@selectedExam", examSelect));
            parameterS.Add(new SqlParameter("@studentId", studentId));

            var updateSelectedExame = await _dbContext.Database
                 .ExecuteSqlRawAsync(@"exec UpdateSelectedExamByStudentId @studentId, @selectedExam", parameterS.ToArray());

            await _dbContext.SaveChangesAsync();

            return updateSelectedExame;
        }

        public async Task<int> Delete(int id)
        {
            var param = new SqlParameter("@scoreResultId", id);

            return await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec DeleteScoreResult @scoreResultId", param));
        }
    }
}
