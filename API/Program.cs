using Infrastructure.Persistence.Conection;
using Microsoft.EntityFrameworkCore;
using Application.Users.UseCases;
using Domain.Ports.Users;
using Infrastructure.Persistence.Users.Repositories;
using Infrastructure.Persistence.Users.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexi�n desde el archivo de configuraci�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar el mapper
builder.Services.AddScoped<UserMapper>();  // Registrar UserMapper si no es est�tico y necesita inyecci�n de dependencias

// Registrar los repositorios y los casos de uso
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GeneralUserUseCase>();

// Configuraci�n de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar otros servicios necesarios
builder.Services.AddControllers();

var app = builder.Build();

// Aplicar migraciones al iniciar la aplicaci�n
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

// Configurar el middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
