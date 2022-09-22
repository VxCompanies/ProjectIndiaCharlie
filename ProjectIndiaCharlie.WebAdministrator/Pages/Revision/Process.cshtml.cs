using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
using System.Data.SqlTypes;
using System.Linq;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.Revision
{
    public class ProcessModel : PageModel
    {
        public VGradeRevision UnresolverRevision { get; set; }

        public ProcessModel()
        {
            UnresolverRevision = new VGradeRevision();
        }

        public async Task<IActionResult> OnGetAsync(int? personID, int? subjectDetailId)
        {
            var a = await StudentService.GetUnresolvedRevisions();

            VGradeRevision revision =
                a.Where((x) => x.PersonId == personID & x.SubjectDetailId == subjectDetailId).ElementAt(0);

            UnresolverRevision = revision;

            if (UnresolverRevision == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
