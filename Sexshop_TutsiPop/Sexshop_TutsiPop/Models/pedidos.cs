using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class pedidos
    {
        [Key]
        public int id_pedido { get; set; }

        public int id_proveedor { get; set; }

        [Required]
        public DateTime fecha_pedido { get; set; } = DateTime.Now;

        public int id_direccion { get; set; }

        public int? id_metodo_pago { get; set; }

        public int? id_estado { get; set; }


    }
}
