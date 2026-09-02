# DocumentaciÃ³n API - Lowkeys Drops

## IntroducciÃ³n y Objetivo
El objetivo de este documento es detallar la estructura y funcionamiento de la API REST de Lowkeys Drops (Fase 2). Esta API expone la lÃ³gica de negocio a aplicaciones cliente (Web/MÃ³vil) respetando las restricciones, triggers y procedimientos definidos previamente en la base de datos de SQL Server.

## Arquitectura y TecnologÃ­as
Se utilizÃ³ **ASP.NET Core 8**, **Entity Framework Core**, y **Swagger**. La aplicaciÃ³n se divide en Capa de PresentaciÃ³n (Controllers), Capa LÃ³gica (Services) y Capa de Acceso a Datos (Repositories). 

## ConexiÃ³n con la base de datos
Se empleÃ³ **Database First** mediante Scaffold-DbContext. La Connection String se inyecta mediante Dependency Injection. No se utilizan migraciones ya que la BD no debe ser alterada por el cÃ³digo.

## DescripciÃ³n de Endpoints y DTOs
Los DTOs garantizan que la informaciÃ³n que entra y sale estÃ¡ validada mediante DataAnnotations (ej. [Required], [StringLength]).
- **Productos, CategorÃ­as, Drops, Direcciones**: Disponen de endpoints estÃ¡ndar para CRUD (GET, POST, PUT, DELETE).
- **Pedidos**: GestiÃ³n transaccional invocando Procedimientos Almacenados (ej. sp_CrearPedido, sp_TomarPedido).
- **Admin**: Endpoints de solo lectura basados en Vistas para estadÃ­sticas de ventas y auditorÃ­a.

## Validaciones y Manejo de Errores
Se implementÃ³ un **GlobalExceptionMiddleware** que intercepta todas las excepciones y las traduce a un formato JSON entendible.
- Los errores SQL THROW (ej. cÃ³digos 50000+) se mapean a HTTP 400 Bad Request.
- Excepciones de unicidad SQL (2601, 2627) se mapean a 409 Conflict.
- Registros no encontrados se mapean a 404 Not Found.

## Swagger e Instrucciones de EjecuciÃ³n
Para ejecutar y probar la API:
1. dotnet build
2. dotnet run
3. Visitar /swagger en el navegador. Swagger listarÃ¡ todos los DTOs y parÃ¡metros esperados.
