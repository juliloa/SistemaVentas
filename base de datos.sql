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
    confirmar_contrasenna VARCHAR(200)
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
