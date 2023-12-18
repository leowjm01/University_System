using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Drawing.Printing;
using UniSystemTest.Models;
using University_System.Data;

namespace University_System.Reposibility
{
    public class TeachersRepo : ITeachersReponsitory
    {
        private readonly UniDBContext _dbContext;

        public TeachersRepo(UniDBContext dbContext)
        {
            _dbContext = dbContext;  //constructor
        }

        public async Task<IEnumerable<Teachers>> GetAll()
        {
            return await _dbContext.Teachers
                 .FromSqlRaw<Teachers>("GetTeacherList")
                 .ToListAsync();
        }

        public async Task<IEnumerable<Teachers>> GetById(int id)
        {
            var param = new SqlParameter("@teacherId", id);

            var reult = await Task.Run(() => _dbContext.Teachers
                .FromSqlRaw(@"exec GetTeacherID @teacherId", param)
                .ToListAsync());

            return reult;
        }

        public async Task<IEnumerable<Teachers>> GetPagedTeachers(string teacherName, int pageNum, int pageSize)
        {
            var teachers = await _dbContext.Teachers
                .FromSqlRaw("exec GetPaginatedTeachers @TeacherName, @PageNum, @PageSize",
                    new SqlParameter("@TeacherName", teacherName ?? (object)DBNull.Value),
                    new SqlParameter("@PageNum", pageNum),
                    new SqlParameter("@PageSize", pageSize))
                .ToListAsync();

            return (teachers);
        }

        public async Task<int> GetCountAllTeachers()
        {
            var students = await _dbContext.Teachers
             .FromSqlRaw<Teachers>("GetTeacherList")
             .ToListAsync();

            return students.Count();
        }

        public async Task<int> GetCountByName(string name)
        {
            var param = new SqlParameter("@teacherName", name ?? (object)DBNull.Value);

            var result = await Task.Run(() => _dbContext.Teachers
                .FromSqlRaw(@"exec GetTeacherName @teacherName", param)
                .ToListAsync());

            return result.Count();
        }

        public async Task<int> Add(Teachers teacher)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@teacherName", teacher.teacherName));
            parameter.Add(new SqlParameter("@gender", teacher.gender));
            parameter.Add(new SqlParameter("@email", teacher.email));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec AddNewTeacher @teacherName, @email, @gender", parameter.ToArray()));

            return result;
        }

        public async Task<int> Update(Teachers teacher)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@teacherId", teacher.teacherId));
            parameter.Add(new SqlParameter("@teacherName", teacher.teacherName));
            parameter.Add(new SqlParameter("@gender", teacher.gender));
            parameter.Add(new SqlParameter("@email", teacher.email));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec UpdateTeacher @teacherId, @teacherName, @email, @gender", parameter.ToArray()));

            return result;
        }

        public async Task<int> Delete(int id)
        {
            var param = new SqlParameter("@teacherId", id);

            return await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec DeletedTeacher @teacherId", param));
        }
    }
}
