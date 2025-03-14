using Microsoft.EntityFrameworkCore;
using Application.Users.UseCases;
using Domain.Ports.Users;
using Infrastructure.Persistence.Users.Repositories;
using Infrastructure.Persistence.Users.Mapper;
using Infrastructure.Persistence.Conection;
using Application.Leagues.UseCases;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Get;
using Application.Leagues.UseCases.Update;
using Domain.Ports.Leagues;
using Infrastructure.Persistence.Leagues.Repositories;
using Application.Leagues.Mapper;
using Domain.Services.Leagues;
using Application.Matches.UseCases.Create;
using Application.Matches.UseCases.Delete;
using Application.Matches.UseCases.Get;
using Application.Matches.UseCases.Update;
using Application.Matches.UseCases;
using Domain.Services.Matches;
using Domain.Ports.Matches;
using Infrastructure.Persistence.Matches.Repositories;
using Domain.Ports.Teams;
using Infrastructure.Persistence.Teams.Repositories;
using Application.Teams.UseCases.Create;
using Domain.Services.Teams;
using Application.Teams.Mapper;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Application.Teams.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar el mapper
builder.Services.AddScoped<UserMapper>();  // Registrar UserMapper si no es estático y necesita inyección de dependencias

// Registrar los repositorios y los casos de uso
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GeneralUserUseCase>();  // Este es el "puente" que redirige a otros casos de uso

// Para la capa Application
builder.Services.AddScoped<Application.Leagues.Mapper.LeagueMapper>();

// Para la capa Infrastructure
builder.Services.AddScoped<Infrastructure.Persistence.Leagues.Mapper.LeagueMapper>();

builder.Services.AddScoped<IMatchRepository, MatchRepository>();

builder.Services.AddScoped<MatchService>();
builder.Services.AddScoped<GetAllMatches>();
builder.Services.AddScoped<GetMatchById>();
builder.Services.AddScoped<CreateMatch>();
builder.Services.AddScoped<UpdateMatch>();
builder.Services.AddScoped<DeleteMatch>();

// Registrar el handler de casos de uso generales
builder.Services.AddScoped<GeneralMatchUseCaseHandler>();
builder.Services.AddScoped<Infrastructure.Persistence.Matches.Mapper.MatchMapper>();
builder.Services.AddScoped<Application.Matches.Mapper.MatchMapper>();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<TeamService>();  // Servicio que gestiona la lógica de negocios del equipo
builder.Services.AddScoped<CreateTeam>();  // Caso de uso para crear el equipo
builder.Services.AddScoped<TeamMapper>();
builder.Services.AddTransient<GetTeamById>();
builder.Services.AddTransient<CreateTeam>();
builder.Services.AddTransient<UpdateTeam>();
builder.Services.AddTransient<DeleteTeam>();
builder.Services.AddTransient<GetAllTeams>();
builder.Services.AddTransient<GeneralTeamUseCaseHandler>();

builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<GetAllLeagues>();
builder.Services.AddScoped<GetLeagueById>();
builder.Services.AddScoped<AddLeague>();
builder.Services.AddScoped<UpdateLeague>();
builder.Services.AddScoped<DeleteLeague>();
builder.Services.AddScoped<GeneralLeagueUseCaseHandler>();
builder.Services.AddScoped<LeagueService>();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar otros servicios necesarios
builder.Services.AddControllers();

var app = builder.Build();

// Aplicar migraciones al iniciar la aplicación
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
