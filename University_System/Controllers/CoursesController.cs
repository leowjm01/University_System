using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniSystemTest.Models;
using University_System.Data;
using University_System.Services;

namespace University_System.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ITeachersService TeachersService;
        private readonly ICoursesService CoursesService;

        public CoursesController(ITeachersService TService, ICoursesService CService)
        {
            TeachersService = TService;
            CoursesService = CService;
        }


        // GET: Courses
        public async Task<IActionResult> Index(string courseName, int pageNum = 1, int pageSize = 10)
        {
            IEnumerable<Courses> courses;

            if (!string.IsNullOrEmpty(courseName))
            {
                courses = await CoursesService.GetByName(courseName);
            }
            else
            {
                courses = await CoursesService.GetAll();
            }

            //pagination
            var paginatedTeachers = pagination(courses, pageNum, pageSize);

            return View(paginatedTeachers);
        }


        // GET: Courses/Create
        public async Task<IActionResult> Create()
        {
            var getAllTeacher = await TeachersService.GetAll();

            ViewData["teacherId"] = new SelectList(getAllTeacher, "teacherId", "teacherName");
            return View();
        }
        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("courseId,courseName,teacherId")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                await CoursesService.Add(courses);
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }


        // GET: Courses/Details
        public async Task<IActionResult> Details(int id)
        {
            var course = await CoursesService.GetById(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course.FirstOrDefault());
        }


        // GET: Courses/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var course = await CoursesService.GetById(id);

            var getAllTeacher = await TeachersService.GetAll();

            if (course == null)
            {
                return NotFound();
            }
            ViewData["teacherId"] = new SelectList(getAllTeacher, "teacherId", "teacherName");

            return View(course.FirstOrDefault());
        }
        // POST: Courses/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("courseId,courseName,teacherId")] Courses courses)
        {
            var getAllTeacher = await TeachersService.GetAll();

            if (id != courses.courseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await CoursesService.Update(courses);
                return RedirectToAction(nameof(Index));
            }

            ViewData["teacherId"] = new SelectList(getAllTeacher, "teacherId", "teacherName");
            return View(courses);
        }


        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var courses = await CoursesService.GetById(id);

            if (courses == null)
            {
                return NotFound();
            }

            return View(courses.FirstOrDefault());
        }
        
        // POST: Courses/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await CoursesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        //pagination 
        public IEnumerable<Courses> pagination(IEnumerable<Courses> courses, int pageNum, int pageSize)
        {
            // Store the original students for pagination
            var originalStudents = courses.ToList();

            int totalStudentsCount = originalStudents.Count();
            int totalP = (int)Math.Ceiling(totalStudentsCount / (double)pageSize);

            ViewBag.TotalPages = totalP;
            ViewBag.CurrentPage = pageNum;
            ViewBag.DisplayedStudentsCount = totalStudentsCount;
            ViewBag.PageSize = pageSize;

            int startIndex = (pageNum - 1) * pageSize;

            if (startIndex >= totalStudentsCount)
            {
                if (totalStudentsCount > 0)
                {
                    // Calculate the maximum possible page number based on available data
                    int maxPage = (int)Math.Ceiling(totalStudentsCount / (double)pageSize);
                    return originalStudents.Skip((maxPage - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    return originalStudents.Skip(0).Take(pageSize).ToList();
                }
            }

            return originalStudents.Skip(startIndex)
                .Take(pageSize > 10 ? 10 : pageSize)    // each page total display 10 data
                .ToList();
        }

    }
}
