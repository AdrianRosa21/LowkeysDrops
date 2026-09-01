/* =============================================================
   LOWKEYS DROPS - BASE DE DATOS SQL SERVER
   Proyecto final MOD 3.3, 3.4, 3.5
   Motor: Microsoft SQL Server

   Incluye:
   - 10 tablas principales + Auditoria
   - PK, FK, UNIQUE, CHECK y DEFAULT
   - Índices
   - Vistas
   - Triggers de auditoría y validación
   - Procedimientos almacenados para el flujo real del negocio
   - Roles/permisos de base de datos
   - Registros iniciales de prueba

   IMPORTANTE:
   Ejecutar en una instancia de SQL Server con permisos para crear BD.
   Se recomienda ejecutar sobre una base nueva.
   ============================================================= */

/* =============================================================
   0. CREACIÓN DE LA BASE DE DATOS
   ============================================================= */
IF DB_ID('LowkeysDropsDB') IS NULL
BEGIN
    CREATE DATABASE LowkeysDropsDB;
END;
GO

USE LowkeysDropsDB;
GO

/* =============================================================
   1. TABLAS
   ============================================================= */

CREATE TABLE dbo.Usuario (
    IdUsuario               INT IDENTITY(1,1) PRIMARY KEY,
    Nombre                  NVARCHAR(100) NOT NULL,
    Correo                  NVARCHAR(150) NOT NULL,
    ContrasenaHash          NVARCHAR(255) NOT NULL,
    Telefono                VARCHAR(20) NOT NULL,
    DUI                     VARCHAR(10) NULL,
    FotoPerfilUrl           NVARCHAR(500) NULL,
    Rol                     VARCHAR(20) NOT NULL CONSTRAINT DF_Usuario_Rol DEFAULT ('CLIENTE'),
    RequierePagoAnticipado  BIT NOT NULL CONSTRAINT DF_Usuario_PagoAnticipado DEFAULT (0),
    Estado                  BIT NOT NULL CONSTRAINT DF_Usuario_Estado DEFAULT (1),
    FechaRegistro           DATETIME2 NOT NULL CONSTRAINT DF_Usuario_Fecha DEFAULT (SYSDATETIME()),

    CONSTRAINT UQ_Usuario_Correo UNIQUE (Correo),
    CONSTRAINT CK_Usuario_Rol CHECK (Rol IN ('CLIENTE','ADMIN','REPARTIDOR')),
    CONSTRAINT CK_Usuario_DUICliente CHECK (Rol <> 'CLIENTE' OR DUI IS NOT NULL)
);
GO

-- Evita DUI duplicado, pero permite NULL para ADMIN/REPARTIDOR si se desea.
CREATE UNIQUE INDEX UX_Usuario_DUI
ON dbo.Usuario(DUI)
WHERE DUI IS NOT NULL;
GO

CREATE TABLE dbo.Direccion (
    IdDireccion     INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario       INT NOT NULL,
    Tipo            VARCHAR(10) NOT NULL,
    Departamento    NVARCHAR(80) NOT NULL,
    Municipio       NVARCHAR(100) NOT NULL,
    DireccionTexto  NVARCHAR(300) NOT NULL,
    Referencia      NVARCHAR(300) NULL,
    FechaRegistro   DATETIME2 NOT NULL CONSTRAINT DF_Direccion_Fecha DEFAULT (SYSDATETIME()),

    CONSTRAINT FK_Direccion_Usuario FOREIGN KEY (IdUsuario)
        REFERENCES dbo.Usuario(IdUsuario),
    CONSTRAINT CK_Direccion_Tipo CHECK (Tipo IN ('CASA','TRABAJO')),
    CONSTRAINT UQ_Direccion_UsuarioTipo UNIQUE (IdUsuario, Tipo)
);
GO

CREATE TABLE dbo.Drops (
    IdDrop            INT IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(120) NOT NULL,
    Descripcion       NVARCHAR(500) NULL,
    FechaPublicacion  DATE NULL,
    Estado            VARCHAR(15) NOT NULL CONSTRAINT DF_Drops_Estado DEFAULT ('BORRADOR'),

    CONSTRAINT CK_Drops_Estado CHECK (Estado IN ('BORRADOR','PUBLICADO','CERRADO'))
);
GO

CREATE TABLE dbo.Categoria (
    IdCategoria  INT IDENTITY(1,1) PRIMARY KEY,
    Nombre       NVARCHAR(100) NOT NULL,
    Descripcion  NVARCHAR(300) NULL,
    Estado       BIT NOT NULL CONSTRAINT DF_Categoria_Estado DEFAULT (1),

    CONSTRAINT UQ_Categoria_Nombre UNIQUE (Nombre)
);
GO

CREATE TABLE dbo.Producto (
    IdProducto     INT IDENTITY(1,1) PRIMARY KEY,
    IdDrop         INT NOT NULL,
    IdCategoria    INT NOT NULL,
    Nombre         NVARCHAR(150) NOT NULL,
    Descripcion    NVARCHAR(700) NULL,
    Talla          NVARCHAR(30) NULL,
    Precio         DECIMAL(10,2) NOT NULL,
    ImagenUrl      NVARCHAR(500) NULL,
    EsUnico        BIT NOT NULL CONSTRAINT DF_Producto_EsUnico DEFAULT (1),
    Stock          INT NOT NULL CONSTRAINT DF_Producto_Stock DEFAULT (1),
    Estado         VARCHAR(15) NOT NULL CONSTRAINT DF_Producto_Estado DEFAULT ('DISPONIBLE'),
    FechaRegistro  DATETIME2 NOT NULL CONSTRAINT DF_Producto_Fecha DEFAULT (SYSDATETIME()),

    CONSTRAINT FK_Producto_Drop FOREIGN KEY (IdDrop)
        REFERENCES dbo.Drops(IdDrop),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria)
        REFERENCES dbo.Categoria(IdCategoria),
    CONSTRAINT CK_Producto_Precio CHECK (Precio > 0),
    CONSTRAINT CK_Producto_Stock CHECK (Stock >= 0),
    CONSTRAINT CK_Producto_UnicoStock CHECK (EsUnico = 0 OR Stock <= 1),
    CONSTRAINT CK_Producto_Estado CHECK (Estado IN ('DISPONIBLE','APARTADO','VENDIDO'))
);
GO

