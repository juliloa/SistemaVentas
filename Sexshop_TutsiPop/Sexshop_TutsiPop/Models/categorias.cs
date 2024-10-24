using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class categorias
    {
        [Key]
        public int id_categoria { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre_categoria { get; set; }

        public string descripcion { get; set; }

        [Required]
        public bool activo { get; set; } = true;
    }
}
