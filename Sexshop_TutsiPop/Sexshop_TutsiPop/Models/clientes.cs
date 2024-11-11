using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sexshop_TutsiPop.Models
{
    public class clientes
    {
        [DisplayName("Cedula")]
        [Key]
        public string cedula_cliente { get; set; }

        [DisplayName("Apellidos")]
        [Required]
        [StringLength(100)]
        public string apellido { get; set; }

        [DisplayName("Nombre")]
        [Required]
        [StringLength(100)]
        public string nombre { get; set; }

        [DisplayName("Email")]
        public string correo { get; set; }

        [DisplayName("Telefono")]

        public string numero_telefono { get; set; }

        [DisplayName("Direccion")]
        public int? id_direccion { get; set; }

       
    }
}
