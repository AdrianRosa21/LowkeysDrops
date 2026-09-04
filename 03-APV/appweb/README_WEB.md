# Lowkeys Drops - Web App (Spring Boot)

Esta es la Fase 3 del proyecto Lowkeys Drops, una aplicación web de streetwear desarrollada con Spring Boot y Thymeleaf.
La aplicación se comunica con la API de .NET (Fase 2) y no tiene conexión directa a base de datos.

## Estructura del Proyecto

El proyecto está organizado siguiendo una arquitectura MVC:
- `controller/`: Controladores Spring MVC (Admin, Auth, Catalogo, Cliente, Repartidor).
- `client/`: `ApiClient` que envuelve `RestClient` para peticiones HTTP e inyecta el token JWT.
- `service/`: Lógica de negocio y llamados a `ApiClient`.
- `dto/`: Clases de intercambio de datos equivalentes a la API .NET.
- `exception/`: Excepciones personalizadas como `ApiException`.
- `interceptor/`: `AuthInterceptor` para proteger rutas según el rol (ADMIN, CLIENTE, REPARTIDOR).

## Tecnologías Utilizadas

- **Java 17**
- **Spring Boot 3.3.3**
- **Spring Web** (RestClient para consumir API REST)
- **Thymeleaf** (Renderizado de vistas)
- **CSS3 Puro** (Diseño Dark Theme)

## Cómo Ejecutar

1. Iniciar la API .NET (Fase 2) asegurándose de que corra en el puerto 5069. (Revisar `lowkeys.api.base-url` en `application.properties`).
2. Abrir una terminal en este directorio (`03-Web/ProyectoSpring`).
3. Ejecutar `./mvnw spring-boot:run`.
4. Ingresar a `http://localhost:8080` en su navegador.
