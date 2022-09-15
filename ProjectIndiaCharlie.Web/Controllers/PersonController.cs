using Microsoft.AspNetCore.Mvc;
using ProjectIndiaCharlie.Web.Service;

namespace ProjectIndiaCharlie.Web.Controllers
{
    public class PersonController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var subjectSectionDetailsList = PersonService.GetSubjectSectionDetailsAsync();
            return await subjectSectionDetailsList is null ?
                NotFound() :
                View(await subjectSectionDetailsList);
        }
    }
}
