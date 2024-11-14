using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class entregas
    {

        [Key]
        public int id_entrega { get; set; }

        public int id_pedido { get; set; }

        public int id_direccion_entrega { get; set; }

        [Required]
        public DateTime fecha_entrega { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string estado_entrega { get; set; }

        public string cedula_empleado { get; set; }


    }
}
