$('.add-to-cart-btn').click(function (e) {
    e.preventDefault(); // Evitar el envío automático del formulario

    const form = $(this).closest("form"); // Obtener el formulario al que pertenece el botón

    Swal.fire({
        position: "center",
        icon: "success",
        title: "Producto agregado al carrito",
        showConfirmButton: false,
        timer: 1500
    }).then(() => {
        form.submit(); // Enviar el formulario después de que la alerta desaparezca
    });
});
