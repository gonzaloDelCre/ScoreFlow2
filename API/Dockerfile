# Fase de construcción: Usamos el SDK de .NET para restaurar y compilar la solución
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet workload install maui-android

WORKDIR /src

# Copiar el archivo de solución (asumiendo que se encuentra en la raíz)
COPY ScoreFlow2.sln ./   

COPY ../ScoreFlow2.sln ./
COPY ../API/API.csproj ./API/
COPY ../Application/Application.csproj ./Application/
COPY ../Domain/Domain.csproj ./Domain/
COPY ../Infrastructure/Infrastructure.csproj ./Infrastructure/


# Restaurar las dependencias
RUN dotnet restore ./ScoreFlow2.sln

# Copiar el resto de los archivos de la solución
COPY ./ ./   

# Compilar el proyecto API
WORKDIR /src/API
RUN dotnet build -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "API.dll"]
