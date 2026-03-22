# PruebaTecnicaBackAPI

API REST desarrollada en **.NET 10** usando **Minimal API**, con CRUD de users y addresses, currencies, seguridad por API Key y patrón CQRS.

---

## Tecnologías utilizadas

- .NET 10
- Minimal API
- Entity Framework Core con SQLite
- FluentValidation
- BCrypt.Net (hash de contraseñas)
- Patrón CQRS (Commands/Queries)

---

## Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

---

## Cómo correr el proyecto

```bash
# Clonar el repositorio
git clone https://github.com/rudyca16-ai/PruebaTecnicaBackAPI.git
cd PruebaTecnicaBackAPI

# Restaurar dependencias
dotnet restore

# Correr el proyecto
dotnet run
```

La base de datos se crea y las migraciones se aplican automáticamente al iniciar la aplicación.

Swagger estará disponible en:
```
http://localhost:{puerto}/swagger/index.html
```

---

## Base de datos (SQLite)

El proyecto usa **SQLite** con Entity Framework Core. Al iniciar la aplicación, se crea automáticamente el archivo `app.db` y se aplican todas las migraciones pendientes, no es necesario correr ningún comando de EF manualmente.

---

## Seguridad por API Key

Todos los endpoints requieren el header `X-Api-Key`.

### Header requerido

| Header      | Valor            |
|-------------|------------------|
| `X-Api-Key` | `X7kP2mNqR9vL4wB8hJ3cF6tY1sA5eD0g` |

### Ejemplo con curl

```bash
curl -X 'GET' \
  'http://localhost:{puerto}/api/users' \
  -H 'accept: */*'
```

---

## Endpoints disponibles

### Users

| Método   | Ruta                  | Descripción              |
|----------|-----------------------|--------------------------|
| `GET`    | `/api/users`          | Listar usuarios (con filtros) |
| `POST`   | `/api/users`          | Crear usuario            |
| `GET`    | `/api/users/{id}`     | Obtener usuario por ID   |
| `PUT`    | `/api/users/{id}`     | Actualizar usuario       |
| `DELETE` | `/api/users/{id}`     | Eliminar usuario         |

### Addresses

| Método   | Ruta                              | Descripción                   |
|----------|-----------------------------------|-------------------------------|
| `POST`   | `/api/users/{userId}/addresses`   | Agregar dirección a usuario   |
| `GET`    | `/api/users/{userId}/addresses`   | Listar direcciones de usuario |
| `PUT`    | `/api/addresses/{id}`             | Actualizar dirección          |
| `DELETE` | `/api/addresses/{id}`             | Eliminar dirección            |

### Currency

| Método | Ruta                    | Descripción              |
|--------|-------------------------|--------------------------|
| `GET` | `/api/currencies` | Listar Conversiones  |
| `POST` | `/api/currencies` | Crear conversión  |
| `POST` | `/api/currencies/convert` | Convertir entre divisas  |

---

## Ejemplos de requests

### Crear usuario

```http
POST /api/users
X-Api-Key: X7kP2mNqR9vL4wB8hJ3cF6tY1sA5eD0g
Content-Type: application/json

{
  "name": "Juan Pérez",
  "email": "juan@example.com"
}
```

### Convertir divisa

```http
POST /api/currency/convert
X-Api-Key: X7kP2mNqR9vL4wB8hJ3cF6tY1sA5eD0g
Content-Type: application/json

{
  "fromCurrencyCode": "USD",
  "toCurrencyCode": "PYG",
  "amount": 100
}
```

---

### Estructura final
```
PruebaTecnicaBackAPI/
  Users/
    Commands/
      CreateUserCommand.cs
      CreateUserCommandHandler.cs
      DeleteUserCommand.cs
      DeleteUserCommandHandler.cs
      UpdateUserCommand.cs
      UpdateUserCommandHandler.cs
    Queries/
      GetUserByIdQuery.cs
      GetUserByIdQueryHandler.cs
      GetUsersQuery.cs
      GetUsersQueryHandler.cs
    DTOs/
      CreateUserDTO.cs
      UpdateUserDTO.cs
      UserResponseDTO.cs
      UserFilterDTO.cs
    Validators/
      CreateUserValidator.cs
      UpdateUserValidator.cs
  Addresses/
    Commands/
      CreateAddressCommand.cs
      CreateAddressCommandHandler.cs
      DeleteAddressCommand.cs
      DeleteAddressCommandHandler.cs
      UpdateAddressCommand.cs
      UpdateAddressCommandHandler.cs
    Queries/
      GetAddressesByUserQuery.cs
      GetAddressesByUserQueryHandler.cs
    DTOs/
      CreateAddressDTO.cs
      UpdateAddressDTO.cs
      AddressResponseDTO.cs
    Validators/
      CreateAddressValidator.cs
      UpdateAddressValidator.cs
  Currencies/
    Commands/
      CreateCurrencyCommand.cs
      CreateCurrencyCommandHandler.cs
      ConvertCurrencyCommand.cs
      ConvertCurrencyCommandHandler.cs
    Queries/
      GetCurrenciesQuery.cs
      GetCurrenciesQueryHandler.cs
    DTOs/
      CreateCurrencyDTO.cs
      CurrencyResponseDTO.cs
      ConvertCurrencyDTO.cs
      ConversionResultDTO.cs
    Validators/
      CreateCurrencyValidator.cs
      ConvertCurrencyValidator.cs
  Data/
    AppDbContext.cs
  Middleware/
    ApiKeyMiddleware.cs
  Migrations/
  Models/
    Address.cs
    Currency.cs
    User.cs
  app.db
  Program.cs

```

---

## Qué está implementado

- [x] CRUD completo de Users
- [x] CRUD completo de Addresses (relacionados 1:N a Users)
- [x] Creación, listado y conversión de divisas usando tabla Currency
- [x] Seguridad por API Key (header `X-Api-Key`)
- [x] Entity Framework Core con SQLite
- [x] FluentValidation en requests
- [x] Patrón CQRS (Commands/Queries)
- [x] Hash de contraseñas con BCrypt
- [x] Swagger/OpenAPI documentado con tags por módulo

## Qué no está implementado
- [x] Nada, se implementaron todos los puntos solicitados según el documento