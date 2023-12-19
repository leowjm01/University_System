using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniSystemTest.Models;
using University_System.Data;
using University_System.Services;

namespace University_System.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeachersService TeachersService;
        private readonly ICoursesService CourseService;

        public TeachersController(ITeachersService Tservice, ICoursesService CService)
        {
            TeachersService = Tservice;
            CourseService = CService;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string teacherName, int pageNum = 1, int pageSize = 10)
        {

            var paginatedteachers = await pagination(teacherName, pageNum, pageSize);

            return View(paginatedteachers);
        }


        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("teacherId,teacherName,gender,email")] Teachers teachers)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await TeachersService.Add(teachers);

                    //alert message
                    TempData["SuccessMessage"] = "Create new teacher successful";

                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    TempData["ErrorMessage"] = "The teacher email " + teachers.email + " has created. Please try create again !!";
                    return RedirectToAction(nameof(Create));
                }
            }
            return View(teachers);
        }


        // GET: Teachers/Details
        public async Task<IActionResult> Details(int id, int pageNum = 1, int pageSize = 10)
        {
            var viewModel = await paginationCourse(id, pageNum, pageSize);

            return View(viewModel);
        }


        // GET: TeachersDetails
        public async Task<IActionResult> TeacherDetails(int id, int pageNum = 1, int pageSize = 5)
        {

            var viewModel = await paginationCourse(id, pageNum, pageSize);

            return View(viewModel);
        }


        // GET: Teachers/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await TeachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher.FirstOrDefault());
        }
        // POST: Teachers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("teacherId,teacherName,gender,email")] Teachers teachers)
        {
            if (id != teachers.teacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await TeachersService.Update(teachers);
                    //alert message
                    TempData["SuccessMessage"] = "Edit successful";
                    return RedirectToAction(nameof(Index));
                }catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "The teacher email " + teachers.email + " has created. Please try create again !!";
                    return RedirectToAction(nameof(Edit));
                }
            }
            return View(teachers);
        }


        // GET: Teachers/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await TeachersService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher.FirstOrDefault());
        }
        // POST: Teacher/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var count = await CourseService.GetCountAllCoursesByTeacherId(id);

            if (count > 1 ) 
            {
                //alert message
                TempData["ErrorMessage"] = "Delete failed. This is because that have couses handle by the teacher !!";
                return RedirectToAction(nameof(Delete));
            }

            await TeachersService.Delete(id);
            //alert message
            TempData["SuccessMessage"] = "Delete successful";
            return RedirectToAction(nameof(Index));
        }


        //pagination
        public async Task<IEnumerable<Teachers>> pagination(string teacherName, int pageNum, int pageSize)
        {

            var paginatedTeachers = await TeachersService.GetPagedTeachers(teacherName, pageNum, pageSize);
            int totalCount = 0;

            if (teacherName != null)
            {
                totalCount = await TeachersService.GetCountByName(teacherName);
            }
            else
            {
                totalCount = await TeachersService.GetCountAllTeachers();
            }

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;

            return paginatedTeachers;
        }


        //pagination for course
        public async Task<TeacherCourseViewModel> paginationCourse(int teacherId, int pageNum, int pageSize)
        {
            var teacher = await TeachersService.GetById(teacherId);
            var paginatedCourses = await CourseService.GetCourseByTeacherId(teacherId, pageNum, pageSize);
            int totalCount = 0;

            var viewModel = new TeacherCourseViewModel
            {
                Teachers = teacher.FirstOrDefault(),
                Courses = paginatedCourses.ToList(),
            };

            
            totalCount = await CourseService.GetCountAllCoursesByTeacherId(teacherId);

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;

            return viewModel;
        }
    }
}
