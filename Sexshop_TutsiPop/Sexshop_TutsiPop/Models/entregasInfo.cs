using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class entregasInfo
    {
        [DisplayName("ID")]
        [Key] public int id_entrega { get; set; }
        [DisplayName("ID Pedido")]
        public int id_pedido { get; set; }
        [DisplayName("Direccion")]
        public string direccion_entrega { get; set; }

        [DisplayName("Ciudad")]
        public string ciudad_entrega { get; set; }

        [DisplayName("Barrio")]
        public string barrio_entrega { get; set; }

        [DisplayName("Empleado")]
        public string nombre_empleado { get; set; }

        [DisplayName("Fecha")]
        public DateTime fecha_entrega { get; set; }

        [DisplayName("Estado")]
        public string estado_entrega { get; set; }
    }
}
