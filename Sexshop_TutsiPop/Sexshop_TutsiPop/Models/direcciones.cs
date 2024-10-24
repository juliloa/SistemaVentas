using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class direcciones
    {
        [Key]
        public int id_direccion { get; set; }

        [Required]
        [StringLength(255)]
        public string direccion_calle { get; set; }

        public string barrio { get; set; }

        [Required]
        public string ciudad { get; set; }

        public string codigo_postal { get; set; }

        [Required]
        public string pais { get; set; }
    }
}
