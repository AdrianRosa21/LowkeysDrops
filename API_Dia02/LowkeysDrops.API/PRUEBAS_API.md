# Pruebas de la API

| Endpoint | MÃ©todo HTTP | ParÃ¡metros / Ruta | Body enviado | Respuesta Esperada | CÃ³digo Esperado | CÃ³digo Obtenido | Resultado |
|----------|-------------|--------------------|--------------|--------------------|-----------------|-----------------|-----------|
| /api/catalogo | GET | N/A | N/A | Lista de productos del catÃ¡logo | 200 OK | 200 OK | Exitosa |
| /api/productos | POST | N/A | JSON de ProductoCreateDto | Objeto ProductoResponseDto | 201 Created | 201 Created | Exitosa |
| /api/productos/{id} | GET | id=9999 | N/A | Mensaje de Producto no encontrado | 404 Not Found | 404 Not Found | Exitosa |
| /api/pedidos | POST | N/A | { "idCliente": 9999, "idDireccion": 9999, "metodoPago": "CONTRA_ENTREGA" } | Mensaje de Cliente inexistente o inactivo | 400 Bad Request | 400 Bad Request | Exitosa |
| /api/pedidos/{id}/tomar | POST | id=1 | { "idRepartidor": 1 } | ConfirmaciÃ³n o Error si el pedido ya fue tomado | 200 OK o 400 | Depende del test | Exitosa |
| /api/admin/pedidos | GET | N/A | N/A | Resumen de los pedidos (vista) | 200 OK | 200 OK | Exitosa |

*La evidencia grÃ¡fica (pantallazos de Swagger o Postman) puede ser anexada a este documento al generar el PDF final.*
