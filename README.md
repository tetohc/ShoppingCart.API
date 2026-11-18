
# ShoppingCart API

API REST para la gestión de un carrito de compras. Expone operaciones para consultar categorías y productos, registrar productos con imagen y crear y consultar órdenes.

La API es consumida por una aplicación frontend desarrollada en **Angular** [`ShoppingCart`](https://github.com/tetohc/Angular-CarritoCompras.git). En desarrollo, la política CORS está configurada para permitir el origen `http://localhost:4200`.

## Tecnologías

- **C# y .NET 10 / ASP.NET Core Web API** para implementar los servicios HTTP.
- **Entity Framework Core 10** como ORM y **SQL Server** como base de datos relacional.
- **Swagger / OpenAPI (Swashbuckle)** para explorar y probar los endpoints.
- **xUnit** para pruebas unitarias, con **Moq** para dobles de prueba, **FluentAssertions** para aserciones y **EF Core InMemory** para pruebas de acceso a datos sin SQL Server.
- Almacenamiento local de imágenes de productos en `wwwroot/uploads/products`.

## Funcionalidades

- Consultar las categorías disponibles.
- Consultar productos y filtrar por categoría; la respuesta incluye información de categoría, precio, existencias y URL de imagen.
- Registrar productos con una imagen enviada como formulario (`multipart/form-data`).
- Crear órdenes con sus detalles y actualizar las existencias de los productos incluidos.
- Consultar órdenes con fecha, importe y detalles de productos.
- Servir archivos estáticos (incluidas las imágenes cargadas) desde la API.

## Arquitectura en capas

El proyecto separa responsabilidades en las siguientes carpetas:

| Capa | Ubicación | Responsabilidad |
|---|---|---|
| **Controllers** | `ShoppingCart.API/Controllers` | Define los endpoints HTTP, recibe solicitudes, invoca la lógica de negocio y devuelve respuestas. Incluye controladores para categorías, productos y órdenes. |
| **Business** | `ShoppingCart.API/Business` | Implementa las operaciones y reglas de negocio para categorías, productos, órdenes y almacenamiento de archivos. Sus contratos están en `Business/Interfaces`. |
| **Database** | `ShoppingCart.API/Database` | Contiene `AppDbContext`, el contexto de Entity Framework Core que expone las entidades y configura las relaciones con SQL Server. |
| **Model** | `ShoppingCart.API/Models` | Contiene las entidades persistidas (`Entities`) y los modelos de solicitud y respuesta de la API (`DTOs`). |

Flujo general: **Angular → Controllers → Business → AppDbContext / SQL Server**. La carga de imágenes se delega a `IFileStorageBusiness` y su implementación local.

## Endpoints principales

Las rutas no llevan prefijo `/api`:

| Método | Ruta | Descripción |
|---|---|---|
| `GET` | `/Categories` | Lista las categorías. |
| `GET` | `/Products` | Lista productos; admite `categoryId` como parámetro de consulta (`0` devuelve todas las categorías). |
| `POST` | `/Products` | Registra un producto y su imagen mediante formulario. |
| `GET` | `/Orders` | Lista las órdenes y sus detalles. |
| `POST` | `/Orders` | Crea una orden y sus detalles. |

La documentación interactiva de Swagger se sirve en la raíz (`/`) al iniciar la API.

## Configuración y ejecución

Requisitos: **.NET SDK 10** y una instancia de **SQL Server** accesible.

1. Configure la cadena `ConnectionStrings:SQLConnectionString` en `ShoppingCart.API/appsettings.Development.json` o mediante la configuración segura de su entorno.  
2. Desde la raíz del repositorio, ejecute:

   ```powershell
   dotnet run --project .\ShoppingCart.API\ShoppingCart.API.csproj
   ```

3. Abra la dirección local indicada por la aplicación para acceder a Swagger.


## Pruebas

El proyecto de pruebas cubre lógica de negocio de categorías, productos y almacenamiento local de archivos. Para ejecutarlas desde la raíz:

```powershell
dotnet test .\ShoppingCart.Tests\ShoppingCart.Tests.csproj
```

Las pruebas usan una base de datos EF Core InMemory y mocks, por lo que las pruebas unitarias no requieren conectarse a SQL Server.

## Capturas de pantalla

### Swagger
![Catálogo de productos](https://raw.githubusercontent.com/tetohc/MediaResources/refs/heads/main/images/covers/ShoppingCart/swagger.png)