CREATE TABLE dbo.Pedido (
    IdPedido     INT IDENTITY(1,1) PRIMARY KEY,
    IdCliente    INT NOT NULL,
    IdDireccion  INT NOT NULL,
    FechaPedido  DATETIME2 NOT NULL CONSTRAINT DF_Pedido_Fecha DEFAULT (SYSDATETIME()),
    Subtotal     DECIMAL(10,2) NOT NULL CONSTRAINT DF_Pedido_Subtotal DEFAULT (0),
    CostoEnvio   DECIMAL(10,2) NOT NULL,
    Total        DECIMAL(10,2) NOT NULL CONSTRAINT DF_Pedido_Total DEFAULT (0),
    Estado       VARCHAR(30) NOT NULL CONSTRAINT DF_Pedido_Estado DEFAULT ('PENDIENTE'),

    CONSTRAINT FK_Pedido_Cliente FOREIGN KEY (IdCliente)
        REFERENCES dbo.Usuario(IdUsuario),
    CONSTRAINT FK_Pedido_Direccion FOREIGN KEY (IdDireccion)
        REFERENCES dbo.Direccion(IdDireccion),
    CONSTRAINT CK_Pedido_Subtotal CHECK (Subtotal >= 0),
    CONSTRAINT CK_Pedido_CostoEnvio CHECK (CostoEnvio IN (2.00, 5.00)),
    CONSTRAINT CK_Pedido_Total CHECK (Total >= 0),
    CONSTRAINT CK_Pedido_Estado CHECK (
        Estado IN ('PENDIENTE','EN_CAMINO','PENDIENTE_CONFIRMACION','ENTREGADO','FALLIDO')
    )
);
GO

CREATE TABLE dbo.DetallePedido (
    IdDetalle       INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido        INT NOT NULL,
    IdProducto      INT NOT NULL,
    Cantidad        INT NOT NULL,
    PrecioUnitario  DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_Detalle_Pedido FOREIGN KEY (IdPedido)
        REFERENCES dbo.Pedido(IdPedido),
    CONSTRAINT FK_Detalle_Producto FOREIGN KEY (IdProducto)
        REFERENCES dbo.Producto(IdProducto),
    CONSTRAINT CK_Detalle_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT CK_Detalle_Precio CHECK (PrecioUnitario > 0),
    CONSTRAINT UQ_Detalle_PedidoProducto UNIQUE (IdPedido, IdProducto)
);
GO

CREATE TABLE dbo.Pago (
    IdPago      INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido    INT NOT NULL,
    Metodo      VARCHAR(20) NOT NULL,
    Estado      VARCHAR(15) NOT NULL CONSTRAINT DF_Pago_Estado DEFAULT ('PENDIENTE'),
    Referencia  NVARCHAR(120) NULL,
    FechaPago   DATETIME2 NULL,

    CONSTRAINT FK_Pago_Pedido FOREIGN KEY (IdPedido)
        REFERENCES dbo.Pedido(IdPedido),
    CONSTRAINT UQ_Pago_Pedido UNIQUE (IdPedido),
    CONSTRAINT CK_Pago_Metodo CHECK (Metodo IN ('CONTRA_ENTREGA','TRANSFERENCIA','DEPOSITO')),
    CONSTRAINT CK_Pago_Estado CHECK (Estado IN ('PENDIENTE','VERIFICADO','RECHAZADO'))
);
GO

CREATE TABLE dbo.Entrega (
    IdEntrega          INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido           INT NOT NULL,
    IdRepartidor       INT NOT NULL,
    Estado             VARCHAR(30) NOT NULL CONSTRAINT DF_Entrega_Estado DEFAULT ('PENDIENTE'),
    FechaTomado        DATETIME2 NOT NULL CONSTRAINT DF_Entrega_FechaTomado DEFAULT (SYSDATETIME()),
    FechaEntrega       DATETIME2 NULL,
    FotoEntregaUrl     NVARCHAR(500) NULL,
    Observacion        NVARCHAR(500) NULL,
    ConfirmadoCliente  BIT NOT NULL CONSTRAINT DF_Entrega_Confirmado DEFAULT (0),

    CONSTRAINT FK_Entrega_Pedido FOREIGN KEY (IdPedido)
        REFERENCES dbo.Pedido(IdPedido),
    CONSTRAINT FK_Entrega_Repartidor FOREIGN KEY (IdRepartidor)
        REFERENCES dbo.Usuario(IdUsuario),
    CONSTRAINT UQ_Entrega_Pedido UNIQUE (IdPedido),
    CONSTRAINT CK_Entrega_Estado CHECK (
        Estado IN ('PENDIENTE','EN_CAMINO','PENDIENTE_CONFIRMACION','ENTREGADO','FALLIDO')
    )
);
GO

CREATE TABLE dbo.Resena (
    IdResena      INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto    INT NOT NULL,
    IdCliente     INT NOT NULL,
    Calificacion  TINYINT NOT NULL,
    Comentario    NVARCHAR(800) NULL,
    Fecha         DATETIME2 NOT NULL CONSTRAINT DF_Resena_Fecha DEFAULT (SYSDATETIME()),

    CONSTRAINT FK_Resena_Producto FOREIGN KEY (IdProducto)
        REFERENCES dbo.Producto(IdProducto),
    CONSTRAINT FK_Resena_Cliente FOREIGN KEY (IdCliente)
        REFERENCES dbo.Usuario(IdUsuario),
    CONSTRAINT CK_Resena_Calificacion CHECK (Calificacion BETWEEN 1 AND 5),
    CONSTRAINT UQ_Resena_ProductoCliente UNIQUE (IdProducto, IdCliente)
);
GO

/* Tabla de auditoría: registra cambios importantes del sistema. */
CREATE TABLE dbo.Auditoria (
    IdAuditoria      BIGINT IDENTITY(1,1) PRIMARY KEY,
    Tabla            SYSNAME NOT NULL,
    Accion           VARCHAR(10) NOT NULL,
    IdRegistro       BIGINT NULL,
    Fecha            DATETIME2 NOT NULL CONSTRAINT DF_Auditoria_Fecha DEFAULT (SYSDATETIME()),
    UsuarioBD        NVARCHAR(128) NOT NULL CONSTRAINT DF_Auditoria_Usuario DEFAULT (SYSTEM_USER),
    DatosAnteriores  NVARCHAR(MAX) NULL,
    DatosNuevos      NVARCHAR(MAX) NULL,

    CONSTRAINT CK_Auditoria_Accion CHECK (Accion IN ('INSERT','UPDATE','DELETE'))
);
GO

