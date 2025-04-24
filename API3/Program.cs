using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Get;
using Application.Leagues.UseCases.Update;
using Application.Leagues.UseCases;
//using Application.Matches.UseCases.Create;
//using Application.Matches.UseCases.Delete;
//using Application.Matches.UseCases.Get;
//using Application.Matches.UseCases.Update;
//using Application.Matches.UseCases;
using Application.PlayerStatistics.UseCases.Create;
using Application.PlayerStatistics.UseCases.Delete;
using Application.PlayerStatistics.UseCases.Get;
using Application.PlayerStatistics.UseCases.Update;
using Application.PlayerStatistics.UseCases;
using Application.Playes.UseCases.Create;
using Application.Playes.UseCases.Delete;
using Application.Playes.UseCases.Get;
using Application.Playes.UseCases.Update;
using Application.Playes.UseCases;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Application.Teams.UseCases;
using Application.Users.UseCases.Access;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Profile;
using Application.Users.UseCases.Update;
using Application.Users.UseCases;
using Domain.Ports.Leagues;
using Domain.Ports.Players;
using Domain.Ports.PlayerStatistics;
using Domain.Ports.Teams;
using Domain.Ports.Users;
using Domain.Services.Leagues;
//using Domain.Services.Matches;
using Domain.Services.Players;
using Domain.Services.PlayerStatistics;
using Domain.Services.Teams;
using Domain.Services.Users;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Repositories;
using Infrastructure.Persistence.Players.Repositories;
using Infrastructure.Persistence.PlayerStatistics.Repositories;
using Infrastructure.Persistence.Teams.Repositories;
using Infrastructure.Persistence.Users.Repositories;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Users.Mapper;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Net;
using Infrastructure.Services.Scraping.Teams;
using Infrastructure.Persistence.Players.Mapper;
using Application.TeamPlayers.UseCases.Create;
using Application.TeamPlayers.UseCases;
using Infrastructure.Services.Scraping.Players.Import;
using Infrastructure.Services.Scraping.Players.Services;
using Infrastructure.Services.Scraping.TeamPlayers.Imports;
using Infrastructure.Services.Scraping.Teams.Services;
using Domain.Ports.TeamPlayers;
using Infrastructure.Persistence.TeamPlayers.Repositories;
using Application.TeamPlayers.UseCases.Delete;
using Application.TeamPlayers.UseCases.Get;
using Application.TeamPlayers.UseCases.Update;
using Domain.Services.TeamPlayers;
using Infrastructure.Services.Scraping.Teams.Import;

var builder = WebApplication.CreateBuilder(args);

// Configurar Kestrel y puertos (para conexiones LAN/remotas)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000);
});

// Configurar conexión a base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar los mappers
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<Application.Leagues.Mapper.LeagueMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.Leagues.Mapper.LeagueMapper>();
builder.Services.AddScoped<Application.Matches.Mapper.MatchMapper>();
//builder.Services.AddScoped<Infrastructure.Persistence.Matches.Mapper.MatchMapper>();
builder.Services.AddScoped<Application.Teams.Mapper.TeamMapper>();
//builder.Services.AddScoped<Application.Playes.Mappers.PlayerMapper>();
builder.Services.AddScoped<PlayerMapper>();
builder.Services.AddScoped<Application.PlayerStatistics.Mappers.PlayerStatisticMapper>();
builder.Services.AddScoped<Infrastructure.Persistence.PlayerStatistics.Mapper.PlayerStatisticMapper>();


// Registrar los repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
//builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, TeamPlayerRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>();

// Registrar servicios de dominio
builder.Services.AddScoped<LeagueService>();
//builder.Services.AddScoped<MatchService>();
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
builder.Services.AddScoped<GuestLoginUseCase>();
builder.Services.AddScoped<GetUserProfileUseCase>();
builder.Services.AddScoped<UpdateUserProfileUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<GeneralUserUseCaseHandler>();

// Registrar casos de uso de Leagues
builder.Services.AddScoped<GetAllLeagues>();
builder.Services.AddScoped<GetLeagueById>();
builder.Services.AddScoped<CreateLeague>();
builder.Services.AddScoped<UpdateLeague>();
builder.Services.AddScoped<DeleteLeague>();
builder.Services.AddScoped<GeneralLeagueUseCaseHandler>();

// Registrar casos de uso de Matches
//builder.Services.AddScoped<GetAllMatches>();
//builder.Services.AddScoped<GetMatchById>();
//builder.Services.AddScoped<CreateMatch>();
//builder.Services.AddScoped<UpdateMatch>();
//builder.Services.AddScoped<DeleteMatch>();
//builder.Services.AddScoped<GeneralMatchUseCaseHandler>();

// Registrar casos de uso de Teams
builder.Services.AddScoped<CreateTeamUseCase>();
builder.Services.AddScoped<GetTeamByIdUseCase>();
builder.Services.AddScoped<UpdateTeamUseCase>();
builder.Services.AddScoped<DeleteTeamUseCase>();
builder.Services.AddScoped<GetAllTeamsUseCase>();
builder.Services.AddScoped<CreateTeamsFromScraperUseCase>();
builder.Services.AddScoped<TeamUseCaseHandler>();

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

builder.Services.AddHttpClient<TeamScraperService>();
builder.Services.AddHttpClient<PlayerScraperService>();

builder.Services.AddScoped<TeamImportService>();
builder.Services.AddScoped<PlayerImportService>();
builder.Services.AddScoped<TeamPlayerImportService>();

builder.Services.AddScoped<CreateTeamsFromScraperUseCase>();
builder.Services.AddScoped<CreatePlayersFromScraperUseCase>();
builder.Services.AddScoped<CreateTeamPlayersFromScraperUseCase>();

builder.Services.AddScoped<TeamUseCaseHandler>();
builder.Services.AddScoped<GeneralPlayerUseCaseHandler>();
builder.Services.AddScoped<GeneralTeamPlayerUseCaseHandler>();
builder.Services.AddScoped<CreateTeamPlayersFromScraperUseCase>();
builder.Services.AddScoped<GeneralTeamPlayerUseCaseHandler>();
builder.Services.AddScoped<CreateTeamPlayer>();
builder.Services.AddScoped<GetTeamPlayerByIds>();
builder.Services.AddScoped<GetTeamPlayersByTeamId>();
builder.Services.AddScoped<GetTeamPlayersByPlayerId>();
builder.Services.AddScoped<DeleteTeamPlayer>();
builder.Services.AddScoped<UpdateTeamPlayer>();
builder.Services.AddScoped<TeamPlayerService>();

// Swagger completo
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ScoreFlow API",
        Version = "v1"
    });
});


// Configurar JSON para enums como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


// CORS para permitir acceso desde otros dispositivos o frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScoreFlow API V1");
        c.RoutePrefix = string.Empty; // Muestra Swagger en la raíz (http://localhost:5000/)
    });
}


app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
