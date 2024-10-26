using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class detallepedidoInfo
    {
        [Key] public int id_detalle { get; set; }
        public int id_pedido { get; set; }
        public string nombre_producto { get; set; }
        public string proveedor { get; set; }
        public int cantidad { get; set; }
        public decimal descuento { get; set; }
        public DateTime fecha_pedido { get; set; }
        public string estado { get; set; }
    }
}
