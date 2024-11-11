using Humanizer;
using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class carrito
    {
        [Key]
        public int carrito_id { get; set; } 
        public int id_usuario { get; set; }
        public int id_producto { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }

        [Required]
        public DateTime fecha { get; set; } = DateTime.Now;

    }
}
