using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class Proveedores
    {
        [Key]
        public int id_proveedor { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_empresa { get; set; }

        public string nombre_contacto { get; set; }

        public string numero_contacto { get; set; }

        public string correo { get; set; }

        public string pais { get; set; }

        public string ciudad { get; set; }

        public int? id_direccion { get; set; }

        public bool activo { get; set; } = true;

       
    }
}
