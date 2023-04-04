FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GitlabMonitor.csproj", "./"]
RUN dotnet restore "GitlabMonitor.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "GitlabMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GitlabMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GitlabMonitor.dll"]
