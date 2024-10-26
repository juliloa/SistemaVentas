using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class pedidosinfo
    {
        [Key] public int IdPedido { get; set; }
        public string NombreProveedor { get; set; }
        public string EstadoPedido { get; set; }
        public string MetodoPago { get; set; }
        public string DireccionEntrega { get; set; }
        public string Barrio { get; set; }
        public string Ciudad { get; set; }
        public string CodigoPostal { get; set; }
        public string Pais { get; set; }
        public DateTime FechaPedido { get; set; }
    }
}
