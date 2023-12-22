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

            var paginatedCourses = await pagination(courseName, pageNum, pageSize);
            ViewData["CourseName"] = courseName;

            return View(paginatedCourses);
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
                TempData["SuccessMessage"] =  "Create course successful";
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }


        // GET: Courses/Details
        public async Task<IActionResult> Details(int id)
        {
            var course = await CoursesService.GetById(id);

            if (course.Count() == 0)
            {
                return NotFound();
            }

            return View(course.FirstOrDefault());
        }

        // GET: Courses/DetailsFromTeacher
        //public async Task<IActionResult> GetDetailsFromTeacher(int id)
        //{
        //    var course = await CoursesService.GetByTeacherId(id);

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(course.FirstOrDefault());
        //}



        // GET: Courses/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var course = await CoursesService.GetById(id);

            var getAllTeacher = await TeachersService.GetAll();

            if (course.Count() == 0)
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
                TempData["SuccessMessage"] =  "Edit course successful";
                return RedirectToAction(nameof(Index));
            }

            ViewData["teacherId"] = new SelectList(getAllTeacher, "teacherId", "teacherName");
            return View(courses);
        }


        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var courses = await CoursesService.GetById(id);

            if (courses.Count() == 0)
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
            TempData["SuccessMessage"] =  "Delete course successful";
            return RedirectToAction(nameof(Index));
        }

        //pagination
        public async Task<IEnumerable<Courses>> pagination(string courseName, int pageNum, int pageSize)
        {

            var paginatedCourses = await CoursesService.GetPagedCourses(courseName, pageNum, pageSize);
            int totalCount = 0;

            if (courseName != null)
            {
                totalCount = await CoursesService.GetCountByName(courseName);
            }
            else
            {
                totalCount = await CoursesService.GetCountAllCourses();
            }

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;

            return paginatedCourses;
        }

    }
}
