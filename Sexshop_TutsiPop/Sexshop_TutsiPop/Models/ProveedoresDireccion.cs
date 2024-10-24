using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class ProveedoresDireccion
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

        public string direccion_calle { get; set; }
        public string barrio { get; set; }
        public string CiudadDireccion { get; set; } // Para evitar confusión con ciudad en proveedores
        public string codigo_postal { get; set; }
        
    }
}
