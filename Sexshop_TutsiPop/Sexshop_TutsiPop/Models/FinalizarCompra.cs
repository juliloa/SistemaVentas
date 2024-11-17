namespace Sexshop_TutsiPop.Models
{
    public class FinalizarCompraViewModel
    {
        public List<carrito> Carrito { get; set; }
        public clientes Cliente { get; set; }
        public decimal TotalCompra { get; set; }
    }

}
