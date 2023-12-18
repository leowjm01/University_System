using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
    public class StudentsController : Controller
    {
        private readonly IStudentsService Studentservice;
        private readonly IScoreResultsService ScoreResultsservice;

        public StudentsController(IStudentsService service, IScoreResultsService scoreResultsservice)
        {
            Studentservice = service;
            ScoreResultsservice = scoreResultsservice;
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
                try
                {
                    await Studentservice.Add(students);

                    //alert message
                    TempData["SuccessMessage"] = "Create new student successful";

                    return RedirectToAction(nameof(Index));
                }catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "The student email " + students.email + " has created. Please try create again !!";
                    return RedirectToAction(nameof(Create));
                }
            }
            return View(students);
        }


        // GET: Students/Details
        public async Task<IActionResult> Details(int id, int pageNum = 1, int pageSize = 10)
        {

            var student = await Studentservice.GetById(id);
            var result = await ScoreResultsservice.GetScoreResultByStudentId(id);

            if (student == null || result == null)
            {
                return NotFound();
            }

            var viewModel = new StudentScoreResultViewModel
            {
                Students = student.FirstOrDefault(),
                ScoreResults = result.ToList(),
            };

            viewModel.ScoreResults = paginationScoreResult(viewModel.ScoreResults, pageNum, pageSize).ToList();


            return View(viewModel);
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
                try
                {
                    await Studentservice.Update(students);
                    //alert message
                    TempData["SuccessMessage"] = "Edit successful";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "The student email " + students.email + " has created. Please try edit again !! ";
                    return RedirectToAction(nameof(Edit));
                }
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
            //alert message
            TempData["SuccessMessage"] = "Delete successful";
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

        //pagination for course
        public IEnumerable<ScoreResults> paginationScoreResult(IEnumerable<ScoreResults> result, int pageNum, int pageSize)
        {
            // Store the original students for pagination
            var originalStudents = result.ToList();

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
