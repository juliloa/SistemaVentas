using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class estado_pedido
    {
        [Key]
        public int id_estado { get; set; }

        [Required]
        [StringLength(50)]
        public string nombre_estado { get; set; }
    }
}
