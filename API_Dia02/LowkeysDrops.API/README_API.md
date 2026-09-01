# LowkeysDrops.API

## Â¿QuÃ© es este proyecto?
LowkeysDrops.API es una API REST desarrollada con ASP.NET Core, diseÃ±ada para servir como backend del emprendimiento de ropa *Lowkeys Drops*. Esta API representa la Fase 2 del proyecto acadÃ©mico, construida sobre una base de datos SQL Server ya existente (LowkeysDropsDB) utilizando el enfoque Database First con Entity Framework Core.

## Arquitectura y Estructura
La aplicaciÃ³n sigue una arquitectura estructurada en capas para mantener el cÃ³digo ordenado, comprensible y fÃ¡cil de mantener:

- **Controllers/**: Gestionan las peticiones HTTP y dirigen el trÃ¡fico a los servicios.
- **Services/**: Contienen la lÃ³gica de negocio para mapear DTOs y comunicarse con el repositorio.
- **Repositories/**: InteractÃºan directamente con Entity Framework Core y ejecutan Procedimientos Almacenados.
- **DTOs/**: Objetos de Transferencia de Datos utilizados para enviar y recibir datos en las peticiones HTTP, previniendo el sobrepaso (over-posting).
- **Models/Entities/**: Clases generadas automÃ¡ticamente por Scaffold-DbContext que representan la base de datos real.
- **Middleware/**: Contiene el GlobalExceptionMiddleware que captura errores y excepciones globalmente devolviendo respuestas JSON estandarizadas.

## ConexiÃ³n a SQL Server y Database First
El proyecto se conecta a LowkeysDropsDB a travÃ©s de la connection string definida en ppsettings.json. Las entidades fueron mapeadas fielmente a la estructura de la base de datos, garantizando que **no se utilice Code First ni migraciones**; la base de datos es la Ãºnica fuente de verdad.

## Principales Endpoints y CÃ³digos HTTP
La API expone operaciones CRUD para productos, categorÃ­as, drops y direcciones, asÃ­ como un flujo completo de negocio para la gestiÃ³n de pedidos y entregas:

- GET /api/catalogo (200 OK)
- POST /api/pedidos (201 Created, 400 Bad Request, 409 Conflict)
- PUT /api/pedidos/{id}/entrega (200 OK)

## Procedimientos Almacenados y Vistas Utilizadas
Para respetar las reglas de negocio en la base de datos, utilizamos:
**Vistas**: w_CatalogoDisponible, w_PedidosResumen, w_PedidosDisponiblesRepartidor, w_VentasEntregadas, w_AuditoriaReciente.
**Procedimientos Almacenados**: sp_CrearPedido, sp_AgregarProductoPedido, sp_VerificarPagoAnticipado, sp_TomarPedido, sp_MarcarEnCamino, sp_RegistrarEntrega, sp_RegistrarEntregaFallida, sp_ConfirmarRecepcion.

## CÃ³mo ejecutar la API
1. AsegÃºrate de tener la base de datos LowkeysDropsDB en tu SQL Server local (LOWKIPC\LOWK).
2. Abre una terminal en la raÃ­z del proyecto.
3. Ejecuta dotnet restore.
4. Ejecuta dotnet build.
5. Ejecuta dotnet run.
6. Navega a http://localhost:5069/swagger para explorar y probar la API con Swagger.
