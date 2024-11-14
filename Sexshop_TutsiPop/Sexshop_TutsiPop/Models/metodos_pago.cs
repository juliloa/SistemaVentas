using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class metodos_pago
    {

        [DisplayName("ID")]
        [Key]
        public int id_metodo { get; set; }


        [DisplayName("Metodo de Pago")]
        [Required]
        [StringLength(100)]
        public string metodo_pago { get; set; }


        [DisplayName("Activo")]
        public bool activo { get; set; } = true;
    }
}
