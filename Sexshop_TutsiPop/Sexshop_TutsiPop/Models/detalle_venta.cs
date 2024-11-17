using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sexshop_TutsiPop.Models
{
    public class detalle_venta
    {

        [DisplayName("ID")]
        [Key]
        public int id_detalle { get; set; }


        [DisplayName("ID Venta")]
        public int id_venta { get; set; }


        [DisplayName("Producto")]
        public int id_producto { get; set; }


        [DisplayName("Cantidad")]
        public int cantidad { get; set; }

        [DisplayName("Descuento")]
        public decimal descuento { get; set; } = 0.00M;

    }
}
