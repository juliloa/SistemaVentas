using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class productos
    {
        [Key]
        public int id_producto { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_producto { get; set; }

        public int id_categoria { get; set; }

        public int id_proveedor { get; set; }

        [Required]
        public int unidades_stock { get; set; } = 0;

        [Required]
        public decimal precio { get; set; }

        [Required]
        public bool activo { get; set; } = true;

    }
}
