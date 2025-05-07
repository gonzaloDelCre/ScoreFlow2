using System.Net;
using System.Text.Json.Serialization;
using Application.Leagues.UseCases;
using Application.Leagues.UseCases.Create;
using Application.Leagues.UseCases.Delete;
using Application.Leagues.UseCases.Get;
using Application.Leagues.UseCases.Update;
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
using Application.Teams.UseCases;
using Application.Teams.UseCases.Create;
using Application.Teams.UseCases.Delete;
using Application.Teams.UseCases.Get;
using Application.Teams.UseCases.Update;
using Application.Users.UseCases;
using Application.Users.UseCases.Access;
using Application.Users.UseCases.Create;
using Application.Users.UseCases.Delete;
using Application.Users.UseCases.Get;
using Application.Users.UseCases.Profile;
using Application.Users.UseCases.Update;
using Application.TeamPlayers.UseCases;
using Application.TeamPlayers.UseCases.Create;
using Application.TeamPlayers.UseCases.Delete;
using Application.TeamPlayers.UseCases.Get;
using Application.TeamPlayers.UseCases.Update;
using Domain.Ports.Leagues;
using Domain.Ports.Players;
using Domain.Ports.PlayerStatistics;
using Domain.Ports.Teams;
using Domain.Ports.TeamPlayers;
using Domain.Ports.Users;
using Domain.Services.Leagues;
using Domain.Services.Players;
using Domain.Services.PlayerStatistics;
using Domain.Services.Teams;
using Domain.Services.TeamPlayers;
using Domain.Services.Users;
using Infrastructure.Persistence.Conection;
using Infrastructure.Persistence.Leagues.Repositories;
using Infrastructure.Persistence.Players.Mapper;
using Infrastructure.Persistence.Players.Repositories;
using Infrastructure.Persistence.PlayerStatistics.Mapper;
using Infrastructure.Persistence.PlayerStatistics.Repositories;
using Infrastructure.Persistence.Teams.Repositories;
using Infrastructure.Persistence.TeamPlayers.Repositories;
using Infrastructure.Persistence.Users.Mapper;
using Infrastructure.Persistence.Users.Repositories;
using Infrastructure.Services.Scraping.Players.Import;
using Infrastructure.Services.Scraping.Players.Services;
using Infrastructure.Services.Scraping.TeamPlayers.Imports;
using Infrastructure.Services.Scraping.Teams.Import;
using Infrastructure.Services.Scraping.Teams.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// --- Configurar Kestrel (LAN/externo) ---
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000);
});

// --- Verificación de cadena de conexión ---
Console.WriteLine("🔍 ContentRootPath: " + builder.Environment.ContentRootPath);
var rawConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine("🔌 Connection string leída: " + rawConnectionString);

// --- DbContext ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(rawConnectionString, sql =>
        sql.MigrationsAssembly("Infrastructure"))
);

// --- Mappers ---
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<Application.Leagues.Mapper.LeagueMapper>();
builder.Services.AddScoped<Application.Teams.Mapper.TeamMapper>();
builder.Services.AddScoped<PlayerMapper>();
builder.Services.AddScoped<Application.PlayerStatistics.Mappers.PlayerStatisticMapper>();
builder.Services.AddScoped<PlayerStatisticMapper>();
builder.Services.AddScoped<Application.Standings.Mapper.StandingMapper>();

// --- Repositorios ---
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>();
builder.Services.AddScoped<ITeamPlayerRepository, TeamPlayerRepository>();
builder.Services.AddScoped<Domain.Ports.Standings.IStandingRepository, Infrastructure.Persistence.Standings.Repositories.StandingRepository>();

// --- Servicios de dominio ---
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LeagueService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<PlayerStatisticService>();
builder.Services.AddScoped<TeamPlayerService>();
builder.Services.AddScoped<Domain.Services.Standings.StandingService>();

// --- Casos de uso Users ---
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

