using System;
using System.Collections.Generic;
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
    public class StudentsController : Controller
    {
        private readonly IStudentsService Studentservice;

        public StudentsController(IStudentsService service)
        {
            Studentservice = service;
        }
        // GET: Students
        public async Task<IActionResult> Index(string studentName, int pageNum = 1, int pageSize = 10)
        {
            IEnumerable<Students> students;

            if (!string.IsNullOrEmpty(studentName))
            {
                students = await Studentservice.GetByName(studentName);
            }
            else
            {
                students = await Studentservice.GetAll();
            }

            //pagination
            var paginatedStudents = pagination(students, pageNum, pageSize);

            return View(paginatedStudents);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("studentId,studentName,gender,email")] Students students)
        {
            if (ModelState.IsValid)
            {
                await Studentservice.Add(students);

                //message
                TempData["SuccessMessage"] = "Create successful";

                //// Example of setting cancel message after canceling edit
                //TempData["EditCancelMessage"] = "Edit canceled";
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }


        // GET: Students/Details
        public async Task<IActionResult> Details(int id)
        {
            var student = await Studentservice.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student.FirstOrDefault());
        }

        // GET: Students/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var student = await Studentservice.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student.FirstOrDefault());
        }
        // POST: Students/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("studentId,studentName,email")] Students students)
        {
            if (id != students.studentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await Studentservice.Update(students);
                return RedirectToAction(nameof(Index));
            }
            return View(students);
        }


        // GET: Students/Delete
        public async Task<IActionResult> Delete(int id)
        {
            var student = await Studentservice.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student.FirstOrDefault());
        }
        // POST: Students/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await Studentservice.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        //pagination
        public IEnumerable<Students> pagination(IEnumerable<Students> students, int pageNum, int pageSize)
        {
            // Store the original students for pagination
            var originalStudents = students.ToList();

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
