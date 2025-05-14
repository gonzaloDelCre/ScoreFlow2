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
using Application.MatchEvents.UseCases;
using Application.MatchEvents.UseCases.Create;
using Application.MatchEvents.UseCases.Delete;
using Application.MatchEvents.UseCases.Get;
using Application.MatchEvents.UseCases.Update;
using Application.PlayerStatistics.UseCases;
using Application.PlayerStatistics.UseCases.Create;
using Application.PlayerStatistics.UseCases.Delete;
using Application.PlayerStatistics.UseCases.Get;
using Application.PlayerStatistics.UseCases.Update;
using Application.Playes.UseCases;
using Application.Playes.UseCases.Create;
using Application.Playes.UseCases.Delete;
using Application.Playes.UseCases.Get;
using Application.Playes.UseCases.Update;
using Application.Standings.UseCases;
using Application.Standings.UseCases.Create;
using Application.Standings.UseCases.Delete;
using Application.Standings.UseCases.Get;
using Application.TeamPlayers.UseCases;
using Application.TeamPlayers.UseCases.Create;
using Application.TeamPlayers.UseCases.Delete;
using Application.TeamPlayers.UseCases.Get;
using Application.TeamPlayers.UseCases.Update;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Application.Teams.UseCases;
using Domain.Ports.Leagues;
using Domain.Ports.Matches;
using Domain.Ports.MatchEvents;
using Domain.Ports.Players;
using Domain.Ports.PlayerStatistics;
using Domain.Ports.Standings;
using Domain.Ports.TeamPlayers;
using Domain.Ports.Teams;
using Domain.Ports.Users;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Mapper;
using Infrastructure.Persistence.Leagues.Repositories;
using Infrastructure.Persistence.Matches.Mapper;
using Infrastructure.Persistence.Matches.Repositories;
using Infrastructure.Persistence.MatchEvents.Mapper;
using Infrastructure.Persistence.MatchEvents.Repositories;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.Players.Repositories;
using Infrastructure.Persistence.PlayerStatistics.Mapper;
using Infrastructure.Persistence.PlayerStatistics.Repositories;
using Infrastructure.Persistence.Standings.Mapper;
using Infrastructure.Persistence.Standings.Repositories;
using Infrastructure.Persistence.TeamPlayers.Mapper;
using Infrastructure.Persistence.TeamPlayers.Mappers;
using Infrastructure.Persistence.TeamPlayers.Repositories;
using Infrastructure.Persistence.Teams.Mapper;
using Infrastructure.Persistence.Teams.Repositories;
using Infrastructure.Persistence.Users.Mapper;
using Infrastructure.Persistence.Users.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text.Json.Serialization;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Update;
using Application.Users.UseCases;
using Application.Leagues.UseCases.Scraping;
using Application.Matches.UseCases.Scraping;
using Application.Playes.UseCases.Scraping;
using Application.Standings.UseCases.Scraping;
using Application.Teams.UseCases.Scraping;
using Application.Standings.UseCases.Update;
using Infrastructure.Services.Scraping.Leagues.Services;
using Infrastructure.Services.Scraping.Matches.Import;
using Infrastructure.Services.Scraping.Players.Import;
using Infrastructure.Services.Scraping.Standings.Import;
using Infrastructure.Services.Scraping.Teams.Import;
using Infrastructure.Services.Scraping.Leagues.Import;
using Infrastructure.Services.Scraping.Matches.Services;
using Infrastructure.Services.Scraping.Players.Services;
using Infrastructure.Services.Scraping.Standings.Services;
using Infrastructure.Services.Scraping.Teams.Services;
using Application.TeamPlayers.UseCases.Scraping;

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
builder.Services.AddScoped<ILeagueMapper, LeagueMapper>();
builder.Services.AddScoped<IMatchMapper, MatchMapper>();
builder.Services.AddScoped<IMatchEventMapper, MatchEventMapper>();
builder.Services.AddScoped<IPlayerMapper, PlayerMapper>();
builder.Services.AddScoped<IPlayerStatisticMapper, PlayerStatisticMapper>();
builder.Services.AddScoped<IStandingMapper, StandingMapper>();
builder.Services.AddScoped<ITeamPlayerMapper, TeamPlayerMapper>();
builder.Services.AddScoped<ITeamMapper, TeamMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();


