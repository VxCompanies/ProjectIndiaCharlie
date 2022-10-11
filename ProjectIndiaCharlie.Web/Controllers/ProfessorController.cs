using Microsoft.AspNetCore.Mvc;
using ProjectIndiaCharlie.Web.Models;
using ProjectIndiaCharlie.Web.Service;

namespace ProjectIndiaCharlie.Web.Controllers;

public class ProfessorController : Controller
{
    public async Task<IActionResult> Login(LoginToken loginToken)
    {
        var loginResponse = PersonService.Login(loginToken);
        return !await loginResponse ?
            NotFound() :
            RedirectToAction(nameof(HomeController.Index), "HomeController");
    }
    
    public async Task<IActionResult> Index()
    {
        var subjectSectionDetailsList = PersonService.GetSubjectSections();
        return !(await subjectSectionDetailsList).Any() ?
            NotFound() :
            View(await subjectSectionDetailsList);
    }

    public async Task<IActionResult> StudentsList(int subjectDetailId)
    {
        var students = PersonService.GetStudentsOfSubjects(subjectDetailId);

        return (await students).Any() ?
            View((await students).OrderBy(s => s.StudentName)) :
            NotFound("");
    }
    
    public async Task<IActionResult> PublishGrade(int subjectDetailId, int studentId)
    {
        var student = (await PersonService.GetStudentsOfSubjects(subjectDetailId))
            .FirstOrDefault(s => s.StudentId == studentId);

        return View(new GradeDetail(student!));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PublishGrade(GradeDetail gradeDetail)
    {
        var response = PersonService.PublishGrade(gradeDetail);

        if (gradeDetail.Grade > 100)
        {
            ModelState.AddModelError("Grade", "Grade cannot be grater than 100 points.");
            return View(gradeDetail);
        }
        
        if (gradeDetail.Grade < 0)
        {
            ModelState.AddModelError("Grade", "Grade cannot be a negative value.");
            return View(gradeDetail);
        }

        if ((await response).ToLower().Contains("student"))
            TempData["Error"] = await response;
        else
            TempData["Success"] = await response;
        
        return RedirectToAction(nameof(StudentsList), new { gradeDetail.SubjectDetailId });
    }
}
