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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var paginatedScoreResults = await pagination(studentName, pageNum, pageSize);
            ViewData["StudentName"] = studentName;

            return View(paginatedScoreResults);
        }

        public async Task<IActionResult> Create()
        {
            var students = await ScoreResultsService.GetAll();

            var studentId = students.FirstOrDefault()?.Students.studentId;
            ViewData["courseId"] = new SelectList(await CoursesService.GetCourseByStudentId(Convert.ToInt32(studentId)), "courseId", "courseName");
            ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName");

            return View();
        }

        // POST: ScoreResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("scoreResultId,mark,grade,courseId,studentId")] ScoreResults scoreResults)
        {
            //Chech there same course and student in database
            var check = await ScoreResultsService.CheckCourseSelected(scoreResults.scoreResultId, scoreResults.studentId, scoreResults.courseId);
            if (!check.IsNullOrEmpty())
            {
                ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", scoreResults.courseId);
                ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", scoreResults.studentId);

                TempData["ErrorMessage"] =  "Course already selected before. Please try select another course.....";
                return View(scoreResults);
            }

            //check quantity of selected Exam 
            var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(scoreResults.studentId);
            if (getExamSelected >= 10)
            {
                ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", scoreResults.courseId);
                ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", scoreResults.studentId);
                TempData["ErrorMessage"] =  "Student already select 10 subject. Cannot select more already!!! ";
                return View(scoreResults);
            }

            // add new score result
            if (ModelState.IsValid)
            {
                //add new score result
                await ScoreResultsService.Add(scoreResults, getExamSelected);
                TempData["SuccessMessage"] =  "Score result create successful";
                return RedirectToAction(nameof(Index));
            }

            return View(scoreResults);
        }


        // GET: ScoreResults/Details
        public async Task<IActionResult> Details(int id)
        {
            var result = await ScoreResultsService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result.FirstOrDefault());
        }


        // GET: ScoreResults/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await ScoreResultsService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", result.First().courseId);
            ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", result.First().studentId);

            return View(result.FirstOrDefault());
        }

        // POST: ScoreResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("scoreResultId,mark,grade,courseId,studentId")] ScoreResults scoreResults)
        {
            if (ModelState.IsValid)
            {    
                var result = await ScoreResultsService.GetById(id);

                //check update data is same student
               var isUpdate =  await CheckScoreResultsBySameSelect(scoreResults, result.First().studentId, Convert.ToDecimal(result.First().mark));
                if (isUpdate != true)
                {
                    ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", scoreResults.courseId);
                    ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", scoreResults.studentId);
                    TempData["ErrorMessage"] =  "Student already select 10 subject. Cannot select more already!!!";
                    return View(scoreResults);
                }
            }
            TempData["SuccessMessage"] =  "Edit score result successful";
            return RedirectToAction(nameof(Index));
        }

        // GET: ScoreResults/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var result = await ScoreResultsService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result.FirstOrDefault());
        }

        // POST: ScoreResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await ScoreResultsService.GetById(id);

            //delete the select exam in student table
            var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(result.First().studentId);
            getExamSelected = result.First().mark == null || result.First().mark < 50 ? getExamSelected - 1 : getExamSelected;
            getExamSelected = getExamSelected == getExamSelected - 1 ? 0 : getExamSelected;

            await ScoreResultsService.UpdateExamSelected(result.First().studentId, getExamSelected);


            //update the score result data
            await ScoreResultsService.Delete(id);
            TempData["SuccessMessage"] =  "Delete score result successful";
            return RedirectToAction(nameof(Index));
        }


        //check what avalaible course can select by the student
        public async Task<IActionResult> GetCoursesForStudent(int studentId)
        {
            var courses = await CoursesService.GetCourseByStudentId(studentId);

            return Json(courses);
        }

        //check update data is same student
        public async Task<bool> CheckScoreResultsBySameSelect(ScoreResults scoreResults, int studentIdNow, decimal markBefore)
        {

            // check the user is edit the same studentid and course
            if (scoreResults.studentId ==  studentIdNow)
            {
                //check the student edited already
                var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(studentIdNow);

                // add or decrease the quantity of selected result
                if (markBefore >= 50)
                {
                    getExamSelected = scoreResults.mark == null || scoreResults.mark < 50 ? getExamSelected + 1 : getExamSelected;
                }
                else if (markBefore == 0)
                {
                    getExamSelected = scoreResults.mark == null || scoreResults.mark < 50 ? getExamSelected : getExamSelected - 1;
                    getExamSelected = getExamSelected == -1 ? 0 : getExamSelected;
                }
                else 
                {
                    getExamSelected = scoreResults.mark == null || scoreResults.mark < 50 ? getExamSelected : getExamSelected - 1;
                    getExamSelected = getExamSelected == -1 ? 0 : getExamSelected;
                }

                if (getExamSelected <= 10)
                {
                    await ScoreResultsService.Update(scoreResults, getExamSelected);
                    return true;
                }
            }
            return false;
        }

        //pagination
        public async Task<IEnumerable<ScoreResults>> pagination(string studentName, int pageNum, int pageSize)
        {

            var paginatedScoreResult = await ScoreResultsService.GetPagedScoreResults(studentName, pageNum, pageSize);
            int totalCount = 0;

            if (studentName != null)
            {
                totalCount = await ScoreResultsService.GetCountByStudentName(studentName);
            }
            else
            {
                totalCount = await ScoreResultsService.GetCountAllScoreResults();
            }

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = pageNum;
            ViewBag.PageSize = pageSize;
            

            return paginatedScoreResult;
        }
    }
}
