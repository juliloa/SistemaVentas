using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sexshop_TutsiPop.Models
{
    public class ventas
    {
        internal readonly object Detalles;

        [DisplayName("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_venta { get; set; }

        [DisplayName("Cliente")]
        [Required]
        public string cedula_cliente { get; set; }

        [DisplayName("Empleado")]
        public string cedula_empleado { get; set; }

        [DisplayName("Fecha")]
        public DateTime fecha_venta { get; set; } = DateTime.Now;

        [DisplayName("Metodo de pago")]
        public int? id_metodo_pago { get; set; }


    }
}
