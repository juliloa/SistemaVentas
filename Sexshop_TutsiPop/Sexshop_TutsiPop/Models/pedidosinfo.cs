using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class pedidosinfo
    {
        [DisplayName("ID")]
        [Key] public int IdPedido { get; set; }

        [DisplayName("Proveedor")]
        public string NombreProveedor { get; set; }

        [DisplayName("Estado Pediddo")]
        public string EstadoPedido { get; set; }

        [DisplayName("Metodo de Pago")]
        public string MetodoPago { get; set; }

        [DisplayName("Direccion")]
        public string DireccionEntrega { get; set; }
        public string Barrio { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }

        [DisplayName("Fecha")]
        public DateTime FechaPedido { get; set; }
    }
}
