FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /src

# Copiar el archivo de solución y los proyectos necesarios
COPY ./ScoreFlow2.sln ./ 
COPY ./API3/API3.csproj ./API3/
COPY ./Application/Application.csproj ./Application/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restaurar paquetes
RUN dotnet restore "ScoreFlow2.sln"

# Copiar todo el código fuente
COPY ./ ./ 

# Compilar el proyecto
WORKDIR /src/API3
RUN dotnet build "API3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API3.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------------------------
# FASE 3: Final (runtime)
# ---------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish ./ 

# Establecer la variable de entorno ASPNETCORE_URLS para permitir conexiones externas
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

# Exponer el puerto 5000 para que sea accesible desde fuera del contenedor
EXPOSE 5000

# Comando de inicio
ENTRYPOINT ["dotnet", "API3.dll"]
