using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class categorias
    {
        [DisplayName("ID")]
        [Key]
        public int id_categoria { get; set; }

        [DisplayName("Categoría")]
        [Required]
        [StringLength(100)]
        public string nombre_categoria { get; set; }

        [DisplayName("Descripcion")]
        public string descripcion { get; set; }

        [DisplayName("Activo")]
        [Required]
        public bool activo { get; set; } = true;
    }
}
