using Microsoft.AspNetCore.Mvc;
using ProjectIndiaCharlie.Web.Service;

namespace ProjectIndiaCharlie.Web.Controllers
{
    public class ProfessorController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var subjectSectionDetailsList = PersonService.GetSubjectSections();
            return await subjectSectionDetailsList is null ?
                NotFound() :
                View(await subjectSectionDetailsList);
        }
    }
}
