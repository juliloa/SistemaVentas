using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class metodos_pago
    {
        [Key]
        public int id_metodo { get; set; }

        [Required]
        [StringLength(100)]
        public string metodo_pago { get; set; }

        public bool activo { get; set; } = true;
    }
}
