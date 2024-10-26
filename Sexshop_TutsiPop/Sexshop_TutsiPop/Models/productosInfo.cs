using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class productosInfo
    {

            [Key] public int id_producto { get; set; }
            public string nombre_producto { get; set; }
            public string categoria { get; set; }
            public string proveedor { get; set; }
            public int unidades_stock { get; set; }
            public decimal precio { get; set; }
            public bool activo { get; set; }
        

    }
}
