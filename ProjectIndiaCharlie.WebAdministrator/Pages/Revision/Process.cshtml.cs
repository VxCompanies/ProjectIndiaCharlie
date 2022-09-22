using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
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

        //public ProcessModel()
        //{
        //    UnresolverRevision = new VGradeRevision();
        //}


        [BindProperty]
        public int ModifiedGradeIDVM { get; set; }

        public async Task<IActionResult> OnGetAsync(int? personID, int? subjectDetailId)
        {
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

            VGradeRevision revision = new VGradeRevision()
            {
                PersonId = int.Parse(perosnId),
                SubjectDetailId = int.Parse(subjectDetailId),
                ModifiedGradeId = RevisionVM.ModifiedGradeId,
                Admin = RevisionVM.AdminID,
                Professor = 1,
                GradeId = 1,
                DateRequested = DateTime.Now,
                Grade ="a",
                ModifiedGrade = "a",
                Section = "1",
                Student = "sd"
            };

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

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
            public int ModifiedGradeId { get; set; }
            public int AdminID { get; set; }
        }
    }
}