/* =============================================================
   2. ÍNDICES ÚTILES
   ============================================================= */
CREATE INDEX IX_Direccion_IdUsuario ON dbo.Direccion(IdUsuario);
CREATE INDEX IX_Producto_Drop ON dbo.Producto(IdDrop);
CREATE INDEX IX_Producto_Categoria ON dbo.Producto(IdCategoria);
CREATE INDEX IX_Producto_Estado ON dbo.Producto(Estado);
CREATE INDEX IX_Producto_Nombre ON dbo.Producto(Nombre);
CREATE INDEX IX_Pedido_Cliente ON dbo.Pedido(IdCliente);
CREATE INDEX IX_Pedido_Estado ON dbo.Pedido(Estado);
CREATE INDEX IX_Detalle_Producto ON dbo.DetallePedido(IdProducto);
CREATE INDEX IX_Entrega_RepartidorEstado ON dbo.Entrega(IdRepartidor, Estado);
CREATE INDEX IX_Resena_Producto ON dbo.Resena(IdProducto);
CREATE INDEX IX_Auditoria_Fecha ON dbo.Auditoria(Fecha DESC);
GO

/* =============================================================
   3. TRIGGERS DE VALIDACIÓN DE REGLAS DE NEGOCIO
   ============================================================= */

/* El pedido debe pertenecer a un CLIENTE y la dirección debe ser de ese mismo cliente. */
CREATE TRIGGER dbo.TR_Pedido_ValidarClienteDireccion
ON dbo.Pedido
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.Usuario u ON u.IdUsuario = i.IdCliente
        WHERE u.Rol <> 'CLIENTE' OR u.Estado = 0
    )
    BEGIN
        THROW 50001, 'El pedido debe pertenecer a un usuario activo con rol CLIENTE.', 1;
    END;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.Direccion d ON d.IdDireccion = i.IdDireccion
        WHERE d.IdUsuario <> i.IdCliente
    )
    BEGIN
        THROW 50002, 'La dirección seleccionada no pertenece al cliente del pedido.', 1;
    END;
END;
GO

/* Solo un usuario con rol REPARTIDOR puede tomar una entrega. */
CREATE TRIGGER dbo.TR_Entrega_ValidarRepartidor
ON dbo.Entrega
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.Usuario u ON u.IdUsuario = i.IdRepartidor
        WHERE u.Rol <> 'REPARTIDOR' OR u.Estado = 0
    )
    BEGIN
        THROW 50003, 'La entrega debe estar asignada a un usuario activo con rol REPARTIDOR.', 1;
    END;
END;
GO

/* Solo clientes que recibieron el producto pueden publicar reseña. */
CREATE TRIGGER dbo.TR_Resena_ValidarCompraEntregada
ON dbo.Resena
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted r
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.Pedido p
            INNER JOIN dbo.DetallePedido dp ON dp.IdPedido = p.IdPedido
            INNER JOIN dbo.Entrega e ON e.IdPedido = p.IdPedido
            WHERE p.IdCliente = r.IdCliente
              AND dp.IdProducto = r.IdProducto
              AND p.Estado = 'ENTREGADO'
              AND e.Estado = 'ENTREGADO'
              AND e.ConfirmadoCliente = 1
        )
    )
    BEGIN
        THROW 50004, 'Solo se puede reseñar un producto que el cliente haya recibido.', 1;
    END;
END;
GO

/* =============================================================
   4. TRIGGERS DE AUDITORÍA
   Se auditan las tablas más importantes del negocio.
   ============================================================= */

