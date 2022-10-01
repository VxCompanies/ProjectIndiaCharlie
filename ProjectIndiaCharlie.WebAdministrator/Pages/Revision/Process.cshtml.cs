using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text.Json;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.Revision
{
    public class ProcessModel : PageModel
    {
        
        public VGradeRevision UnresolverRevision { get; set; }

        [BindProperty]
        public VGradeRevisionVM RevisionVM { get; set; }


        public SelectList Grades { get; set; }
        public List<SelectListItem> Grades2 { get; set; }


        [BindProperty]
        public int ModifiedGradeIDVM { get; set; }

        public List<SelectListItem> BaremeList;

        public async Task<IActionResult> OnGetAsync(int? personID, int? subjectDetailId)
        {
            //Grades = (List<VGrade>)Program.Grades.ToList();
            //Grades = (IEnumerable<SelectListItem>)Program.Grades;
            var grades = await StudentService.GetGrades();
            //= await StudentService.GetGrades();
            ViewData["Numbers"] = grades
                .Select(n => new SelectListItem
                {
                    Value = n.GradeId.ToString(),
                    Text = n.Grade.ToString()
                }).ToList();

            this.Grades2 = grades
                .Select(n => new SelectListItem
                {
                    Value = n.GradeId.ToString(),
                    Text = n.Grade.ToString()
                }).ToList();

            this.Grades =  new SelectList(grades, "GradeId", "Grade"); //, nameof(Program.Grades), nameof(grades.GradeID));



            var a = await StudentService.GetUnresolvedRevisions();

            //VGradeRevision revision =
            //    a.Where((x) => x.PersonId == personID & x.SubjectDetailId == subjectDetailId).ElementAt(0);

            VGradeRevision revision =
                a.First((x) => x.PersonId == personID & x.SubjectDetailId == subjectDetailId);

            UnresolverRevision = revision;

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            RevisionVM.AdminID = 1112200;
            var perosnId = Request.Query["PersonId"];
            var subjectDetailId = Request.Query["SubjectDetailId"];

            var grade = RevisionVM.ModifiedGrade;

            var revision = new VGradeRevision()
            {
                PersonId = int.Parse(perosnId),
                SubjectDetailId = int.Parse(subjectDetailId),
                ModifiedGradeId = grade,
                Admin = RevisionVM.AdminID,
                Professor = 1,
                GradeId = 1,
                DateRequested = DateTime.Now,
                Grade ="a",
                ModifiedGrade = grade,
                Section = "1",
                Student = "sd"
            };



            var a = await StudentService.ProcessRevision(revision);

            if (a == null)
            {
                return NotFound();

            }
            //var userData = new CreatedUser()
            //{
            //    UserID = a.PersonId,
            //    Password = a.Password
            //};

            //TempData["createdPerson"] = JsonSerializer.Serialize(userData);

            //return RedirectToPage("/RegisterUser/Created");
            return RedirectToPage("/Index");
        }

        public partial class VGradeRevisionVM
        {
            public int PersonId { get; set; }
            public int SubjectDetailId { get; set; }
            public int ModifiedGrade { get; set; }
            public int AdminID { get; set; }
        }
    }
}
