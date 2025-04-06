FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5108

ENV ASPNETCORE_URLS=http://+:5108

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Api/ProjectService/Api/Api.csproj", "Api/ProjectService/Api/"]
COPY ["Api/ProjectService/Infrastructure/Infrastructure.csproj", "Api/ProjectService/Infrastructure/"]
COPY ["Api/ProjectService/Service/Service.csproj", "Api/ProjectService/Service/"]
COPY ["Api/ProjectService/Domain/Domain.csproj", "Domain/"]
COPY ["../../Libs/Core/Core.csproj", "../../Libs/Core/"]
RUN dotnet restore "Api/ProjectService/Api/Api.csproj"
COPY . .
WORKDIR "/src/Api/ProjectService/Api"
RUN dotnet build "Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
