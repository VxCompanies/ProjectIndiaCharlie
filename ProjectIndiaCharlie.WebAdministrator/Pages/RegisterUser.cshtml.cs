using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
using System.ComponentModel.DataAnnotations;

namespace ProjectIndiaCharlie.WebAdministrator.Pages
{
    public class RegisterUserModel : PageModel
    {

        public async Task<IActionResult> OnPostAsync()
        {
            Student student = new Student()
            {
                
            };

            student.PersonId = 1;

            var a = PersonService.RegisterPerson(student);

            if (!ModelState.IsValid)
            {
                return Page();
            }


            return RedirectToPage("./Index");
        }
        public void OnGet()
        {
        }

        public class RegisterUserViewModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public bool RememberMe { get; set; }
        }
    }
}
