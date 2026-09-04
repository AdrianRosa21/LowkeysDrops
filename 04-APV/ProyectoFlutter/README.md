# Lowkeys Drops - Aplicación Móvil (Fase 4)

Aplicación móvil desarrollada en Flutter para la tienda Lowkeys Drops. Reutiliza la misma API REST desarrollada en la Fase 2 del proyecto.

## Arquitectura y Configuración

- **Patrón:** MVCS (Model-View-Controller-Service simplificado)
- **Cliente HTTP:** `http`
- **Almacenamiento Local:** `shared_preferences` para la persistencia de la sesión y token de autenticación.

### Estructura
```
lib/
├── config/
│   ├── api_config.dart (Configuración de la Base URL)
│   └── app_theme.dart (Paleta de colores oscura, gótica, streetwear)
├── models/
│   └── (Mapeos de los DTOs de la API)
├── screens/
│   └── (Vistas principales: Login, Catálogo, Detalle, Checkout, Mis Pedidos)
├── services/
│   └── (Lógica de conexión: AuthService, ProductoService, PedidoService, DireccionService)
└── main.dart
```

## Requisitos y Configuración de API

### Base URL

La URL principal está en `lib/config/api_config.dart`. Por defecto, está apuntada a:
- Android Emulator: `http://10.0.2.2:5069/api` (Recomendado si pruebas en emulador local)
- Dispositivo Físico o Entorno Web: Usa tu dirección IPv4 (e.g. `http://192.168.1.XX:5069/api`). No uses `localhost` en dispositivo móvil físico ya que apuntaría al propio teléfono.

### Cómo Instalar y Ejecutar

1. Abre una terminal en `04-Mobile/ProyectoFlutter`
2. Ejecuta `flutter pub get`
3. Ejecuta `flutter run` o presiona F5 en tu IDE seleccionado tu emulador o dispositivo web.

## Endpoints Consumidos (Ejemplos Reales)

- `POST /api/auth/login`: Autenticación del usuario y recepción de token.
- `GET /api/Catalogo`: Obtiene la vista de catálogo (`VwCatalogoDisponible`).
- `GET /api/Productos/{id}`: Obtiene el detalle de un producto específico.
- `GET /api/Direcciones`: Obtiene las direcciones del cliente para confirmar el envío.
- `POST /api/Pedidos`: Crea un pedido con ID Cliente, Dirección y Método de Pago (Operación de Escritura Principal).
- `POST /api/Pedidos/{id}/productos`: Agrega un producto al pedido previamente creado.
- `GET /api/Pedidos/cliente/{id}`: Obtiene el historial del cliente desde la vista de resumen de pedidos.

## Flujo Principal (Cliente)

1. **SplashScreen**: Verifica si existe un token en la sesión.
2. **LoginScreen**: Si no hay token, el cliente inicia sesión.
3. **CatalogScreen**: Muestra los productos disponibles (con manejo de Loading, Error y Reintento).
4. **ProductDetailScreen**: Muestra imagen grande, descripciones, stock y botón para comprar.
5. **CheckoutScreen**: Permite seleccionar una dirección previamente creada y confirmar pedido (Escritura hacia la API).
6. **MyOrdersScreen**: Permite ver los pedidos, su ID y estado actual.
