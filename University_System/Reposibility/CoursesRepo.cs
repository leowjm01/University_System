using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UniSystemTest.Models;
using University_System.Data;

namespace University_System.Reposibility
{
    public class CoursesRepo :ICoursesReponsitory
    {
        private readonly UniDBContext _dbContext;

        public CoursesRepo(UniDBContext dbContext)
        {
            _dbContext = dbContext;  
        }
        public async Task<IEnumerable<Courses>> GetAll()
        {
            var CourseTeacher = await _dbContext.CoursesTeachers
                .FromSqlRaw<CoursesAndTeachers>("GetCourseList").ToListAsync();

            var courses = new List<Courses>();

            foreach (var CT in CourseTeacher)
            {
                var c = new Courses()
                {
                    teacherId = CT.teacherId,
                    courseId = CT.courseId,
                    courseName = CT.courseName,
                    Teachers = new Teachers
                    {
                        teacherId = CT.teacherId,
                        teacherName = CT.teacherName,
                        email = CT.email,
                        gender = CT.gender,
                        IsDeleted= CT.IsDeleted
                    }
                };
                courses.Add(c);
            }

            return courses;
        }

        public async Task<IEnumerable<Courses>> GetById(int id)
        {
            var param = new SqlParameter("@courseId", id);

            var CourseTeacher = await _dbContext.CoursesTeachers
                .FromSqlRaw(@"exec GetCourseID @courseId", param).ToListAsync();


            var courses = new List<Courses>();

            foreach (var CT in CourseTeacher)
            {
                var c = new Courses()
                {
                    teacherId = CT.teacherId,
                    courseId = CT.courseId,
                    courseName = CT.courseName,
                    Teachers = new Teachers
                    {
                        teacherId = CT.teacherId,
                        teacherName = CT.teacherName,
                        email = CT.email,
                        gender = CT.gender,
                        IsDeleted = CT.IsDeleted
                    }
                };
                courses.Add(c);
            }

            return courses;
        }

        public async Task<IEnumerable<Courses>> GetCourseByTeacherId(int id, int pageNum, int pageSize)
        {

            var CourseTeacher = await _dbContext.CoursesTeachers
                    .FromSqlRaw("exec GetCourseByTeacherId @teacherId, @PageNum, @PageSize",
                        new SqlParameter("@CourseName", id),
                        new SqlParameter("@PageNum", pageNum),
                        new SqlParameter("@PageSize", pageSize))
                    .ToListAsync();


            var courses = new List<Courses>();

            foreach (var CT in CourseTeacher)
            {
                var c = new Courses()
                {
                    teacherId = CT.teacherId,
                    courseId = CT.courseId,
                    courseName = CT.courseName,
                    Teachers = new Teachers
                    {
                        teacherId = CT.teacherId,
                        teacherName = CT.teacherName,
                        email = CT.email,
                        gender = CT.gender,
                        IsDeleted = CT.IsDeleted
                    }
                };
                courses.Add(c);
            }

            return courses;
        }

        public async Task<IEnumerable<Courses>> GetPagedCourses(string courseName, int pageNum, int pageSize)
        {
            var CourseTeacher = await _dbContext.CoursesTeachers
                .FromSqlRaw("exec GetPaginatedCourses @CourseName, @PageNum, @PageSize",
                    new SqlParameter("@CourseName", courseName ?? (object)DBNull.Value),
                    new SqlParameter("@PageNum", pageNum),
                    new SqlParameter("@PageSize", pageSize))
                .ToListAsync();
            var courses = new List<Courses>();
            foreach (var CT in CourseTeacher)
            {
                var c = new Courses()
                {
                    teacherId = CT.teacherId,
                    courseId = CT.courseId,
                    courseName = CT.courseName,
                    Teachers = new Teachers
                    {
                        teacherId = CT.teacherId,
                        teacherName = CT.teacherName,
                        email = CT.email,
                        gender = CT.gender,
                        IsDeleted= CT.IsDeleted
                    }
                };
                courses.Add(c);
            }
            return (courses);
        }

        public async Task<int> GetCountAllCoursesByTeacherId(int id)
        {
            var param = new SqlParameter("@teacherId", id);

            var courses = await _dbContext.Courses
                .FromSqlRaw(@"exec GetCourseListByTeacherId @teacherId", param)
                .ToListAsync();

            return courses.Count();
        }

        public async Task<int> GetCountAllCourses()
        {
            var courses = await _dbContext.Courses
             .FromSqlRaw<Courses>("GetCourseList")
             .ToListAsync();

            return courses.Count();
        }

        public async Task<int> GetCountByName(string name)
        {
            var param = new SqlParameter("@courseName", name ?? (object)DBNull.Value);

            var result = await Task.Run(() => _dbContext.CoursesTeachers
                .FromSqlRaw(@"exec GetCourseName @courseName", param)
                .ToListAsync());

            return result.Count();
        }

        public async Task<int> Add(Courses course)
        {
            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@courseName", course.courseName));
            parameter.Add(new SqlParameter("@teacherId", course.teacherId));

            var result = await Task.Run(() => _dbContext.Database
           .ExecuteSqlRawAsync(@"exec AddNewCourse @courseName, @teacherId", parameter.ToArray()));

            return result;
        }

        public async Task<int> Update(Courses course)
        {

            var parameter = new List<SqlParameter>();
            parameter.Add(new SqlParameter("@courseId", course.courseId));
            parameter.Add(new SqlParameter("@courseName", course.courseName));
            parameter.Add(new SqlParameter("@teacherId", course.teacherId));

            var result = await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec UpdateCourse @courseId, @courseName, @teacherId", parameter.ToArray()));

            return result;
        }

        public async Task<int> Delete(int id)
        {
            var param = new SqlParameter("@courseId", id);

            return await Task.Run(() => _dbContext.Database
            .ExecuteSqlRawAsync(@"exec DeleteCourse @courseId", param));
        }
    }
}