// --- Casos de uso Leagues ---
builder.Services.AddScoped<CreateLeagueUseCase>();
builder.Services.AddScoped<UpdateLeagueUseCase>();
builder.Services.AddScoped<DeleteLeagueUseCase>();
builder.Services.AddScoped<GetAllLeaguesUseCase>();
builder.Services.AddScoped<GetLeagueByIdUseCase>();
builder.Services.AddScoped<LeagueUseCaseHandler>();

// --- Casos de uso Teams ---
builder.Services.AddScoped<CreateTeamUseCase>();
builder.Services.AddScoped<UpdateTeamUseCase>();
builder.Services.AddScoped<DeleteTeamUseCase>();
builder.Services.AddScoped<GetAllTeamsUseCase>();
builder.Services.AddScoped<GetTeamByIdUseCase>();
builder.Services.AddScoped<CreateTeamsFromScraperUseCase>();
builder.Services.AddScoped<ITeamImporter, TeamImportService>();
builder.Services.AddScoped<TeamUseCaseHandler>();

// --- Casos de uso Players ---
builder.Services.AddScoped<CreatePlayer>();
builder.Services.AddScoped<UpdatePlayer>();
builder.Services.AddScoped<DeletePlayer>();
builder.Services.AddScoped<GetAllPlayer>();
builder.Services.AddScoped<GetPlayerById>();
builder.Services.AddScoped<GeneralPlayerUseCaseHandler>();
builder.Services.AddScoped<IPlayerImporter, PlayerImportService>();

// --- Casos de uso PlayerStatistics ---
builder.Services.AddScoped<CreatePlayerStatistic>();
builder.Services.AddScoped<UpdatePlayerStatistic>();
builder.Services.AddScoped<DeletePlayerStatistic>();
builder.Services.AddScoped<GetAllPlayerStatistics>();
builder.Services.AddScoped<GetPlayerStatisticById>();
builder.Services.AddScoped<GeneralPlayerStatisticsUseCaseHandler>();

// --- Casos de uso TeamPlayers ---
builder.Services.AddScoped<CreateTeamPlayer>();
builder.Services.AddScoped<UpdateTeamPlayer>();
builder.Services.AddScoped<DeleteTeamPlayer>();
builder.Services.AddScoped<GetTeamPlayerByIds>();
builder.Services.AddScoped<GetTeamPlayersByPlayerId>();
builder.Services.AddScoped<GetTeamPlayersByTeamId>();
builder.Services.AddScoped<GetPlayersByTeamUseCase>();
builder.Services.AddScoped<IGetTeamRosterUseCase, GetTeamRosterUseCase>();
builder.Services.AddScoped<CreateTeamPlayersFromScraperUseCase>();
builder.Services.AddScoped<GeneralTeamPlayerUseCaseHandler>();

// --- Casos de uso Standings ---
builder.Services.AddScoped<Application.Standings.UseCases.Create.CreateStandingUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Update.UpdateStandingUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Delete.DeleteStandingUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Get.GetAllStandingsUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Get.GetStandingByIdUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Get.GetByLeagueUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Get.GetByTeamAndLeagueUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.Get.GetClassificationUseCase>();
builder.Services.AddScoped<Application.Standings.UseCases.StandingUseCaseHandler>();

// --- Scrapers y servicios externos ---
builder.Services.AddHttpClient<TeamScraperService>();
builder.Services.AddHttpClient<PlayerScraperService>();
builder.Services.AddScoped<TeamImportService>();
builder.Services.AddScoped<PlayerImportService>();
builder.Services.AddScoped<TeamPlayerImportService>();
// Para Players
builder.Services.AddScoped<CreatePlayersFromScraperUseCase>();
builder.Services.AddScoped<IPlayerImporter, PlayerImportService>();

// Para TeamPlayers
builder.Services.AddScoped<CreateTeamPlayersFromScraperUseCase>();
builder.Services.AddScoped<ITeamPlayerImporter, TeamPlayerImportService>();



// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScoreFlow API", Version = "v1" });
});

// --- JSON enum como string ---
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// --- Aplicar migraciones ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// --- Middleware ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScoreFlow API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
