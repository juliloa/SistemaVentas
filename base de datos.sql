-- BASE DE DATOS 

CREATE TABLE estado_pedido (
    id_estado SERIAL PRIMARY KEY,
    nombre_estado VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE categorias (
    id_categoria SERIAL PRIMARY KEY,
    nombre_categoria VARCHAR(100) NOT NULL,
    descripcion TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE metodos_pago (
    id_metodo SERIAL PRIMARY KEY,
    metodo_pago VARCHAR(100) NOT NULL UNIQUE,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE direcciones (
    id_direccion SERIAL PRIMARY KEY,
    direccion_calle VARCHAR(255) NOT NULL,
    barrio VARCHAR(100),
    ciudad VARCHAR(100) NOT NULL,
    codigo_postal VARCHAR(20),
    pais VARCHAR(100) NOT NULL
);

CREATE TABLE proveedores (
    id_proveedor SERIAL PRIMARY KEY,
    nombre_empresa VARCHAR(100) NOT NULL,
    nombre_contacto VARCHAR(100),
    numero_contacto VARCHAR(20),
    correo VARCHAR(100),
    pais VARCHAR(50),
    ciudad VARCHAR(50),
    id_direccion INT,
    activo BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (id_direccion) REFERENCES direcciones (id_direccion) ON DELETE SET NULL
);

CREATE TABLE productos (
    id_producto SERIAL PRIMARY KEY,
    nombre_producto VARCHAR(100) NOT NULL,
    id_categoria INT NOT NULL,
    id_proveedor INT NOT NULL,
    unidades_stock INT NOT NULL DEFAULT 0,
    precio DECIMAL(10,2) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    imagen_url Varchar (255),
    FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria) ON DELETE CASCADE,
    FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor) ON DELETE CASCADE
);

CREATE TABLE pedidos (
    id_pedido SERIAL PRIMARY KEY,
    id_proveedor INT NOT NULL,
    fecha_pedido DATE NOT NULL DEFAULT CURRENT_DATE,
    id_direccion INT NOT NULL,
    id_metodo_pago INT,
    id_estado INT,
    FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor),
    FOREIGN KEY (id_direccion) REFERENCES direcciones(id_direccion),
    FOREIGN KEY (id_metodo_pago) REFERENCES metodos_pago(id_metodo),
    FOREIGN KEY (id_estado) REFERENCES estado_pedido(id_estado)
);

CREATE TABLE detalle_pedido (
    id_detalle SERIAL PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    descuento DECIMAL(3, 2) DEFAULT 0.00,
    FOREIGN KEY (id_pedido) REFERENCES pedidos(id_pedido) ON DELETE CASCADE,
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto) ON DELETE CASCADE
);

CREATE TABLE clientes (
    cedula_cliente VARCHAR(20) PRIMARY KEY,
    apellido VARCHAR(100) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    correo VARCHAR(100) UNIQUE,
    numero_telefono VARCHAR(20),
    id_direccion INT,
    FOREIGN KEY (id_direccion) REFERENCES direcciones(id_direccion) ON DELETE SET NULL
);

CREATE TABLE empleados (
    cedula_empleado VARCHAR(20) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NOT NULL UNIQUE,
    numero_telefono VARCHAR(20),
    fecha_contratacion DATE NOT NULL DEFAULT CURRENT_DATE,
    rol VARCHAR(50) NOT NULL,
    salario DECIMAL(10, 2) NOT NULL,
    activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE ventas (
    id_venta SERIAL PRIMARY KEY,
    cedula_cliente VARCHAR(20) NOT NULL,
    cedula_empleado VARCHAR(20) NOT NULL,
    fecha_venta DATE NOT NULL DEFAULT CURRENT_DATE,
    id_metodo_pago INT,
    FOREIGN KEY (id_metodo_pago) REFERENCES metodos_pago(id_metodo),
    FOREIGN KEY (cedula_cliente) REFERENCES clientes(cedula_cliente),
    FOREIGN KEY (cedula_empleado) REFERENCES empleados(cedula_empleado)
);

CREATE TABLE detalle_venta (
    id_detalle SERIAL PRIMARY KEY,
    id_venta INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    descuento DECIMAL(3, 2) DEFAULT 0.00,
    FOREIGN KEY (id_venta) REFERENCES ventas(id_venta) ON DELETE CASCADE,
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto) ON DELETE CASCADE
);

