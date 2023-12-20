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
            var paginatedStudents = await pagination(studentName, pageNum, pageSize);
            ViewData["StudentName"] = studentName;

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
        public async Task<IActionResult> Details(int id, int pageNum = 1, int pageSize = 5)
        {

            var viewModel = await paginationScoreResult(id, pageNum, pageSize);

            return View(viewModel);
        }

        // GET: Students/TeacherDetails
        public async Task<IActionResult> StudentDetails(int id, int pageNum = 1, int pageSize = 5)
        {

            var viewModel = await paginationScoreResult(id, pageNum, pageSize);

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
            await ScoreResultsservice.DeleteByStudentId(id);
            //alert message
            TempData["SuccessMessage"] = "Delete successful";
            return RedirectToAction(nameof(Index));
        }


        //pagination
        public async Task <IEnumerable<Students>> pagination(string studentName, int pageNum, int pageSize)
        {

            var paginatedStudents = await Studentservice.GetPagedStudents(studentName, pageNum, pageSize);
            int totalCount = 0;
  
            if (studentName != null)
            {
                totalCount = await Studentservice.GetCountByName(studentName);
            }
            else {
                totalCount = await Studentservice.GetCountAllStudents();
            }

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize); 
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;

            return paginatedStudents;
        }

        //pagination for course
        public async Task<StudentScoreResultViewModel> paginationScoreResult(int studentId, int pageNum, int pageSize)
        {
            var students = await Studentservice.GetById(studentId);
            var paginatedResult = await ScoreResultsservice.GetScoreResultByStudentId(studentId, pageNum, pageSize);
            int totalCount = 0;

            var viewModel = new StudentScoreResultViewModel
            {
                Students = students.FirstOrDefault(),
                ScoreResults = paginatedResult.ToList(),
            };


            totalCount = await ScoreResultsservice.GetCountAllScoreResultByStudentId(studentId);

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;

            return viewModel;
        }
    }
}
