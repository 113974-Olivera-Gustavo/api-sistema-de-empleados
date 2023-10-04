using api_sistema_de_empleados.CQRS.Commands.Clientes;
using api_sistema_de_empleados.CQRS.Commands.Empleados;
using api_sistema_de_empleados.Data;
using FluentValidation;
using System.Reflection;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.DeleteCliente;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.PostCliente;
using static api_sistema_de_empleados.CQRS.Commands.Clientes.PutCliente;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.DeleteEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PostEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Empleados.PutEmpleado;
using static api_sistema_de_empleados.CQRS.Commands.Login.PostLogin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddDbContext<ApplicationContext>();
builder.Services.AddScoped<IValidator<PostLoginCommand>, PostLoginCommandValidator>();
builder.Services.AddScoped<IValidator<PostEmpleadoCommand>, PostEmpleadoCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteEmpleadoCommand>, DeleteEmpleadoCommandValidator>();
builder.Services.AddScoped<IValidator<DeleteClienteCommand>, DeleteClienteValidator>();
builder.Services.AddScoped<IValidator<PutEmpleadoCommand>, PutEmpleadoCommandValidator>();
builder.Services.AddScoped<IValidator<PutClienteCommand>, PutClienteCommandValidator>();
builder.Services.AddScoped<IValidator<PostClienteCommand>, PostClienteCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