CREATE TRIGGER dbo.TR_AUD_Usuario
ON dbo.Usuario
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Usuario',
        CASE
            WHEN i.IdUsuario IS NOT NULL AND d.IdUsuario IS NULL THEN 'INSERT'
            WHEN i.IdUsuario IS NOT NULL AND d.IdUsuario IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdUsuario, d.IdUsuario),
        SYSTEM_USER,
        CASE WHEN d.IdUsuario IS NULL THEN NULL ELSE (
            SELECT d.IdUsuario AS idUsuario, d.Nombre AS nombre, d.Correo AS correo,
                   d.Telefono AS telefono, d.DUI AS dui, d.Rol AS rol,
                   d.RequierePagoAnticipado AS requierePagoAnticipado,
                   d.Estado AS estado
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdUsuario IS NULL THEN NULL ELSE (
            SELECT i.IdUsuario AS idUsuario, i.Nombre AS nombre, i.Correo AS correo,
                   i.Telefono AS telefono, i.DUI AS dui, i.Rol AS rol,
                   i.RequierePagoAnticipado AS requierePagoAnticipado,
                   i.Estado AS estado
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdUsuario = i.IdUsuario;
END;
GO

CREATE TRIGGER dbo.TR_AUD_Producto
ON dbo.Producto
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Producto',
        CASE
            WHEN i.IdProducto IS NOT NULL AND d.IdProducto IS NULL THEN 'INSERT'
            WHEN i.IdProducto IS NOT NULL AND d.IdProducto IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdProducto, d.IdProducto),
        SYSTEM_USER,
        CASE WHEN d.IdProducto IS NULL THEN NULL ELSE (
            SELECT d.IdProducto AS idProducto, d.Nombre AS nombre, d.Precio AS precio,
                   d.EsUnico AS esUnico, d.Stock AS stock, d.Estado AS estado,
                   d.IdDrop AS idDrop, d.IdCategoria AS idCategoria
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdProducto IS NULL THEN NULL ELSE (
            SELECT i.IdProducto AS idProducto, i.Nombre AS nombre, i.Precio AS precio,
                   i.EsUnico AS esUnico, i.Stock AS stock, i.Estado AS estado,
                   i.IdDrop AS idDrop, i.IdCategoria AS idCategoria
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdProducto = i.IdProducto;
END;
GO

CREATE TRIGGER dbo.TR_AUD_Pedido
ON dbo.Pedido
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Pedido',
        CASE
            WHEN i.IdPedido IS NOT NULL AND d.IdPedido IS NULL THEN 'INSERT'
            WHEN i.IdPedido IS NOT NULL AND d.IdPedido IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdPedido, d.IdPedido),
        SYSTEM_USER,
        CASE WHEN d.IdPedido IS NULL THEN NULL ELSE (
            SELECT d.IdPedido AS idPedido, d.IdCliente AS idCliente, d.IdDireccion AS idDireccion,
                   d.Subtotal AS subtotal, d.CostoEnvio AS costoEnvio, d.Total AS total,
                   d.Estado AS estado
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdPedido IS NULL THEN NULL ELSE (
            SELECT i.IdPedido AS idPedido, i.IdCliente AS idCliente, i.IdDireccion AS idDireccion,
                   i.Subtotal AS subtotal, i.CostoEnvio AS costoEnvio, i.Total AS total,
                   i.Estado AS estado
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdPedido = i.IdPedido;
END;
GO

CREATE TRIGGER dbo.TR_AUD_Pago
ON dbo.Pago
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Pago',
        CASE
            WHEN i.IdPago IS NOT NULL AND d.IdPago IS NULL THEN 'INSERT'
            WHEN i.IdPago IS NOT NULL AND d.IdPago IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdPago, d.IdPago),
        SYSTEM_USER,
        CASE WHEN d.IdPago IS NULL THEN NULL ELSE (
            SELECT d.IdPago AS idPago, d.IdPedido AS idPedido, d.Metodo AS metodo,
                   d.Estado AS estado, d.Referencia AS referencia, d.FechaPago AS fechaPago
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdPago IS NULL THEN NULL ELSE (
            SELECT i.IdPago AS idPago, i.IdPedido AS idPedido, i.Metodo AS metodo,
                   i.Estado AS estado, i.Referencia AS referencia, i.FechaPago AS fechaPago
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdPago = i.IdPago;
END;
GO

CREATE TRIGGER dbo.TR_AUD_Entrega
ON dbo.Entrega
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Entrega',
        CASE
            WHEN i.IdEntrega IS NOT NULL AND d.IdEntrega IS NULL THEN 'INSERT'
            WHEN i.IdEntrega IS NOT NULL AND d.IdEntrega IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdEntrega, d.IdEntrega),
        SYSTEM_USER,
        CASE WHEN d.IdEntrega IS NULL THEN NULL ELSE (
            SELECT d.IdEntrega AS idEntrega, d.IdPedido AS idPedido, d.IdRepartidor AS idRepartidor,
                   d.Estado AS estado, d.FechaTomado AS fechaTomado, d.FechaEntrega AS fechaEntrega,
                   d.FotoEntregaUrl AS fotoEntregaUrl, d.ConfirmadoCliente AS confirmadoCliente
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdEntrega IS NULL THEN NULL ELSE (
            SELECT i.IdEntrega AS idEntrega, i.IdPedido AS idPedido, i.IdRepartidor AS idRepartidor,
                   i.Estado AS estado, i.FechaTomado AS fechaTomado, i.FechaEntrega AS fechaEntrega,
                   i.FotoEntregaUrl AS fotoEntregaUrl, i.ConfirmadoCliente AS confirmadoCliente
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdEntrega = i.IdEntrega;
END;
GO

CREATE TRIGGER dbo.TR_AUD_Resena
ON dbo.Resena
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Auditoria (Tabla, Accion, IdRegistro, UsuarioBD, DatosAnteriores, DatosNuevos)
    SELECT
        'Resena',
        CASE
            WHEN i.IdResena IS NOT NULL AND d.IdResena IS NULL THEN 'INSERT'
            WHEN i.IdResena IS NOT NULL AND d.IdResena IS NOT NULL THEN 'UPDATE'
            ELSE 'DELETE'
        END,
        COALESCE(i.IdResena, d.IdResena),
        SYSTEM_USER,
        CASE WHEN d.IdResena IS NULL THEN NULL ELSE (
            SELECT d.IdResena AS idResena, d.IdProducto AS idProducto, d.IdCliente AS idCliente,
                   d.Calificacion AS calificacion, d.Comentario AS comentario, d.Fecha AS fecha
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END,
        CASE WHEN i.IdResena IS NULL THEN NULL ELSE (
            SELECT i.IdResena AS idResena, i.IdProducto AS idProducto, i.IdCliente AS idCliente,
                   i.Calificacion AS calificacion, i.Comentario AS comentario, i.Fecha AS fecha
            FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
        ) END
    FROM inserted i
    FULL OUTER JOIN deleted d ON d.IdResena = i.IdResena;
END;
GO

/* =============================================================
   5. PROCEDIMIENTOS ALMACENADOS DEL FLUJO PRINCIPAL
   ============================================================= */

/* Crea el encabezado del pedido y su registro de pago.
   El costo de envío se calcula automáticamente:
   San Salvador = $2.00 / Fuera = $5.00
*/
CREATE PROCEDURE dbo.sp_CrearPedido
    @IdCliente   INT,
    @IdDireccion INT,
    @MetodoPago  VARCHAR(20),
    @IdPedido    INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Departamento NVARCHAR(80),
            @CostoEnvio DECIMAL(10,2),
            @RequierePagoAnticipado BIT;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Usuario
        WHERE IdUsuario = @IdCliente AND Rol = 'CLIENTE' AND Estado = 1
    )
        THROW 51001, 'Cliente inexistente o inactivo.', 1;

    SELECT
        @Departamento = Departamento
    FROM dbo.Direccion
    WHERE IdDireccion = @IdDireccion
      AND IdUsuario = @IdCliente;

    IF @Departamento IS NULL
        THROW 51002, 'La dirección no pertenece al cliente.', 1;

    IF @MetodoPago NOT IN ('CONTRA_ENTREGA','TRANSFERENCIA','DEPOSITO')
        THROW 51003, 'Método de pago no válido.', 1;

    SELECT @RequierePagoAnticipado = RequierePagoAnticipado
    FROM dbo.Usuario
    WHERE IdUsuario = @IdCliente;

    IF @RequierePagoAnticipado = 1 AND @MetodoPago = 'CONTRA_ENTREGA'
        THROW 51004, 'El cliente requiere pago anticipado por incumplimientos anteriores.', 1;

    SET @CostoEnvio = CASE
        WHEN UPPER(LTRIM(RTRIM(@Departamento))) = 'SAN SALVADOR' THEN 2.00
        ELSE 5.00
    END;

    BEGIN TRANSACTION;

    INSERT INTO dbo.Pedido (IdCliente, IdDireccion, CostoEnvio, Total)
    VALUES (@IdCliente, @IdDireccion, @CostoEnvio, @CostoEnvio);

    SET @IdPedido = SCOPE_IDENTITY();

    INSERT INTO dbo.Pago (IdPedido, Metodo, Estado)
    VALUES (@IdPedido, @MetodoPago, 'PENDIENTE');

    COMMIT TRANSACTION;
END;
GO

/* Agrega un producto al pedido usando bloqueo de fila para evitar
   que dos clientes aparten la misma prenda única al mismo tiempo. */
CREATE PROCEDURE dbo.sp_AgregarProductoPedido
    @IdPedido   INT,
    @IdProducto INT,
    @Cantidad   INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Precio DECIMAL(10,2),
            @Stock INT,
            @EsUnico BIT,
            @EstadoProducto VARCHAR(15),
            @EstadoPedido VARCHAR(30);

    IF @Cantidad <= 0
        THROW 51010, 'La cantidad debe ser mayor que cero.', 1;

    BEGIN TRANSACTION;

    SELECT @EstadoPedido = Estado
    FROM dbo.Pedido WITH (UPDLOCK, HOLDLOCK)
    WHERE IdPedido = @IdPedido;

    IF @EstadoPedido IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51011, 'Pedido no encontrado.', 1;
    END;

    IF @EstadoPedido <> 'PENDIENTE'
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51012, 'Solo se pueden agregar productos a pedidos pendientes.', 1;
    END;

    SELECT
        @Precio = Precio,
        @Stock = Stock,
        @EsUnico = EsUnico,
        @EstadoProducto = Estado
    FROM dbo.Producto WITH (UPDLOCK, HOLDLOCK)
    WHERE IdProducto = @IdProducto;

    IF @Precio IS NULL
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51013, 'Producto no encontrado.', 1;
    END;

    IF @EsUnico = 1 AND @Cantidad <> 1
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51014, 'Una prenda única solo puede agregarse con cantidad 1.', 1;
    END;

    IF @EstadoProducto = 'VENDIDO' OR @Stock < @Cantidad
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51015, 'No hay stock disponible para este producto.', 1;
    END;

    IF @EsUnico = 1 AND @EstadoProducto <> 'DISPONIBLE'
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51016, 'La prenda única ya está apartada o vendida.', 1;
    END;

    INSERT INTO dbo.DetallePedido (IdPedido, IdProducto, Cantidad, PrecioUnitario)
    VALUES (@IdPedido, @IdProducto, @Cantidad, @Precio);

    UPDATE dbo.Producto
    SET Stock = Stock - @Cantidad,
        Estado = CASE
                    WHEN EsUnico = 1 THEN 'APARTADO'
                    WHEN Stock - @Cantidad = 0 THEN 'APARTADO'
                    ELSE 'DISPONIBLE'
                 END
    WHERE IdProducto = @IdProducto;

    UPDATE p
    SET p.Subtotal = x.Subtotal,
        p.Total = x.Subtotal + p.CostoEnvio
    FROM dbo.Pedido p
    CROSS APPLY (
        SELECT SUM(dp.Cantidad * dp.PrecioUnitario) AS Subtotal
        FROM dbo.DetallePedido dp
        WHERE dp.IdPedido = p.IdPedido
    ) x
    WHERE p.IdPedido = @IdPedido;

    COMMIT TRANSACTION;
END;
GO

/* Verificación manual de transferencia o depósito por el administrador. */
CREATE PROCEDURE dbo.sp_VerificarPagoAnticipado
    @IdPedido   INT,
    @Referencia NVARCHAR(120)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Metodo VARCHAR(20);

    SELECT @Metodo = Metodo
    FROM dbo.Pago
    WHERE IdPedido = @IdPedido;

    IF @Metodo IS NULL
        THROW 51020, 'No existe pago para el pedido.', 1;

    IF @Metodo = 'CONTRA_ENTREGA'
        THROW 51021, 'Este pedido es contra entrega y no requiere verificación anticipada.', 1;

    BEGIN TRANSACTION;

    UPDATE dbo.Pago
    SET Estado = 'VERIFICADO',
        Referencia = @Referencia,
        FechaPago = SYSDATETIME()
    WHERE IdPedido = @IdPedido;

    -- Para una prenda única pagada anticipadamente, ya se considera vendida.
    UPDATE pr
    SET pr.Estado = CASE
                        WHEN pr.EsUnico = 1 THEN 'VENDIDO'
                        WHEN pr.Stock = 0 THEN 'VENDIDO'
                        ELSE 'DISPONIBLE'
                    END
    FROM dbo.Producto pr
    INNER JOIN dbo.DetallePedido dp ON dp.IdProducto = pr.IdProducto
    WHERE dp.IdPedido = @IdPedido;

    COMMIT TRANSACTION;
END;
GO

/* Un repartidor toma un pedido disponible. */
CREATE PROCEDURE dbo.sp_TomarPedido
    @IdPedido     INT,
    @IdRepartidor INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Usuario
        WHERE IdUsuario = @IdRepartidor AND Rol = 'REPARTIDOR' AND Estado = 1
    )
        THROW 51030, 'Usuario no válido como repartidor.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Pedido p
        INNER JOIN dbo.Pago pg ON pg.IdPedido = p.IdPedido
        WHERE p.IdPedido = @IdPedido
          AND p.Estado = 'PENDIENTE'
          AND (
                pg.Metodo = 'CONTRA_ENTREGA'
                OR (pg.Metodo IN ('TRANSFERENCIA','DEPOSITO') AND pg.Estado = 'VERIFICADO')
              )
    )
        THROW 51031, 'El pedido no está disponible para entrega.', 1;

    BEGIN TRY
        INSERT INTO dbo.Entrega (IdPedido, IdRepartidor, Estado)
        VALUES (@IdPedido, @IdRepartidor, 'PENDIENTE');
    END TRY
    BEGIN CATCH
        IF ERROR_NUMBER() IN (2601,2627)
            THROW 51032, 'El pedido ya fue tomado por otro repartidor.', 1;
        ELSE
            THROW;
    END CATCH;
END;
GO

CREATE PROCEDURE dbo.sp_MarcarEnCamino
    @IdPedido     INT,
    @IdRepartidor INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Entrega
        WHERE IdPedido = @IdPedido
          AND IdRepartidor = @IdRepartidor
          AND Estado = 'PENDIENTE'
    )
        THROW 51040, 'La entrega no pertenece al repartidor o no está pendiente.', 1;

    UPDATE dbo.Entrega
    SET Estado = 'EN_CAMINO'
    WHERE IdPedido = @IdPedido AND IdRepartidor = @IdRepartidor;

    UPDATE dbo.Pedido
    SET Estado = 'EN_CAMINO'
    WHERE IdPedido = @IdPedido;
END;
GO

/* El repartidor registra la entrega física y sube evidencia.
   Aún falta la confirmación final del cliente. */
CREATE PROCEDURE dbo.sp_RegistrarEntrega
    @IdPedido       INT,
    @IdRepartidor   INT,
    @FotoEntregaUrl NVARCHAR(500),
    @Observacion    NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NULLIF(LTRIM(RTRIM(@FotoEntregaUrl)), '') IS NULL
        THROW 51050, 'La evidencia fotográfica es obligatoria.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Entrega
        WHERE IdPedido = @IdPedido
          AND IdRepartidor = @IdRepartidor
          AND Estado = 'EN_CAMINO'
    )
        THROW 51051, 'La entrega no está en camino o no pertenece al repartidor.', 1;

    UPDATE dbo.Entrega
    SET Estado = 'PENDIENTE_CONFIRMACION',
        FechaEntrega = SYSDATETIME(),
        FotoEntregaUrl = @FotoEntregaUrl,
        Observacion = @Observacion
    WHERE IdPedido = @IdPedido AND IdRepartidor = @IdRepartidor;

    UPDATE dbo.Pedido
    SET Estado = 'PENDIENTE_CONFIRMACION'
    WHERE IdPedido = @IdPedido;
END;
GO

/* El cliente confirma que sí recibió el pedido. */
CREATE PROCEDURE dbo.sp_ConfirmarRecepcion
    @IdPedido  INT,
    @IdCliente INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.Pedido p
        INNER JOIN dbo.Entrega e ON e.IdPedido = p.IdPedido
        WHERE p.IdPedido = @IdPedido
          AND p.IdCliente = @IdCliente
          AND p.Estado = 'PENDIENTE_CONFIRMACION'
          AND e.Estado = 'PENDIENTE_CONFIRMACION'
    )
        THROW 51060, 'El pedido no está pendiente de confirmación para este cliente.', 1;

    BEGIN TRANSACTION;

    UPDATE dbo.Entrega
    SET Estado = 'ENTREGADO',
        ConfirmadoCliente = 1
    WHERE IdPedido = @IdPedido;

    UPDATE dbo.Pedido
    SET Estado = 'ENTREGADO'
    WHERE IdPedido = @IdPedido;

    UPDATE pr
    SET pr.Estado = CASE
                        WHEN pr.EsUnico = 1 THEN 'VENDIDO'
                        WHEN pr.Stock = 0 THEN 'VENDIDO'
                        ELSE 'DISPONIBLE'
                    END
    FROM dbo.Producto pr
    INNER JOIN dbo.DetallePedido dp ON dp.IdProducto = pr.IdProducto
    WHERE dp.IdPedido = @IdPedido;

    -- Si era contra entrega, el pago se considera realizado al confirmar recepción.
    UPDATE dbo.Pago
    SET Estado = 'VERIFICADO',
        FechaPago = COALESCE(FechaPago, SYSDATETIME())
    WHERE IdPedido = @IdPedido
      AND Metodo = 'CONTRA_ENTREGA';

    COMMIT TRANSACTION;
END;
GO

/* Si falla una entrega contra entrega:
   - libera el stock
   - vuelve el producto a disponible
   - marca al cliente para pago anticipado futuro
   En un pago anticipado, NO se libera el producto porque ya fue pagado.
*/
CREATE PROCEDURE dbo.sp_RegistrarEntregaFallida
    @IdPedido      INT,
    @IdRepartidor  INT,
    @Observacion   NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @Metodo VARCHAR(20), @IdCliente INT;

    SELECT
        @Metodo = pg.Metodo,
        @IdCliente = p.IdCliente
    FROM dbo.Pedido p
    INNER JOIN dbo.Pago pg ON pg.IdPedido = p.IdPedido
    WHERE p.IdPedido = @IdPedido;

    IF @Metodo IS NULL
        THROW 51070, 'Pedido no encontrado.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.Entrega
        WHERE IdPedido = @IdPedido
          AND IdRepartidor = @IdRepartidor
          AND Estado IN ('PENDIENTE','EN_CAMINO','PENDIENTE_CONFIRMACION')
    )
        THROW 51071, 'La entrega no puede marcarse como fallida por este repartidor.', 1;

    BEGIN TRANSACTION;

    UPDATE dbo.Entrega
    SET Estado = 'FALLIDO',
        Observacion = @Observacion
    WHERE IdPedido = @IdPedido AND IdRepartidor = @IdRepartidor;

    UPDATE dbo.Pedido
    SET Estado = 'FALLIDO'
    WHERE IdPedido = @IdPedido;

    IF @Metodo = 'CONTRA_ENTREGA'
    BEGIN
        UPDATE pr
        SET pr.Stock = pr.Stock + dp.Cantidad,
            pr.Estado = 'DISPONIBLE'
        FROM dbo.Producto pr
        INNER JOIN dbo.DetallePedido dp ON dp.IdProducto = pr.IdProducto
        WHERE dp.IdPedido = @IdPedido;

        UPDATE dbo.Usuario
        SET RequierePagoAnticipado = 1
        WHERE IdUsuario = @IdCliente;
    END;

    COMMIT TRANSACTION;
