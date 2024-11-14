//// Al cargar la página, evita mostrar el precio hasta que el usuario interactúe
//$(document).ready(function () {
//    // Aquí puedes recorrer los productos y asegurarte de que el precio no se muestre inicialmente.
//    $('#productos-seleccionados span[id^="precio-"]').each(function () {
//        $(this).hide();
//    });
//});

//function updateQuantity(carritoId, cambio) {
//    var cantidadElement = document.getElementById("cantidad-" + carritoId);
//    var precioUnitario = parseFloat(document.getElementById("precio-unitario-" + carritoId).innerText); // Obtiene el precio unitario
//    var nuevaCantidad = parseInt(cantidadElement.innerText) + cambio;

//    if (nuevaCantidad < 1) return; // Evita que la cantidad sea menor a 1

//    var nuevoPrecioTotal = nuevaCantidad * precioUnitario; // Calcula el nuevo precio total

//    // Muestra el precio en la interfaz
//    document.getElementById("precio-" + carritoId).innerText = "$" + nuevoPrecioTotal.toFixed(2);
//    document.getElementById("precio-" + carritoId).style.display = "inline"; // Muestra el precio solo cuando se actualiza

//    // Envía la actualización de la cantidad al servidor para ajustar el stock
//    var token = $('input[name="__RequestVerificationToken"]').val();

//    $.ajax({
//        url: '@Url.Action("UpdateQuantity", "carrito")',
//        type: 'POST',
//        data: {
//            carritoId: carritoId,
//            nuevaCantidad: nuevaCantidad,
//            __RequestVerificationToken: token
//        },
//        success: function (response) {
//            if (response.success) {
//                cantidadElement.innerText = nuevaCantidad;
//                document.getElementById("precio-" + carritoId).innerText = "$" + response.precio.toFixed(2);
//                // Actualizar stock si decides mostrarlo en la vista
//            } else {
//                alert("Error al actualizar la cantidad");
//            }
//        },
//        error: function () {
//            alert("Ocurrió un error al intentar actualizar la cantidad.");
//        }
//    });
//}