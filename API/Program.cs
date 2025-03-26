using Microsoft.EntityFrameworkCore;
using Application.Users.UseCases;
using Application.Leagues.UseCases;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Get;
using Application.Leagues.UseCases.Update;
using Application.Matches.UseCases;
using Application.Matches.UseCases.Create;
using Application.Matches.UseCases.Delete;
using Application.Matches.UseCases.Get;
using Application.Matches.UseCases.Update;
using Application.Teams.UseCases;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Application.Leagues.Mapper;
using Application.Teams.Mapper;
using Application.Matches.Mapper;
using Domain.Ports.Users;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.Teams;
using Domain.Services.Leagues;
using Domain.Services.Matches;
using Domain.Services.Teams;
using Infrastructure.Persistence.Users.Repositories;
using Infrastructure.Persistence.Leagues.Repositories;
using Infrastructure.Persistence.Matches.Repositories;
using Infrastructure.Persistence.Teams.Repositories;
using Infrastructure.Persistence.Users.Mapper;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Matches.Mapper;
using Infrastructure.Persistence.Conection;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Update;
using Domain.Services.Users;
using Infrastructure.Persistence.Players.Mapper;
using Domain.Ports.Players;
using Infrastructure.Persistence.Players.Repositories;
using Domain.Services.Players;
using Application.Playes.UseCases.Delete;
using Application.Playes.UseCases.Get;
using Application.Playes.UseCases.Update;
using Application.Playes.UseCases;
using Application.Playes.UseCases.Create;
using System.Text.Json.Serialization;
using Application.PlayerStatistics.UseCases.Create;
using Application.PlayerStatistics.UseCases;
using Application.PlayerStatistics.UseCases.Get;
using Application.PlayerStatistics.UseCases.Update;
using Application.PlayerStatistics.UseCases.Delete;
using Domain.Entities.PlayerStatistics;
using Domain.Services.PlayerStatistics;
using Domain.Ports.PlayerStatistics;
using Infrastructure.Persistence.PlayerStatistics.Repositories;
using Amazon.Lambda.AspNetCoreServer;
using Microsoft.OpenApi.Models;
using API;
using Application.Users.UseCases.Access;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

var apiGatewayUrl = builder.Configuration.GetValue<string>("ApiGateway:BaseUrl");
var apiEndpoints = builder.Configuration.GetSection("ApiGateway:Endpoints").Get<Dictionary<string, string>>();

// Obtener la cadena de conexión desde el archivo de configuración
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 8080);  // Cambiar a 8080 si es necesario
});

builder.Services.AddTransient<RegisterUserUseCase>();
builder.Services.AddTransient<GeneralUserUseCaseHandler>();

// Configurar el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar los mappers
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<Application.Leagues.Mapper.LeagueMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.Leagues.Mapper.LeagueMapper>();
builder.Services.AddScoped<Application.Matches.Mapper.MatchMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.Matches.Mapper.MatchMapper>();
builder.Services.AddScoped<TeamMapper>();
builder.Services.AddScoped<Application.Playes.Mappers.PlayerMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.Players.Mapper.PlayerMapper>();
builder.Services.AddScoped<Application.PlayerStatistics.Mappers.PlayerStatisticMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.PlayerStatistics.Mapper.PlayerStatisticMapper>();


// Registrar los repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>();

// Registrar servicios de dominio
builder.Services.AddScoped<LeagueService>();
builder.Services.AddScoped<MatchService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<PlayerStatisticService>();

// Registrar casos de uso de Users
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<GetUserByEmailUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<GeneralUserUseCaseHandler>();

// Registrar casos de uso de Leagues
builder.Services.AddScoped<GetAllLeagues>();
builder.Services.AddScoped<GetLeagueById>();
builder.Services.AddScoped<CreateLeague>();
builder.Services.AddScoped<UpdateLeague>();
builder.Services.AddScoped<DeleteLeague>();
builder.Services.AddScoped<GeneralLeagueUseCaseHandler>();

// Registrar casos de uso de Matches
builder.Services.AddScoped<GetAllMatches>();
builder.Services.AddScoped<GetMatchById>();
builder.Services.AddScoped<CreateMatch>();
builder.Services.AddScoped<UpdateMatch>();
builder.Services.AddScoped<DeleteMatch>();
builder.Services.AddScoped<GeneralMatchUseCaseHandler>();

// Registrar casos de uso de Teams
builder.Services.AddScoped<CreateTeam>();
builder.Services.AddScoped<GetTeamById>();
builder.Services.AddScoped<UpdateTeam>();
builder.Services.AddScoped<DeleteTeam>();
builder.Services.AddScoped<GetAllTeams>();
builder.Services.AddScoped<GeneralTeamUseCaseHandler>();

//Registrar casos de uso de Players
builder.Services.AddScoped<CreatePlayer>();
builder.Services.AddScoped<GetAllPlayer>();  
builder.Services.AddScoped<GetPlayerById>();  
builder.Services.AddScoped<UpdatePlayer>(); 
builder.Services.AddScoped<DeletePlayer>();  
builder.Services.AddScoped<GeneralPlayerUseCaseHandler>();

//Registrar casos de uso de Player Statistics
builder.Services.AddScoped<CreatePlayerStatistic>();
builder.Services.AddScoped<GetAllPlayerStatistics>();
builder.Services.AddScoped<GetPlayerStatisticById>();
builder.Services.AddScoped<UpdatePlayerStatistic>();
builder.Services.AddScoped<DeletePlayerStatistic>();
builder.Services.AddScoped<GeneralPlayerStatisticsUseCaseHandler>();


// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScoreFlow API", Version = "v1" });
});

// Registrar otros servicios necesarios
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScoreFlow API V1");
        c.RoutePrefix = string.Empty; 
    });
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
