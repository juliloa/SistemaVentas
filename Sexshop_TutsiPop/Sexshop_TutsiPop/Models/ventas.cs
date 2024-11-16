using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sexshop_TutsiPop.Models
{
    public class ventas
    {
        [DisplayName("ID")]
        [Key]
        public int id_venta { get; set; }

        [DisplayName("Cliente")]
        [Required]
        public string cedula_cliente { get; set; }

        [DisplayName("Empleado")]
        [Required]
        public string cedula_empleado { get; set; }

        [DisplayName("Fecha")]
        [Required]
        public DateTime fecha_venta { get; set; } = DateTime.Now;

        [DisplayName("Metodo de pago")]
        public int? id_metodo_pago { get; set; }


    }
}
