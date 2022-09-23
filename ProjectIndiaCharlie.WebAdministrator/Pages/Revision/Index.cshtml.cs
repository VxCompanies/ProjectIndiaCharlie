using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.Revision
{
    public class IndexModel : PageModel
    {
        public IEnumerable<VGradeRevision> UnresolverRevision { get; set; }

        public IndexModel()
        {
            UnresolverRevision = new List<VGradeRevision>();
        }

        public async Task OnGet()
        {
            var a = await StudentService.GetUnresolvedRevisions();
            UnresolverRevision = a;
        }
    }
}
