using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class empleados
    {
        [Key]
        public string cedula_empleado { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string correo { get; set; }

        public string numero_telefono { get; set; }

        [Required]
        public DateTime fecha_contratacion { get; set; } = DateTime.Now;

        [Required]
        public string rol { get; set; }

        [Required]
        public decimal salario { get; set; }

        public bool activo { get; set; } = true;
    }
}