// Registrar los repositorios
builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchEventRepository, MatchEventRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>();
builder.Services.AddScoped<IStandingRepository, StandingRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, TeamPlayerRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Registrar servicios de dominio


// Registrar casos de uso de Leagues
builder.Services.AddScoped<CreateLeagueUseCase>();
builder.Services.AddScoped<DeleteLeagueUseCase>();
builder.Services.AddScoped<GetAllLeaguesUseCase>();
builder.Services.AddScoped<GetLeagueByIdUseCase>();
builder.Services.AddScoped<GetLeagueByNameUseCase>();
builder.Services.AddScoped<GetStandingsUseCase>();
builder.Services.AddScoped<UpdateLeagueUseCase>();
builder.Services.AddScoped<UpdateStandingsUseCase>();
builder.Services.AddScoped<ImportLeaguesUseCase>();
builder.Services.AddScoped<LeagueUseCaseHandler>();


// Registrar casos de uso de Matches
builder.Services.AddScoped<CreateMatchUseCase>();
builder.Services.AddScoped<DeleteMatchUseCase>();
builder.Services.AddScoped<GetAllMatchesUseCase>();
builder.Services.AddScoped<GetMatchByIdUseCase>();
builder.Services.AddScoped<GetMatchesByLeagueUseCase>();
builder.Services.AddScoped<GetMatchesByTeamUseCase>();
builder.Services.AddScoped<UpdateMatchUseCase>();
builder.Services.AddScoped<ImportMatchUseCase>();
builder.Services.AddScoped<MatchUseCaseHandler>();


// Registrar casos de uso de MatchEvents
builder.Services.AddScoped<CreateMatchEventUseCase>();
builder.Services.AddScoped<DeleteMatchEventUseCase>();
builder.Services.AddScoped<GetAllMatchEventsUseCase>();
builder.Services.AddScoped<GetMatchEventByIdUseCase>();
builder.Services.AddScoped<GetMatchEventsByMatchUseCase>();
builder.Services.AddScoped<GetMatchEventsByMinuteRangeUseCase>();
builder.Services.AddScoped<GetMatchEventsByPlayerUseCase>();
builder.Services.AddScoped<GetMatchEventsByTypeUseCase>();
builder.Services.AddScoped<UpdateMatchEventUseCase>();
builder.Services.AddScoped<MatchEventUseCaseHandler>();


//Registrar casos de uso de Players
builder.Services.AddScoped<CreatePlayerUseCase>();
builder.Services.AddScoped<DeletePlayerUseCase>();
builder.Services.AddScoped<GetAllPlayersUseCase>();
builder.Services.AddScoped<GetPlayerByIdUseCase>();
builder.Services.AddScoped<GetPlayerByNameUseCase>();
builder.Services.AddScoped<GetPlayersByAgeRangeUseCase>();
builder.Services.AddScoped<GetPlayersByPositionUseCase>();
builder.Services.AddScoped<GetPlayersByTeamUseCase>();
builder.Services.AddScoped<GetTopScorersUseCase>();
builder.Services.AddScoped<SearchPlayersByNameUseCase>();
builder.Services.AddScoped<UpdatePlayerUseCase>();
builder.Services.AddScoped<ImportPlayersByTeamExternalIdUseCase>();
builder.Services.AddScoped<PlayerUseCaseHandler>();


//Registrar casos de uso de Player Statistics
builder.Services.AddScoped<CreatePlayerStatisticUseCase>();
builder.Services.AddScoped<DeletePlayerStatisticUseCase>();
builder.Services.AddScoped<GetAllPlayerStatisticsUseCase>();
builder.Services.AddScoped<GetByMatchIdUseCase>();
builder.Services.AddScoped<GetByPlayerIdUseCase>();
builder.Services.AddScoped<GetPlayerStatisticByIdUseCase>();
builder.Services.AddScoped<UpdatePlayerStatisticUseCase>();
builder.Services.AddScoped<PlayerStatisticUseCaseHandler>();


