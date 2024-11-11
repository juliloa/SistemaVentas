using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class detalleventaInfo
    {
        [DisplayName("ID")]
        [Key]
        public int id_detalle { get; set; }

        [DisplayName("ID Venta")]
        public int id_venta { get; set; }

        [DisplayName("Productos")]
        public string nombre_producto { get; set; }

        [DisplayName("Cantidad")]
        public int cantidad { get; set; }

        [DisplayName("Descuento")]
        public decimal descuento { get; set; }


    }
}
