using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class detalleventaInfo
    {

        [Key]
        public int id_detalle { get; set; }
        public int id_venta { get; set; }
        public string nombre_producto { get; set; }
        public int cantidad { get; set; }
        public decimal descuento { get; set; }


    }
}
