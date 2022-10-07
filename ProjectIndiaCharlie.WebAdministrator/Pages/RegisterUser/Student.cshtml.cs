using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectIndiaCharlie.WebAdministrator.Models;
using ProjectIndiaCharlie.WebAdministrator.Service;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.RegisterUser
{
    public class RegisterUserModel : PageModel
    {

        [BindProperty]
        public RegisterUserViewModel RegisterUserVM { get; set; }

        public SelectList Careers { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            var careerId = int.Parse(Request.Form["CustomerId"]);

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
                CareerId = careerId
            };


            if (!ModelState.IsValid)
            {
                return Page();
            }

            var a = await StudentService.RegisterPerson(student);

            var userData = new CreatedUser()
            {
                UserID = a.PersonId,
                Password = a.Password
            };

            TempData["createdPerson"] = JsonSerializer.Serialize(userData);

            return RedirectToPage("/RegisterUser/Created");
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var carreras = await CareerService.GetCareers();

            this.Careers = new SelectList(carreras, "CareerId", "Name"); //CompleteName

            if (Careers == null)
            {
                return NotFound();
            }

            return Page();
        }


        public class RegisterUserViewModel
        {
            [Required]
            [EmailAddress(ErrorMessage = "Invalid email adress")]
            public string Email { get; set; }


            [Required]
            [Display(Name = "Numero de documento")]
            [MaxLength(13)]
            public string DocNo { get; set; }

            [Required]
            [Display(Name = "Primer nombre")]
            public string firstName { get; set; }


            [Display(Name = "Segundo nombre")]
            public string? middleName { get; set; }

            [Required]
            [Display(Name = "Primer apellido")]
            public string firstSurname { get; set; }


            [Display(Name = "Segundo apellido")]
            public string? secondSurname { get; set; }

            [Required]
            [Display(Name = "Genero")]
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

        }
    }
}