END;
GO

/* =============================================================
   6. VISTAS
   ============================================================= */

/* Catálogo público: muestra productos disponibles y sus calificaciones. */
CREATE VIEW dbo.vw_CatalogoDisponible
AS
SELECT
    p.IdProducto,
    p.Nombre,
    p.Descripcion,
    p.Talla,
    p.Precio,
    p.ImagenUrl,
    p.EsUnico,
    p.Stock,
    p.Estado,
    c.Nombre AS Categoria,
    d.Nombre AS DropNombre,
    d.FechaPublicacion,
    CAST(ISNULL(r.Promedio, 0) AS DECIMAL(4,2)) AS PromedioCalificacion,
    ISNULL(r.CantidadResenas, 0) AS CantidadResenas
FROM dbo.Producto p
INNER JOIN dbo.Categoria c ON c.IdCategoria = p.IdCategoria
INNER JOIN dbo.Drops d ON d.IdDrop = p.IdDrop
OUTER APPLY (
    SELECT
        AVG(CAST(rs.Calificacion AS DECIMAL(4,2))) AS Promedio,
        COUNT(*) AS CantidadResenas
    FROM dbo.Resena rs
    WHERE rs.IdProducto = p.IdProducto
) r
WHERE p.Estado = 'DISPONIBLE'
  AND p.Stock > 0
  AND c.Estado = 1
  AND d.Estado = 'PUBLICADO';
