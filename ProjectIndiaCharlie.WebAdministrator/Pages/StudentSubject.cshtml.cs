using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;

namespace ProjectIndiaCharlie.WebAdministrator.Pages
{
    public class StudentSubjectModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int ? IdSearched { get; set; }

        public IEnumerable<VStudentSubject> StudentSubjectVM { get; set; }
        
        public StudentSubjectModel()
        {
            StudentSubjectVM = new List<VStudentSubject>();
        }

        public async Task OnGet()
        {
            if (IdSearched != null)
            {
                var a = await StudentService.GetSchedule((int)IdSearched);

                StudentSubjectVM = a;
            }               
        }
    }
}
