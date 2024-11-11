using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class direcciones
    {
        [DisplayName("ID")]
        [Key]
        public int id_direccion { get; set; }

        [DisplayName("Calle")]
        [Required]
        [StringLength(255)]
        public string direccion_calle { get; set; }

        [DisplayName("Barrio")]
        public string barrio { get; set; }

        [DisplayName("Ciudad")]
        [Required]
        public string ciudad { get; set; }

        [DisplayName("Codigo Postal")]
        public string codigo_postal { get; set; }

        [DisplayName("Pais")]
        [Required]
        public string pais { get; set; }
    }
}