GO

/* Resumen general de pedidos para el administrador. */
CREATE VIEW dbo.vw_PedidosResumen
AS
SELECT
    p.IdPedido,
    p.FechaPedido,
    p.Estado AS EstadoPedido,
    u.IdUsuario AS IdCliente,
    u.Nombre AS Cliente,
    u.Telefono,
    d.Tipo AS TipoDireccion,
    d.Departamento,
    d.Municipio,
    d.DireccionTexto,
    p.Subtotal,
    p.CostoEnvio,
    p.Total,
    pg.Metodo AS MetodoPago,
    pg.Estado AS EstadoPago,
    e.IdEntrega,
    e.IdRepartidor,
    rep.Nombre AS Repartidor,
    e.Estado AS EstadoEntrega,
    e.ConfirmadoCliente
FROM dbo.Pedido p
INNER JOIN dbo.Usuario u ON u.IdUsuario = p.IdCliente
INNER JOIN dbo.Direccion d ON d.IdDireccion = p.IdDireccion
LEFT JOIN dbo.Pago pg ON pg.IdPedido = p.IdPedido
LEFT JOIN dbo.Entrega e ON e.IdPedido = p.IdPedido
LEFT JOIN dbo.Usuario rep ON rep.IdUsuario = e.IdRepartidor;
GO

