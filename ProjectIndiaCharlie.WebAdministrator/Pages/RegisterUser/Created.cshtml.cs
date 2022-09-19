using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIndiaCharlie.WebAdministrator.Models;
using System.Text.Json;

namespace ProjectIndiaCharlie.WebAdministrator.Pages.RegisterUser
{
    public partial class CreatedModel : PageModel
    {
        public CreatedUser CreatedUserVM { get; set; }

        public CreatedModel()
        {
            CreatedUserVM = new CreatedUser();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var data = TempData["createdPerson"] as string;
            try
            {
                var a = JsonSerializer.Deserialize<CreatedUser>(data);

                if (a != null)
                    this.CreatedUserVM = a;
            }
            catch (Exception ex)
            {
                this.CreatedUserVM.UserID = 1;
                this.CreatedUserVM.Password = "not exists";
            }


            if (CreatedUserVM == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
