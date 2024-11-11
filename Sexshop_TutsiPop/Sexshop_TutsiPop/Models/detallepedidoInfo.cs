using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class detallepedidoInfo
    {
        [DisplayName("ID")]
        [Key] public int id_detalle { get; set; }

        [DisplayName("ID Pedido")]
        public int id_pedido { get; set; }

        [DisplayName("Producto")]
        public string nombre_producto { get; set; }

        [DisplayName("Proveedor")]
        public string proveedor { get; set; }

        [DisplayName("Cantidad")]
        public int cantidad { get; set; }

        [DisplayName("Descuento")]
        public decimal descuento { get; set; }

        [DisplayName("Fecha")]
        public DateTime fecha_pedido { get; set; }

        [DisplayName("Estado")]
        public string estado { get; set; }
    }
}
