using System.ComponentModel.DataAnnotations;

namespace ProjectIndiaCharlie.WebAdministrator.Models
{
    public class NewCareer
    {
        [Required]
        [Display(Name = "Nombre de carrera")]
        public string Name { get; set; } = null!;

        [Required]
        [Display(Name = "Código de carrera")]
        public string Code { get; set; } = null!;

        [Required]
        [Display(Name = "Cantidad de subjetos")]
        public int Subjects { get; set; }

        [Required]
        [Display(Name = "Cantidad de creditos")]
        public int Credits { get; set; }

        [Required]
        [Display(Name = "Año de carrera")]
        public int Year { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
