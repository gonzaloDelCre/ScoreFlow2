# Fase de construcci�n: Usamos el SDK de .NET para restaurar y compilar la soluci�n completa
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Instalar los workloads necesarios para MAUI
RUN dotnet workload install maui-android

# Establecer el directorio de trabajo
WORKDIR /src

# Copiar el archivo de solución (ajustado para que apunte al directorio correcto)
COPY ./ScoreFlow2.sln ./  # La solución está en el directorio raíz

# Copiar los archivos .csproj de cada proyecto que forma parte de la soluci�n
COPY ./API/API.csproj ./API/
COPY ./Application/Application.csproj ./Application/
COPY ./Domain/Domain.csproj ./Domain/
COPY ./Infrastructure/Infrastructure.csproj ./Infrastructure/

# Restaurar las dependencias para toda la soluci�n
RUN dotnet restore ./ScoreFlow2.sln

# Copiar todo el contenido de la soluci�n
COPY ./ ./  # Copiar el resto de la aplicaci�n

# Compilar el proyecto API (que ser� el entry point de la aplicaci�n)
WORKDIR /src/API
RUN dotnet build -c Release -o /app/build

# Publicar la aplicaci�n (para obtener la versi�n optimizada)
FROM build AS publish
WORKDIR /src/API
RUN dotnet publish -c Release -o /app/publish

# Imagen final: Usamos una imagen base m�s ligera para ejecutar la aplicaci�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Exponer el puerto 80 para la API
EXPOSE 8080

# Definir el comando de entrada (asumiendo que la aplicaci�n API genera API.dll)
ENTRYPOINT ["dotnet", "API.dll"]