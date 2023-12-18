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
            IEnumerable<Teachers> teachers;

            if (!string.IsNullOrEmpty(teacherName))
            {
                teachers = await TeachersService.GetByName(teacherName);
            }
            else
            {
                teachers = await TeachersService.GetAll();
            }

            //pagination
            var paginatedTeachers = pagination(teachers, pageNum, pageSize);

            return View(paginatedTeachers);
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
            var teacher = await TeachersService.GetById(id);
            var course = await CourseService.GetCourseByTeacherId(id);

            if (teacher == null || course == null)
            {
                return NotFound();
            }

            var viewModel = new TeacherCourseViewModel
            {
                Teachers = teacher.FirstOrDefault(),
                Courses = course.ToList(),
            };

            viewModel.Courses = paginationCourse(viewModel.Courses, pageNum, pageSize).ToList();


            return View(viewModel);
        }


        // GET: TeachersDetails
        public async Task<IActionResult> TeacherDetails(int id, int pageNum = 1, int pageSize = 10)
        {
            var teacher = await TeachersService.GetById(id);
            var course = await CourseService.GetCourseByTeacherId(id);

            if (teacher == null || course == null)
            {
                return NotFound();
            }

            var viewModel = new TeacherCourseViewModel
            {
                Teachers = teacher.FirstOrDefault(),
                Courses = course.ToList(),
            };

            viewModel.Courses = paginationCourse(viewModel.Courses, pageNum, pageSize).ToList();


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
            await TeachersService.Delete(id);
            //alert message
            TempData["SuccessMessage"] = "Delete successful";
            return RedirectToAction(nameof(Index));
        }


        //pagination 
        public IEnumerable<Teachers> pagination(IEnumerable<Teachers> teachers, int pageNum, int pageSize)
        {
            // Store the original students for pagination
            var originalStudents = teachers.ToList();

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

        //pagination for course
        public IEnumerable<Courses> paginationCourse(IEnumerable<Courses> courses, int pageNum, int pageSize)
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
