using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.Career
{
    public class AddModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty]
        public NewCareer NewCareerVM{ get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            NewCareerVM.IsActive = true;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var a = await CareerService.RegisterCareer(NewCareerVM);


            return RedirectToPage("/Index");
        }


    }
}