CREATE TABLE entregas (
    id_entrega SERIAL PRIMARY KEY,
    id_pedido INT NOT NULL,
    id_direccion_entrega INT NOT NULL,
    fecha_entrega DATE NOT NULL DEFAULT CURRENT_DATE,
    estado_entrega VARCHAR(50) NOT NULL,
    cedula_empleado VARCHAR(20),
    FOREIGN KEY (id_pedido) REFERENCES pedidos(id_pedido),
    FOREIGN KEY (id_direccion_entrega) REFERENCES direcciones(id_direccion),
    FOREIGN KEY (cedula_empleado) REFERENCES empleados(cedula_empleado) ON DELETE SET NULL
);

CREATE TABLE usuarios (
    id_usuario SERIAL PRIMARY KEY,
    nombre VARCHAR(50),
    email VARCHAR(50) UNIQUE,
    contrasenna VARCHAR(200),
    token VARCHAR(200),
    confirmado BOOLEAN DEFAULT FALSE,
    restablecer BOOLEAN DEFAULT FALSE,
    confirmar_contrasenna VARCHAR(200),
    rol VARCHAR(50) DEFAULT 'cliente'
);

CREATE TABLE carrito (
    carrito_id serial PRIMARY KEY,
    id_usuario INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    fecha TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    precio integer,
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario) ON DELETE CASCADE,
    FOREIGN KEY (id_producto) REFERENCES productos(id_producto) ON DELETE CASCADE
);


CREATE TABLE auditorias (
    id_auditoria SERIAL PRIMARY KEY,
    id_usuario INT NOT NULL,
    accion VARCHAR(50) NOT NULL,
    tabla_afectada VARCHAR(50) NOT NULL,
    id_registro_afectado INT NOT NULL,
    fecha TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ip_usuario VARCHAR(45),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

-- PROCEDIMIENTOS ALMACENADOS 

CREATE OR REPLACE PROCEDURE agregar_al_carrito(
    p_id_producto INT,
    p_id_usuario INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_stock INT;
    v_existe_carrito INT;
    v_precio DECIMAL;
    v_id_pedido INT;
BEGIN
    -- Obtener el stock disponible del producto
    SELECT unidades_stock INTO v_stock
    FROM productos
    WHERE id_producto = p_id_producto;

    -- Verificar si el producto tiene stock
    IF v_stock = 0 THEN
        RAISE EXCEPTION 'Producto sin stock disponible';
    END IF;

    -- Verificar si el producto ya está en el carrito (detalle_pedido)
    SELECT COUNT(*) INTO v_existe_carrito
    FROM carrito
    WHERE id_usuario = p_id_usuario AND id_producto = p_id_producto;

    IF v_existe_carrito > 0 THEN
        -- Si el producto ya está en el carrito, incrementar la cantidad
        UPDATE carrito
        SET cantidad = cantidad + 1
        WHERE id_usuario = p_id_usuario AND id_producto = p_id_producto;
    ELSE
        -- Si el producto no está en el carrito, agregarlo
        SELECT precio INTO v_precio
        FROM productos
        WHERE id_producto = p_id_producto;

        INSERT INTO carrito (id_usuario, id_producto, cantidad,fecha, precio)
        VALUES (p_id_usuario, p_id_producto, 1, CURRENT_TIMESTAMP, v_precio);
    END IF;

    -- Actualizar el stock del producto
    UPDATE productos
    SET unidades_stock = unidades_stock - 1
    WHERE id_producto = p_id_producto;

    COMMIT;
END;
$$;

CREATE OR REPLACE PROCEDURE eliminar_del_carrito(
    p_id_carrito INT
)
LANGUAGE plpgsql
AS $$
DECLARE
    v_id_producto INT;
    v_cantidad INT;
BEGIN
    -- Obtener el id del producto y la cantidad del carrito
    SELECT id_producto, cantidad INTO v_id_producto, v_cantidad
    FROM carrito
    WHERE carrito_id = p_id_carrito;

    -- Eliminar el producto del carrito (detalle_pedido)
    DELETE FROM carrito
    WHERE carrito_id = p_id_carrito;

    -- Actualizar el stock del producto
    UPDATE productos
    SET unidades_stock = unidades_stock + v_cantidad
    WHERE id_producto = v_id_producto;

    COMMIT;
END;
$$;

CREATE OR REPLACE PROCEDURE obtener_carrito_usuario(
    p_id_usuario INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Obtener todos los productos en el carrito del usuario
    SELECT
        c.carrito_id,
        p.nombre_producto AS nombre_producto,
        c.cantidad AS cantidad,
        c.precio AS precio,
        (c.cantidad * c.precio) AS total
    FROM carrito c
    JOIN productos p ON c.id_producto = p.id_producto
    WHERE c.id_usuario = p_id_usuario;
END;
$$;


-- DISPARADORES 