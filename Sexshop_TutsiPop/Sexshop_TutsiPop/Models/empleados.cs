using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class empleados
    {

        [DisplayName("Cedula")]
        [Key]
        public string cedula_empleado { get; set; }


        [DisplayName("Nombre")]
        [Required]
        [StringLength(100)]
        public string nombre { get; set; }


        [DisplayName("Apellidos")]
        [Required]
        [StringLength(100)]
        public string apellido { get; set; }


        [DisplayName("Email")]
        [Required]
        [StringLength(100)]
        public string correo { get; set; }


        [DisplayName("Telefono")]
        public string numero_telefono { get; set; }


        [DisplayName("Fecha de Contrato")]
        [Required]
        public DateTime fecha_contratacion { get; set; } = DateTime.Now;


        [DisplayName("Rol")]
        [Required]
        public string rol { get; set; }


        [DisplayName("Salario")]
        [Required]
        public decimal salario { get; set; }


        [DisplayName("Activo")]
        public bool activo { get; set; } = true;
    }
}
