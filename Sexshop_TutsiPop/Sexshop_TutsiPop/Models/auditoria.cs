using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class auditoria
    {
        [Key]
        public int pkauditoria { get; set; }

        [Required]
        [MaxLength(20)]
        public string nombreauditoria { get; set; }

        [Required]
        [MaxLength(20)]
        public string tipooperacion { get; set; }

        public string datosantiguos { get; set; }

        public string datosnuevos { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        [MaxLength(20)]
        public string usuario { get; set; }
    }
}
