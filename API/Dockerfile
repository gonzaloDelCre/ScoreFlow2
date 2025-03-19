# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solución y archivos del proyecto
COPY ../ScoreFlow2.sln ./
COPY ../Application/Application.csproj ./Application/
COPY ../Domain/Domain.csproj ./Domain/
COPY ../Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY ./API/API.csproj ./API/


# Restaurar dependencias
RUN dotnet restore "./API/API.csproj"

# Copiar el resto del código
COPY . .

# Construir el proyecto
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

# Publicar el proyecto
FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Etapa final: ejecuta la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

COPY --from=publish /app/publish .

# Configurar las variables de entorno para la base de datos
ENV ConnectionStrings__DefaultConnection="Server=db-scoreflow1.c7cmuw2a0yjr.eu-west-3.rds.amazonaws.com,1433;Database=ScoreFlow;User Id=admin;Password=Triski2015;TrustServerCertificate=True;MultipleActiveResultSets=True;"

ENTRYPOINT ["dotnet", "API.dll"]
