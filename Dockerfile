# Fase de construcción: Usamos el SDK de .NET para restaurar y compilar la solución completa
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instalar los workloads necesarios para MAUI
RUN dotnet workload install maui-android

# Establecer el directorio de trabajo
WORKDIR /src

# Copiar el archivo de solución (ScoreFlow2.sln)
COPY ScoreFlow2.sln ./

# Copiar los archivos .csproj de cada proyecto que forma parte de la solución
COPY API/API.csproj ./API/
COPY AppMovil/AppMovil.csproj ./AppMovil/
COPY Application/Application.csproj ./Application/
COPY Domain/Domain.csproj ./Domain/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restaurar las dependencias para toda la solución
RUN dotnet restore ScoreFlow2.sln

# Copiar todo el contenido de la solución
COPY . ./

# Compilar la solución
RUN dotnet publish API/API.csproj -c Release -o /app/publish

# Fase de ejecución: Usamos la imagen base para la ejecución de la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Copiar los archivos compilados desde la fase de construcción
COPY --from=build /app/publish .

# Exponer el puerto en el que estará corriendo la API
EXPOSE 8080

# Establecer el punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "API.dll"]
