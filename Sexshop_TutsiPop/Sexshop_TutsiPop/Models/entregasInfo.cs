using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class entregasInfo
    {
        [Key] public int id_entrega { get; set; }
        public int id_pedido { get; set; }
        public string direccion_entrega { get; set; }
        public string ciudad_entrega { get; set; }
        public string barrio_entrega { get; set; }
        public string nombre_empleado { get; set; }
        public DateTime fecha_entrega { get; set; }
        public string estado_entrega { get; set; }
    }
}