/* Pedidos que puede ver/tomar un repartidor en la app móvil. */
CREATE VIEW dbo.vw_PedidosDisponiblesRepartidor
AS
SELECT
    p.IdPedido,
    p.FechaPedido,
    u.Nombre AS Cliente,
    u.Telefono,
    d.Tipo AS TipoDireccion,
    d.Departamento,
    d.Municipio,
    d.DireccionTexto,
    d.Referencia,
    p.Total,
    pg.Metodo AS MetodoPago
FROM dbo.Pedido p
INNER JOIN dbo.Usuario u ON u.IdUsuario = p.IdCliente
INNER JOIN dbo.Direccion d ON d.IdDireccion = p.IdDireccion
INNER JOIN dbo.Pago pg ON pg.IdPedido = p.IdPedido
LEFT JOIN dbo.Entrega e ON e.IdPedido = p.IdPedido
WHERE p.Estado = 'PENDIENTE'
  AND e.IdEntrega IS NULL
  AND (
        pg.Metodo = 'CONTRA_ENTREGA'
        OR (pg.Metodo IN ('TRANSFERENCIA','DEPOSITO') AND pg.Estado = 'VERIFICADO')
      );
GO

/* Ventas finalizadas para reportes sencillos. */
CREATE VIEW dbo.vw_VentasEntregadas
AS
SELECT
    p.IdPedido,
    p.FechaPedido,
    e.FechaEntrega,
    u.Nombre AS Cliente,
    p.Subtotal,
    p.CostoEnvio,
    p.Total,
    pg.Metodo AS MetodoPago,
    rep.Nombre AS Repartidor
FROM dbo.Pedido p
INNER JOIN dbo.Usuario u ON u.IdUsuario = p.IdCliente
INNER JOIN dbo.Pago pg ON pg.IdPedido = p.IdPedido
INNER JOIN dbo.Entrega e ON e.IdPedido = p.IdPedido
INNER JOIN dbo.Usuario rep ON rep.IdUsuario = e.IdRepartidor
WHERE p.Estado = 'ENTREGADO'
  AND e.Estado = 'ENTREGADO'
  AND e.ConfirmadoCliente = 1;
GO

/* Vista simplificada de auditoría. */
CREATE VIEW dbo.vw_AuditoriaReciente
AS
SELECT TOP (500)
    IdAuditoria,
    Tabla,
    Accion,
    IdRegistro,
    Fecha,
    UsuarioBD,
    DatosAnteriores,
    DatosNuevos
FROM dbo.Auditoria
ORDER BY Fecha DESC;
GO

/* =============================================================
   7. ROLES Y PERMISOS DE BASE DE DATOS
   (No confundir con CLIENTE / ADMIN / REPARTIDOR de la aplicación.)
   ============================================================= */
CREATE ROLE rol_lowkeys_consulta;
CREATE ROLE rol_lowkeys_operacion;
GO

GRANT SELECT ON dbo.vw_CatalogoDisponible TO rol_lowkeys_consulta;
GRANT SELECT ON dbo.vw_PedidosResumen TO rol_lowkeys_consulta;
GRANT SELECT ON dbo.vw_PedidosDisponiblesRepartidor TO rol_lowkeys_consulta;
GRANT SELECT ON dbo.vw_VentasEntregadas TO rol_lowkeys_consulta;
GO

GRANT SELECT ON dbo.vw_CatalogoDisponible TO rol_lowkeys_operacion;
GRANT SELECT ON dbo.vw_PedidosResumen TO rol_lowkeys_operacion;
GRANT SELECT ON dbo.vw_PedidosDisponiblesRepartidor TO rol_lowkeys_operacion;
GRANT SELECT ON dbo.vw_VentasEntregadas TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_CrearPedido TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_AgregarProductoPedido TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_VerificarPagoAnticipado TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_TomarPedido TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_MarcarEnCamino TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_RegistrarEntrega TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_ConfirmarRecepcion TO rol_lowkeys_operacion;
GRANT EXECUTE ON dbo.sp_RegistrarEntregaFallida TO rol_lowkeys_operacion;
GO

/* Para agregar posteriormente un usuario SQL existente a un rol:
   ALTER ROLE rol_lowkeys_operacion ADD MEMBER NombreUsuarioSQL;
*/

/* =============================================================
   8. REGISTROS INICIALES / DATOS DE PRUEBA
   Las contraseñas son valores DEMO. En la API real deben almacenarse
   hashes generados con BCrypt/PasswordHasher, nunca texto plano.
   ============================================================= */

