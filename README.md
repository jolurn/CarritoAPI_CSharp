# Carrito de Compras API 🛒

Este es un proyecto de una API REST desarrollada en **C# y .NET** para gestionar un carrito de compras, aplicando una arquitectura híbrida (CQRS Lite) y patrones de diseño modernos.

## 🛠️ Tecnologías y Patrones Utilizados

*   **Entity Framework Core (ORM):** Utilizado para las operaciones de escritura (POST) garantizando la integridad de los datos.
*   **Dapper (MicroORM):** Utilizado para las operaciones de lectura (GET), optimizando el rendimiento.
*   **DTOs (Data Transfer Objects):** Implementados para separar las entidades de la base de datos de lo que se expone en la API, mejorando la seguridad.
*   **Data Annotations:** Validaciones automáticas en los DTOs de entrada (ej. campos requeridos, rangos de precios).
*   **Colecciones Avanzadas:** Uso de `Dictionary` para búsquedas rápidas O(1) y `HashSet` para agrupar productos sin duplicados en el carrito.

## 🚀 Endpoints Principales

*   `GET /api/Productos` - Lista todos los productos (Usa Dapper).
*   `POST /api/Productos` - Registra un nuevo producto (Usa EF Core y DTOs).
*   `POST /api/Productos/comprar` - Procesa el carrito de compras, calcula el total y valida el stock usando colecciones.

## ⚙️ Cómo ejecutar el proyecto

1. Clona este repositorio.
2. Abre el archivo `appsettings.json` y configura tu cadena de conexión a SQL Server (`DefaultConnection`).
3. Ejecuta el script SQL para crear la base de datos y la tabla `Productos`.
4. Ejecuta el proyecto en Visual Studio y prueba los endpoints desde Swagger (`/swagger`).
