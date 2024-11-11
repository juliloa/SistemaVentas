using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class productosInfo
    {
            [DisplayName("ID")]
            [Key] public int id_producto { get; set; }

            [DisplayName("Nombre de Producto")]
            public string nombre_producto { get; set; }

            [DisplayName("Categoría")]
            public string categoria { get; set; }

            [DisplayName("Proveedor")]
            public string proveedor { get; set; }

            [DisplayName("Unidades Disponibles")]
            public int unidades_stock { get; set; }

            [DisplayName("Precio")]
            public decimal precio { get; set; }

            [DisplayName("Activo")]
            public bool activo { get; set; }

            [DisplayName("Imagen URL")]
            public string imagen_url { get; set; }


    }
}
