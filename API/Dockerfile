# Fase de construcción: Usamos el SDK de .NET para restaurar y compilar la solución completa
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instalar los workloads necesarios para MAUI
RUN dotnet workload install maui-android

# Establecer el directorio de trabajo
WORKDIR /src

# Copiar el archivo de solución (ajustado para que apunte al directorio correcto)
COPY ./ScoreFlow2.sln ./  # La solución está en el directorio raíz

# Copiar los archivos .csproj de cada proyecto que forma parte de la solución
COPY ./API/API.csproj ./API/
COPY ./Application/Application.csproj ./Application/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restaurar las dependencias para toda la solución
RUN dotnet restore ./ScoreFlow2.sln

# Copiar todo el contenido de la solución
COPY ./ ./  # Copiar el resto de la aplicación

# Compilar el proyecto API (que será el entry point de la aplicación)
WORKDIR /src/API
RUN dotnet build -c Release -o /app/build

# Publicar la aplicación (para obtener la versión optimizada)
FROM build AS publish
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# Imagen final: Usamos una imagen base más ligera para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto 80 para la API
EXPOSE 8080

# Definir el comando de entrada (asumiendo que la aplicación API genera API.dll)
ENTRYPOINT ["dotnet", "API.dll"]
