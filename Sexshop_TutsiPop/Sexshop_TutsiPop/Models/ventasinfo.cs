using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class ventasinfo
    {
        [DisplayName("ID")]
        [Key]
        public int id_venta { get; set; }

        [DisplayName("Cedúla Cliente")]
        [Required]
        public string cedula_cliente { get; set; }

        [DisplayName("Cedúla Empleado")]
        [Required]
        public string cedula_empleado { get; set; }

        [DisplayName("Fecha")]
        [Required]
        public DateTime fecha_venta { get; set; } = DateTime.Now;

        [DisplayName("Metodo de Pago")]
        public string metodo_pago { get; set; }
    }
}
