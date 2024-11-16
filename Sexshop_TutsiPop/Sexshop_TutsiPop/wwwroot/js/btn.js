//$('.add-to-cart-btn').click(function (e) {
//    console.log("Botón clickeado"); // Para verificar si la función se está ejecutando
//    e.preventDefault(); // Evitar el envío automático del formulario

//    const form = $(this).closest("form"); // Obtener el formulario al que pertenece el botón
//    const productoId = form.find("input[name='id_producto']").val(); // Obtener el ID del producto desde el formulario
//    const stockDisponible = parseInt($(this).attr('data-stock'));  // Obtener el stock disponible del producto desde el atributo 'data-stock'
//    const stockCarrito = parseInt($('#cantidad-' + productoId).val()) || 0; // Obtener la cantidad en el carrito (si existe)

//    // Verificar si el producto está agotado
//    if (stockDisponible <= 0) {
//        Swal.fire({
//            position: "center",
//            icon: "error",
//            title: "Producto agotado",
//            showConfirmButton: false,
//            timer: 1500
//        });
//        return; // No enviar el formulario si el producto está agotado
//    }

//    // Mostrar el mensaje de éxito si hay stock disponible
//    Swal.fire({
//        position: "center",
//        icon: "success",
//        title: "Producto agregado al carrito",
//        showConfirmButton: false,
//        timer: 1500
  
//    });
//});
document.addEventListener("DOMContentLoaded", () => {
    const botonesAgregar = document.querySelectorAll(".add-to-cart-btn");

    botonesAgregar.forEach(boton => {
        boton.addEventListener("click", function (event) {
            const stock = parseInt(this.dataset.stock);

            if (stock <= 0) {
                event.preventDefault(); 
                Swal.fire({
                    icon: "error",
                    title: "Producto agotado",
                    text: "Lo sentimos, este producto no está disponible.",
                });
            } else {
               
                Swal.fire({
                    icon: "success",
                    title: "Producto agregado",
                    text: "El producto se ha añadido al carrito.",
                    showConfirmButton: false,
                    timer: 1500,
                });

               
            }
        });
    });
});
