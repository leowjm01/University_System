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
            _dbContext = dbContext;  //constructor
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

        public async Task<IEnumerable<Courses>> GetByName(string name)
        {
            var param = new SqlParameter("@courseName", name);

            var CourseTeacher = await _dbContext.CoursesTeachers
                .FromSqlRaw(@"exec GetCourseName @courseName", param).ToListAsync();

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

        public async Task<IEnumerable<Courses>> GetCourseByTeacherId(int id)
        {
            var param = new SqlParameter("@teacherId", id);

            var CourseTeacher = await _dbContext.CoursesTeachers
                .FromSqlRaw(@"exec GetCourseByTeacherId @teacherId", param).ToListAsync();


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
