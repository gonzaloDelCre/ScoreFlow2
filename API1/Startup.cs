using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using API1.Controllers.Users;
using Infrastructure.ApiClients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var apiGatewayUrl = Configuration.GetValue<string>("ApiGateway:BaseUrl");
            var apiEndpoints = Configuration.GetSection("ApiGateway:Endpoints").Get<Dictionary<string, string>>();
            services.AddHttpClient<ApiGatewayService>();
            services.AddScoped<ApiGatewayService>();
            services.AddScoped<IApiGateway, ApiGateway>();
            services.AddHttpClient<ApiGatewayClient>();
            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrar los mappers
            services.AddScoped<UserMapper>();
            services.AddScoped<Application.Leagues.Mapper.LeagueMapper>();
            services.AddScoped<Infrastructure.Persistence.Leagues.Mapper.LeagueMapper>();
            services.AddScoped<Application.Matches.Mapper.MatchMapper>();
            services.AddScoped<Infrastructure.Persistence.Matches.Mapper.MatchMapper>();
            services.AddScoped<TeamMapper>();
            services.AddScoped<Application.Playes.Mappers.PlayerMapper>();
            services.AddScoped<Infrastructure.Persistence.Players.Mapper.PlayerMapper>();
            services.AddScoped<Application.PlayerStatistics.Mappers.PlayerStatisticMapper>();
            services.AddScoped<Infrastructure.Persistence.PlayerStatistics.Mapper.PlayerStatisticMapper>();

            // Registrar los repositorios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IPlayerStatisticRepository, PlayerStatisticRepository>();

            // Registrar servicios de dominio
            services.AddScoped<LeagueService>();
            services.AddScoped<MatchService>();
            services.AddScoped<TeamService>();
            services.AddScoped<Domain.Services.Users.UserService>();
            services.AddScoped<PlayerService>();
            services.AddScoped<PlayerStatisticService>();

            // Registrar casos de uso de Users
            services.AddScoped<CreateUserUseCase>();
            services.AddScoped<UpdateUserUseCase>();
            services.AddScoped<GetAllUsersUseCase>();
            services.AddScoped<GetUserByIdUseCase>();
            services.AddScoped<GetUserByEmailUseCase>();
            services.AddScoped<DeleteUserUseCase>();
            services.AddScoped<LoginUserUseCase>();
            services.AddScoped<GeneralUserUseCaseHandler>();

            // Registrar casos de uso de Leagues
            services.AddScoped<GetAllLeagues>();
            services.AddScoped<GetLeagueById>();
            services.AddScoped<CreateLeague>();
            services.AddScoped<UpdateLeague>();
            services.AddScoped<DeleteLeague>();
            services.AddScoped<GeneralLeagueUseCaseHandler>();

            // Registrar casos de uso de Matches
            services.AddScoped<GetAllMatches>();
            services.AddScoped<GetMatchById>();
            services.AddScoped<CreateMatch>();
            services.AddScoped<UpdateMatch>();
            services.AddScoped<DeleteMatch>();
            services.AddScoped<GeneralMatchUseCaseHandler>();

            // Registrar casos de uso de Teams
            services.AddScoped<CreateTeam>();
            services.AddScoped<GetTeamById>();
            services.AddScoped<UpdateTeam>();
            services.AddScoped<DeleteTeam>();
            services.AddScoped<GetAllTeams>();
            services.AddScoped<GeneralTeamUseCaseHandler>();

            //Registrar casos de uso de Players
            services.AddScoped<CreatePlayer>();
            services.AddScoped<GetAllPlayer>();
            services.AddScoped<GetPlayerById>();
            services.AddScoped<UpdatePlayer>();
            services.AddScoped<DeletePlayer>();
            services.AddScoped<GeneralPlayerUseCaseHandler>();

            //Registrar casos de uso de Player Statistics
            services.AddScoped<CreatePlayerStatistic>();
            services.AddScoped<GetAllPlayerStatistics>();
            services.AddScoped<GetPlayerStatisticById>();
            services.AddScoped<UpdatePlayerStatistic>();
            services.AddScoped<DeletePlayerStatistic>();
            services.AddScoped<GeneralPlayerStatisticsUseCaseHandler>();

            // Configuración de Swagger
            services.AddEndpointsApiExplorer();
            services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()));
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScoreFlow API", Version = "v1" });
            //});

            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddAuthorization();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            //if (env.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScoreFlow API V1");
            //        c.RoutePrefix = string.Empty;
            //    });
            //}

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