INSERT INTO dbo.Usuario (Nombre, Correo, ContrasenaHash, Telefono, DUI, Rol)
VALUES
('Administrador Lowkeys', 'admin@lowkeys.local', 'HASH_DEMO_ADMIN', '7000-0001', NULL, 'ADMIN'),
('Encomendero 1', 'repartidor1@lowkeys.local', 'HASH_DEMO_REPARTIDOR1', '7000-0002', NULL, 'REPARTIDOR'),
('Encomendero 2', 'repartidor2@lowkeys.local', 'HASH_DEMO_REPARTIDOR2', '7000-0003', NULL, 'REPARTIDOR'),
('Cliente Demo 1', 'cliente1@lowkeys.local', 'HASH_DEMO_CLIENTE1', '7000-1001', '01234567-8', 'CLIENTE'),
('Cliente Demo 2', 'cliente2@lowkeys.local', 'HASH_DEMO_CLIENTE2', '7000-1002', '12345678-9', 'CLIENTE');
GO

INSERT INTO dbo.Direccion (IdUsuario, Tipo, Departamento, Municipio, DireccionTexto, Referencia)
VALUES
(4, 'CASA', 'San Salvador', 'San Salvador Centro', 'Colonia Demo, Pasaje 1, Casa 10', 'Portón negro'),
(4, 'TRABAJO', 'La Libertad', 'Antiguo Cuscatlán', 'Zona industrial Demo, edificio B', 'Recepción principal'),
(5, 'CASA', 'Santa Ana', 'Santa Ana Centro', 'Residencial Demo, Calle 3', 'Frente a parque');
GO

INSERT INTO dbo.Categoria (Nombre, Descripcion)
VALUES
('Jeans', 'Jeans bootcut, flared y estilos similares'),
('Camisas', 'Camisas ajustadas y prendas superiores alternativas'),
('Accesorios', 'Cadenas, llaveros y accesorios alternativos'),
('Chaquetas', 'Chaquetas vintage, thrift y alternativas');
GO

INSERT INTO dbo.Drops (Nombre, Descripcion, FechaPublicacion, Estado)
VALUES
('Drop 01 - Revival', 'Primer drop con jeans y camisas estilo revival.', CAST(GETDATE() AS DATE), 'PUBLICADO'),
('Drop 02 - Night Shift', 'Segundo drop con piezas oscuras y accesorios.', DATEADD(DAY, 7, CAST(GETDATE() AS DATE)), 'BORRADOR');
GO

INSERT INTO dbo.Producto
(IdDrop, IdCategoria, Nombre, Descripcion, Talla, Precio, ImagenUrl, EsUnico, Stock, Estado)
VALUES
(1, 1, 'Jeans Bootcut Vintage', 'Jeans thrift bootcut de pieza única.', '32', 28.00, 'img/jeans-bootcut-01.jpg', 1, 1, 'DISPONIBLE'),
(1, 1, 'Jeans Flared Dark Wash', 'Jeans flared oscuro, pieza única.', '30', 32.00, 'img/jeans-flared-01.jpg', 1, 1, 'DISPONIBLE'),
(1, 2, 'Camisa Fit Black', 'Camisa ajustada negra estilo alternativo.', 'M', 18.00, 'img/camisa-black-01.jpg', 1, 1, 'DISPONIBLE'),
(1, 4, 'Chaqueta Denim Distressed', 'Chaqueta denim thrift con desgaste.', 'L', 35.00, 'img/chaqueta-01.jpg', 1, 1, 'DISPONIBLE'),
(1, 3, 'Cadena Metal Lowkeys', 'Cadena metálica para outfit alternativo.', NULL, 8.00, 'img/cadena-01.jpg', 0, 10, 'DISPONIBLE'),
(1, 3, 'Llavero Skull', 'Llavero metálico estilo alt.', NULL, 5.00, 'img/llavero-01.jpg', 0, 15, 'DISPONIBLE');
GO

/* =============================================================
   9. PRUEBAS RÁPIDAS SUGERIDAS
   Puedes ejecutarlas manualmente en clase.
   ============================================================= */

-- A) Ver catálogo público
SELECT * FROM dbo.vw_CatalogoDisponible;
GO

-- B) Crear un pedido de ejemplo para el cliente 4 usando su dirección CASA (1)
DECLARE @PedidoDemo INT;
EXEC dbo.sp_CrearPedido
    @IdCliente = 4,
    @IdDireccion = 1,
    @MetodoPago = 'CONTRA_ENTREGA',
    @IdPedido = @PedidoDemo OUTPUT;

SELECT @PedidoDemo AS PedidoCreado;

-- C) Apartar la prenda única 1
EXEC dbo.sp_AgregarProductoPedido
    @IdPedido = @PedidoDemo,
    @IdProducto = 1,
    @Cantidad = 1;

-- D) Ver el pedido y el catálogo luego del apartado
SELECT * FROM dbo.vw_PedidosResumen WHERE IdPedido = @PedidoDemo;
SELECT * FROM dbo.Producto WHERE IdProducto = 1;
SELECT * FROM dbo.vw_PedidosDisponiblesRepartidor WHERE IdPedido = @PedidoDemo;

-- E) Repartidor 2 toma el pedido, lo pone en camino y registra evidencia
EXEC dbo.sp_TomarPedido @IdPedido = @PedidoDemo, @IdRepartidor = 2;
EXEC dbo.sp_MarcarEnCamino @IdPedido = @PedidoDemo, @IdRepartidor = 2;
EXEC dbo.sp_RegistrarEntrega
    @IdPedido = @PedidoDemo,
    @IdRepartidor = 2,
    @FotoEntregaUrl = 'evidencias/pedido-demo.jpg',
    @Observacion = 'Pedido entregado en la dirección indicada.';

-- F) El cliente confirma recepción
EXEC dbo.sp_ConfirmarRecepcion @IdPedido = @PedidoDemo, @IdCliente = 4;

-- G) Ahora sí puede dejar reseña
INSERT INTO dbo.Resena (IdProducto, IdCliente, Calificacion, Comentario)
VALUES (1, 4, 5, 'La prenda llegó tal como aparecía en el drop.');

-- H) Revisar venta, reseña y auditoría
SELECT * FROM dbo.vw_VentasEntregadas WHERE IdPedido = @PedidoDemo;
SELECT * FROM dbo.Resena WHERE IdProducto = 1;
SELECT TOP 50 * FROM dbo.vw_AuditoriaReciente;
GO

/* =============================================================
   FIN DEL SCRIPT
   ============================================================= */
