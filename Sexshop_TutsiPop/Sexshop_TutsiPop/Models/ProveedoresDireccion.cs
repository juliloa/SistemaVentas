using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class ProveedoresDireccion
    {

        [DisplayName("ID")]
        [Key]
        public int id_proveedor { get; set; }


        [DisplayName("Empresa")]
        [Required]
        [StringLength(100)]
        public string nombre_empresa { get; set; }


        [DisplayName("Nombre Contacto")]
        public string nombre_contacto { get; set; }

        [DisplayName("Numero Contacto")]
        public string numero_contacto { get; set; }

        [DisplayName("Email")]
        public string correo { get; set; }

        [DisplayName("Pais")]
        public string pais { get; set; }

        [DisplayName("ciudad")]
        public string ciudad { get; set; }

        [DisplayName("Activo")]
        public bool activo { get; set; } = true;

        [DisplayName("Calle")]
        public string direccion_calle { get; set; }

        [DisplayName("Barrio")]
        public string barrio { get; set; }

        [DisplayName("Ciudad")]
        public string CiudadDireccion { get; set; } // Para evitar confusión con ciudad en proveedores

        [DisplayName("Codigo Postal")]
        public string codigo_postal { get; set; }
        
    }
}
