# Guía de Evidencias Móviles

Esta carpeta es para apoyar la creación del `EvidenciasMobile.pdf`. Toma capturas de pantalla de la aplicación móvil corriendo y guárdalas o añádelas a tu PDF demostrando lo siguiente:

### Capturas Necesarias:

1. **Pantalla de Login y Splash**: Demuestra que la aplicación inicializa.
2. **Catálogo de Productos**: Muestra la grilla de productos obtenida desde la API. (Demuestra el `GET` real).
3. **Estado de Carga (Loading)**: Captura el indicador de progreso al cargar catálogo o realizar una compra.
4. **Detalle de Producto**: Muestra la información de un producto seleccionado. (Navegación e integración).
5. **Estado de Error / Recuperación**: Apaga temporalmente la API (con `Ctrl+C` en su consola) y recarga el catálogo en Flutter. Captura el mensaje de error de conexión. Enciende la API y dale "REINTENTAR".
6. **Pantalla de Checkout (Confirmar Compra)**: Muestra las direcciones del usuario y el botón de crear pedido.
7. **Confirmación de Escritura**: Toma captura del SnackBar verde "Pedido creado con éxito" al confirmar la compra. (Demuestra el `POST` de escritura a la API).
8. **Mis Pedidos**: Muestra la lista de pedidos con el nuevo pedido recién creado, verificando que la base de datos se actualizó.

Todas estas pruebas cubren la rúbrica de conexión, lectura, escritura, manejo de estados y navegación.