// Registrar casos de uso de Standing
builder.Services.AddScoped<CreateStandingUseCase>();
builder.Services.AddScoped<DeleteStandingUseCase>();
builder.Services.AddScoped<GetAllStandingsUseCase>();
builder.Services.AddScoped<GetByGoalDifferenceRangeUseCase>();
builder.Services.AddScoped<GetClassificationByLeagueIdUseCase>();
builder.Services.AddScoped<GetStandingByIdUseCase>();
builder.Services.AddScoped<GetStandingByTeamAndLeagueUseCase>();
builder.Services.AddScoped<GetStandingsByLeagueIdUseCase>();
builder.Services.AddScoped<GetTopByPointsUseCase>();
builder.Services.AddScoped<UpdateStandingsUseCase>();
builder.Services.AddScoped<ImportStandingsUseCase>();
builder.Services.AddScoped<StandingUseCaseHandler>();


// Registrar casos de uso de TeamPlayer
builder.Services.AddScoped<CreateTeamPlayerUseCase>();
builder.Services.AddScoped<DeleteTeamPlayerUseCase>();
builder.Services.AddScoped<GetAllTeamPlayersUseCase>();
builder.Services.AddScoped<GetTeamPlayerByIdsUseCase>();
builder.Services.AddScoped<GetTeamPlayersByTeamUseCase>();
builder.Services.AddScoped<GetTeamPlayersByPlayerUseCase>();
builder.Services.AddScoped<GetTeamPlayersByRoleUseCase>();
builder.Services.AddScoped<GetTeamPlayersByJoinDateRangeUseCase>();
builder.Services.AddScoped<UpdateTeamPlayerUseCase>();
builder.Services.AddScoped<LinkPlayersToTeamUseCase>();
builder.Services.AddScoped<TeamPlayerUseCaseHandler>();


// Registrar casos de uso de Teams
builder.Services.AddScoped<CreateTeamUseCase>();
builder.Services.AddScoped<DeleteTeamUseCase>();
builder.Services.AddScoped<GetAllTeamsUseCase>();
builder.Services.AddScoped<GetTeamByIdUseCase>();
builder.Services.AddScoped<GetTeamByExternalIdUseCase>();
builder.Services.AddScoped<GetTeamsByCategoryUseCase>();
builder.Services.AddScoped<SearchTeamsByNameUseCase>();
builder.Services.AddScoped<GetTeamPlayersUseCase>();
builder.Services.AddScoped<UpdateTeamUseCase>();
builder.Services.AddScoped<ImportTeamsUseCase>();
builder.Services.AddScoped<TeamUseCaseHandler>();


// Registrar casos de uso de Users
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<DeleteUserUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<GetUserByIdUseCase>();
builder.Services.AddScoped<GetUsersByRoleUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();
builder.Services.AddScoped<UserUseCaseHandler>();





// Servicios Scraper
builder.Services.AddHttpClient<LeagueScraperService>();
builder.Services.AddHttpClient<MatchScraperService>();
builder.Services.AddHttpClient<PlayerScraperService>();
builder.Services.AddHttpClient<StandingsScraperService>();
builder.Services.AddHttpClient<TeamScraperService>();

// Servicios de Importación
builder.Services.AddScoped<LeagueImportService>();
builder.Services.AddScoped<MatchImportService>();
builder.Services.AddScoped<PlayerImportService>();
builder.Services.AddScoped<StandingsImportService>();
builder.Services.AddScoped<TeamImportService>();

// Casos de uso
builder.Services.AddScoped<ImportLeaguesUseCase>();
builder.Services.AddScoped<ImportMatchUseCase>();
builder.Services.AddScoped<ImportPlayersByTeamExternalIdUseCase>();
builder.Services.AddScoped<ImportStandingsUseCase>();
builder.Services.AddScoped<ImportTeamsUseCase>();

// Handlers
builder.Services.AddScoped<LeagueUseCaseHandler>();
builder.Services.AddScoped<MatchUseCaseHandler>();
builder.Services.AddScoped<PlayerUseCaseHandler>();
builder.Services.AddScoped<StandingUseCaseHandler>();
builder.Services.AddScoped<TeamUseCaseHandler>();
builder.Services.AddScoped<UserUseCaseHandler>();

// Casos de uso faltantes en los handlers
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<GetTeamStandingsUseCase>();
builder.Services.AddScoped<UpdateStandingUseCase>();
builder.Services.AddTransient<StandingsImportService>();


// Registrar el caso de uso faltante
builder.Services.AddScoped<Application.Users.UseCases.Update.ChangeUserPasswordUseCase>();

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
