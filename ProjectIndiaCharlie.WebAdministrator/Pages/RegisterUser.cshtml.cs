using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
using System.ComponentModel.DataAnnotations;

namespace ProjectIndiaCharlie.WebAdministrator.Pages
{
    public class RegisterUserModel : PageModel
    {

        [BindProperty]
        public RegisterUserViewModel RegisterUserVM { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            var student = new NewPerson()
            {
                DocNo = RegisterUserVM.DocNo,
                FirstName = RegisterUserVM.firstName,
                MiddleName = RegisterUserVM.middleName,
                FirstSurname = RegisterUserVM.firstSurname,
                SecondSurname = RegisterUserVM.secondSurname,
                Gender = RegisterUserVM.gender,
                BirthDate = RegisterUserVM.birthDate,
                Email = RegisterUserVM.email,
                CareerId = RegisterUserVM.careerId
            };


            var a = StudentService.RegisterPerson(student);

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
            public bool RememberMe { get; set; }

            [Required]
            [Display(Name = "Numero de documento")]
            public string DocNo { get; set; }

            [Required]
            [Display(Name = "Primer nombre")]
            public string firstName { get; set; }

            
            [Display(Name = "Segundo nombre")]
            public string middleName { get; set; }

            [Required]
            [Display(Name = "Primer apellido")]
            public string firstSurname { get; set; }

            
            [Display(Name = "Second surname")]
            public string secondSurname { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string gender { get; set; }

            [Required]
            [Display(Name = "Fecha de nacimiento")]
            public DateTime birthDate { get; set; }

            [Required]
            [Display(Name = "Correo")]
            public string email { get; set; }

            [Required]
            [Display(Name = "Carera")]
            public int careerId { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
