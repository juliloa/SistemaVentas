using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class clientes
    {
        [Key]
        public string cedula_cliente { get; set; }

        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        public string correo { get; set; }

        public string numero_telefono { get; set; }

        public int? id_direccion { get; set; }

       
    }
}
