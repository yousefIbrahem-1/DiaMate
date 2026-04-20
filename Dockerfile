FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore
COPY ["DiaMate/DiaMate.csproj", "DiaMate/"]
RUN dotnet restore "DiaMate/DiaMate.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DiaMate"
RUN dotnet build "DiaMate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiaMate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiaMate.dll"]