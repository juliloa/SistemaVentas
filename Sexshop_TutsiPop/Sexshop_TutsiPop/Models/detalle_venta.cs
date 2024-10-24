using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class detalle_venta
    {
        [Key]
        public int id_detalle { get; set; }

        public int id_venta { get; set; }

        public int id_producto { get; set; }

        public int cantidad { get; set; }

        public decimal descuento { get; set; } = 0.00M;

    }
}
