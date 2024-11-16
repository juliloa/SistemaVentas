using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Sexshop_TutsiPop.Models
{
    public class Proveedores
    {
        [DisplayName("ID")]
        [Key]
        public int id_proveedor { get; set; }

        [DisplayName("Empresa")]
        [Required]
        [StringLength(100)]
        public string nombre_empresa { get; set; }

        [DisplayName("Contacto")]
        public string nombre_contacto { get; set; }

        [DisplayName("Numero")]
        public string numero_contacto { get; set; }

        [DisplayName("Email")]
        public string correo { get; set; }

        [DisplayName("Pais")]
        public string pais { get; set; }

        [DisplayName("Ciudad")]
        public string ciudad { get; set; }

        [DisplayName("Direccion")]
        public int? id_direccion { get; set; }

        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

       
    }
}
