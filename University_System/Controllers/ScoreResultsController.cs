using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UniSystemTest.Models;
using University_System.Data;
using University_System.Services;

namespace University_System.Controllers
{
    public class ScoreResultsController : Controller
    {
        private readonly IScoreResultsService ScoreResultsService;
        private readonly IStudentsService StudentsService;
        private readonly ICoursesService CoursesService;

        public ScoreResultsController(IScoreResultsService SRService, IStudentsService SService, ICoursesService CService)
        {
            StudentsService = SService;
            ScoreResultsService = SRService;
            CoursesService = CService;
        }

        // GET: Score Result
        public async Task<IActionResult> Index(string studentName, int pageNum = 1, int pageSize = 10)
        {
            IEnumerable<ScoreResults> results;

            if (!string.IsNullOrEmpty(studentName))
            {
                results = await ScoreResultsService.GetByStudentName(studentName);
            }
            else
            {
                results = await ScoreResultsService.GetAll();
            }

            //pagination
            var paginatedTeachers = pagination(results, pageNum, pageSize);

            return View(paginatedTeachers);
        }

        public async Task<IActionResult> Create()
        {


            ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName");
            ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName");

            return View();
        }

        // POST: ScoreResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("scoreResultId,mark,grade,courseId,studentId")] ScoreResults scoreResults)
        {

            await CheckExamSelected(scoreResults.studentId, scoreResults.courseId);

            await CheckCourseSelected(scoreResults.scoreResultId, scoreResults.studentId, scoreResults.courseId);

            if (ModelState.IsValid)
            {
                var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(scoreResults.studentId);

                //add new score result
                await ScoreResultsService.Add(scoreResults, getExamSelected);
                return RedirectToAction(nameof(Index));
            }
            return View(scoreResults);
        }

        //check quantity exam selected by student
        public async Task CheckExamSelected(int studentId, int courseId) 
        {
            var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(studentId);

            // check the select exam is more than 10 or not
            if (getExamSelected >= 10)
            {
                ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", courseId);
                ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", studentId);
                RedirectToAction(nameof(Create));
            }
        }

        //check the repeated course student selected
        public async Task CheckCourseSelected(int? scoreResultId, int studentId, int courseId) 
        {

            var check = await ScoreResultsService.CheckCourseSelected(scoreResultId, studentId, courseId);

            //check the student that have select two same course or not
            if (!check.IsNullOrEmpty())
            {
                ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", courseId);
                ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", studentId);

                ModelState.AddModelError("courseId", "Course already selected before. Please try select another course.....");
                RedirectToAction(nameof(Create));
            }
        }

        //pagination
        public IEnumerable<ScoreResults> pagination(IEnumerable<ScoreResults> results, int pageNum, int pageSize)
        {
            // Store the original students for pagination
            var originalStudents = results.ToList();

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
