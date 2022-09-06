using Microsoft.AspNetCore.Mvc;
using ProjectIndiaCharlie.Web.Service;

namespace ProjectIndiaCharlie.Web.Controllers
{
    public class PersonController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var peopleList = await PersonService.GetPeopleAsync();
            return peopleList is null ?
                NotFound() :
                View(peopleList);
        }
    }
}
