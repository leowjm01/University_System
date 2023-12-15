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
            var paginatedScoreResult = pagination(results, pageNum, pageSize);

            return View(paginatedScoreResult);
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

            await CheckCourseSelected(scoreResults.scoreResultId, scoreResults.studentId, scoreResults.courseId);

            if (ModelState.IsValid)
            {
               
                var getExamSelected = await CheckExamSelected(scoreResults.studentId, scoreResults.courseId);

                //add new score result
                await ScoreResultsService.Add(scoreResults, getExamSelected);
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
            await CheckCourseSelected(scoreResults.scoreResultId, scoreResults.studentId, scoreResults.courseId);

            if (id != scoreResults.scoreResultId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {    
                
                var result = await ScoreResultsService.GetById(id);

                //check update data with same student
               var isUpdate =  await CheckScoreResultsBySameSelect(scoreResults, result.First().studentId);

                if (isUpdate != true)
                {

                    //check the student before edit
                    
                    var getSelectExamBefore = await CheckExamSelected(result.First().studentId, scoreResults.courseId);
                    getSelectExamBefore = result.First().mark == null || result.First().mark  < 50 ? getSelectExamBefore - 1 : getSelectExamBefore;
                    getSelectExamBefore = getSelectExamBefore == -1 ? 0 : getSelectExamBefore;

                    //check the student edited already
                    var getExamSelectedNow = await CheckExamSelected(scoreResults.studentId, scoreResults.courseId);
                    getExamSelectedNow = scoreResults.mark == null || scoreResults.mark < 50 ? getExamSelectedNow + 1 : getExamSelectedNow;
                    getExamSelectedNow = getExamSelectedNow == getExamSelectedNow - 1 ? 0 : getExamSelectedNow;

                    //update the exam selected that the student before edit
                    await ScoreResultsService.UpdateExamSelected(result.First().studentId, getSelectExamBefore);

                    //update data and update the exam selected that student edited
                    await ScoreResultsService.Update(scoreResults, getExamSelectedNow);
                }
                return RedirectToAction(nameof(Index));
            }


            ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", scoreResults.courseId);
            ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", scoreResults.studentId);
            return View(scoreResults);
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
            return RedirectToAction(nameof(Index));
        }



        public async Task<bool> CheckScoreResultsBySameSelect(ScoreResults scoreResults, int studentIdNow)
        {

            // check the user is edit the same studentid and course
            if (scoreResults.studentId ==  studentIdNow)
            {
                //check the student edited already
                var getExamSelected = await CheckExamSelected(studentIdNow, scoreResults.courseId);

                // add or decrease the quantity of selected result
                getExamSelected = scoreResults.mark == null || scoreResults.mark < 50 ? getExamSelected + 1 : getExamSelected - 1;
                getExamSelected = getExamSelected == getExamSelected - 1 ? 0 : getExamSelected;

                await ScoreResultsService.Update(scoreResults, getExamSelected);
                return true;
            }
            return false;
        }


        //check quantity exam selected by student
        public async Task<int> CheckExamSelected(int studentId, int courseId) 
        {
            var getExamSelected = await ScoreResultsService.GetExamSelectedByStudentId(studentId);

            // check the select exam is more than 10 or not
            if (getExamSelected >= 10)
            {
                ViewData["courseId"] = new SelectList(await CoursesService.GetAll(), "courseId", "courseName", courseId);
                ViewData["studentId"] = new SelectList(await StudentsService.GetAll(), "studentId", "studentName", studentId);
                RedirectToAction(nameof(Create));
            }

            return getExamSelected;
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
