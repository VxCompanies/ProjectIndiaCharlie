using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;

namespace ProjectIndiaCharlie.WebAdministrator.Pages
{
    public class StudentSubjectModel : PageModel
    {
        
        public IEnumerable<VStudentSubject> StudentSubjectVM { get; set; }
        public StudentSubjectModel()
        {
            StudentSubjectVM = new List<VStudentSubject>();
        }
        public async Task OnGet()
        {
            var a  = await StudentService.GetSelectedSubjects(1110408);
            StudentSubjectVM = a;
        }
    }
}
