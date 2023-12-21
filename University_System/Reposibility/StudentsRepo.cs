using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniSystemTest.Models;
using University_System.Data;

namespace University_System.Reposibility
{
    public class StudentsRepo : IStudentsReponsitory
    {
        private readonly UniDBContext _dbContext;

        public StudentsRepo(UniDBContext dbContext)
        {
            _dbContext = dbContext;  //constructor
        }

        public async Task<IEnumerable<Students>> GetAll()
        {
            var students = await _dbContext.Students
             .FromSqlRaw<Students>("GetAllStudents")
             .ToListAsync();

            return students;
        }

        public async Task<IEnumerable<Students>> GetAllIncludeDelete()
        {
            var students = await _dbContext.Students
             .FromSqlRaw<Students>("GetAllStudentsIncludeDelete")
             .ToListAsync();

            return students;
        }

        public async Task<IEnumerable<Students>> GetById(int id)
        {
            var param = new SqlParameter("@studentId", id);

            var result = await Task.Run(() => _dbContext.Students
                .FromSqlRaw(@"exec GetStudentByStudentId @studentId", param)
                .ToListAsync());

            return result;
        }
        
        public async Task<IEnumerable<Students>> GetPagedStudents(string studentName, int pageNum, int pageSize)
        {

            var students = await _dbContext.Students
                .FromSqlRaw("exec GetPaginatedStudents @StudentName, @PageNum, @PageSize",
                    new SqlParameter("@StudentName", studentName ?? (object)DBNull.Value),
                    new SqlParameter("@PageNum", pageNum),
                    new SqlParameter("@PageSize", pageSize))
                .ToListAsync();

            return (students);
        }

        public async Task<int> GetCountAllStudents()
        {
            var students = await _dbContext.Students
             .FromSqlRaw<Students>("GetAllStudents")
             .ToListAsync();

            return students.Count();
        }

        public async Task<int> GetCountByName(string name)
        {
            var param = new SqlParameter("@studentName", name ?? (object)DBNull.Value);

            var result = await Task.Run(() => _dbContext.Students
                .FromSqlRaw(@"exec GetStudentByStudentName @studentName", param)
                .ToListAsync());

            return result.Count();
        }

        public async Task<int> Add(Students student)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@studentName", student.studentName));
            parameter.Add(new SqlParameter("@gender", student.gender));
            parameter.Add(new SqlParameter("@email", student.email));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec AddNewStudent @StudentName, @email, @gender", parameter.ToArray()));

            return result;
        }

        public async Task<int> Update(Students student)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@studentId", student.studentId));
            parameter.Add(new SqlParameter("@studentName", student.studentName));
            parameter.Add(new SqlParameter("@gender", student.gender));
            parameter.Add(new SqlParameter("@email", student.email));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec UpdateStudent @studentId, @StudentName, @email, @gender", parameter.ToArray()));

            return result;
        }

        public async Task<int> Delete(int id)
        {
            var param = new SqlParameter("@studentId", id);

            return await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec DeleteStudent @studentId", param));
        }
    }
}
