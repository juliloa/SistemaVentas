using System;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class ProductosCarritoInfo
    {
        [Key] public int CarritoId { get; set; }             // ID del carrito
        public string UsuarioNombre { get; set; }      // Nombre del usuario
        public string ProductoNombre { get; set; }     // Nombre del producto
        public int Cantidad { get; set; }  
        public decimal Precio { get; set; } // Cantidad de productos en el carrito
        public DateTime Fecha { get; set; }            // Fecha de la adición al carrito
    }
}

