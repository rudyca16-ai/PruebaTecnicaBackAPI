using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PruebaTecnicaBackAPI.Addresses.Commands;
using PruebaTecnicaBackAPI.Addresses.DTOs;
using PruebaTecnicaBackAPI.Addresses.Queries;
using PruebaTecnicaBackAPI.Currencies.Commands;
using PruebaTecnicaBackAPI.Currencies.DTOs;
using PruebaTecnicaBackAPI.Currencies.Queries;
using PruebaTecnicaBackAPI.Data;
using PruebaTecnicaBackAPI.Middleware;
using PruebaTecnicaBackAPI.Users.Commands;
using PruebaTecnicaBackAPI.Users.DTOs;
using PruebaTecnicaBackAPI.Users.Queries;
using PruebaTecnicaBackAPI.Users.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

// Handlers
// User
builder.Services.AddScoped<GetUsersQueryHandler>();
builder.Services.AddScoped<GetUserByIdQueryHandler>();
builder.Services.AddScoped<CreateUserCommandHandler>();
builder.Services.AddScoped<UpdateUserCommandHandler>();
builder.Services.AddScoped<DeleteUserCommandHandler>();

// Address
builder.Services.AddScoped<CreateAddressCommandHandler>();
builder.Services.AddScoped<UpdateAddressCommandHandler>();
builder.Services.AddScoped<DeleteAddressCommandHandler>();
builder.Services.AddScoped<GetAddressesByUserQueryHandler>();

// Currency
builder.Services.AddScoped<GetCurrenciesQueryHandler>();
builder.Services.AddScoped<CreateCurrencyCommandHandler>();
builder.Services.AddScoped<ConvertCurrencyCommandHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ApiKeyMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Grupos Rutas
var users = app.MapGroup("/api/users").WithTags("Users");
var addresses = app.MapGroup("/api/addresses").WithTags("Addresses");
var currencies = app.MapGroup("/api/currencies").WithTags("Currencies");

// User ----------------------------
// GET /api/users
users.MapGet("/", async ([AsParameters] UserFilterDTO filter, GetUsersQueryHandler handler) =>
    Results.Ok(await handler.HandleAsync(filter)));

// GET /api/users/{userId}
users.MapGet("/{id:int}", async (int id, GetUserByIdQueryHandler handler) =>
{
    var user = await handler.HandleAsync(new GetUserByIdQuery(id));
    return user is null
        ? Results.NotFound(new { error = "Usuario no encontrado." })
        : Results.Ok(user);
});

// POST /api/users
users.MapPost("/", async (
    CreateUserDTO dto,
    IValidator<CreateUserDTO> validator,
    CreateUserCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
               .GroupBy(e => e.PropertyName)
               .ToDictionary(
                   g => g.Key,
                   g => g.Select(e => e.ErrorMessage).ToArray()
               ));

    try
    {
        var user = await handler.HandleAsync(new CreateUserCommand(dto));
        return Results.Created($"/api/users/{user.Id}", user);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});

// PUT /api/users/{userId}
users.MapPut("/{id:int}", async (
    int id,
    UpdateUserDTO dto,
    IValidator<UpdateUserDTO> validator,
    UpdateUserCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
             ));

    try
    {
        var user = await handler.HandleAsync(new UpdateUserCommand(id, dto));
        return user is null
            ? Results.NotFound(new { error = "Usuario no encontrado." })
            : Results.Ok(user);
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});

// DELETE /api/users/{userId}
users.MapDelete("/{id:int}", async (int id, DeleteUserCommandHandler handler) =>
{
    var deleted = await handler.HandleAsync(new DeleteUserCommand(id));
    return deleted
        ? Results.NoContent()
        : Results.NotFound(new { error = "Usuario no encontrado." });
});

// Address ----------------------------
// POST /api/users/{userId}/addresses
users.MapPost("/{userId:int}/addresses", async (
    int userId,
    CreateAddressDTO dto,
    IValidator<CreateAddressDTO> validator,
    CreateAddressCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
             ));

    try
    {
        var address = await handler.HandleAsync(new CreateAddressCommand(userId, dto));
        return Results.Created($"/api/users/{userId}/addresses/{address.Id}", address);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithTags("Addresses");

// GET /api/users/{userId}/addresses
users.MapGet("/{userId:int}/addresses", async (
    int userId,
    GetAddressesByUserQueryHandler handler) =>
{
    try
    {
        var addresses = await handler.HandleAsync(new GetAddressesByUserQuery(userId));
        return Results.Ok(addresses);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
}).WithTags("Addresses");

// PUT /api/addresses/{id}
addresses.MapPut("/{id:int}", async (
    int id,
    UpdateAddressDTO dto,
    IValidator<UpdateAddressDTO> validator,
    UpdateAddressCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
             ));

    var address = await handler.HandleAsync(new UpdateAddressCommand(id, dto));
    return address is null
        ? Results.NotFound(new { error = "Dirección no encontrada." })
        : Results.Ok(address);
});

// DELETE /api/addresses/{id}
addresses.MapDelete("/{id:int}", async (
    int id,
    DeleteAddressCommandHandler handler) =>
{
    var deleted = await handler.HandleAsync(new DeleteAddressCommand(id));
    return deleted
        ? Results.NoContent()
        : Results.NotFound(new { error = "Dirección no encontrada." });
});

// Currency ----------------------------
// GET /api/currencies
currencies.MapGet("/", async (GetCurrenciesQueryHandler handler) =>
    Results.Ok(await handler.GetAllAsync()));

// POST /api/currencies
currencies.MapPost("/", async (
    CreateCurrencyDTO dto,
    IValidator<CreateCurrencyDTO> validator,
    CreateCurrencyCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
             ));

    var currency = await handler.CreateAsync(new CreateCurrencyCommand(dto));
    return Results.Created($"/api/currencies/{currency.Id}", currency);
});

// POST /api/currency/convert
currencies.MapPost("/convert", async (
    ConvertCurrencyDTO dto,
    IValidator<ConvertCurrencyDTO> validator,
    ConvertCurrencyCommandHandler handler) =>
{
    var result = await validator.ValidateAsync(dto);
    if (!result.IsValid)
        return Results.ValidationProblem(result.Errors
            .GroupBy (e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
             ));

    try
    {
        var conversion = await handler.ConvertAsync(new ConvertCurrencyCommand(dto));
        return Results.Ok(conversion);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(new { error = ex.Message });
    }
});

app.Run();
