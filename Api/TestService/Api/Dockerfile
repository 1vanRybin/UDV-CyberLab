FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5068

ENV ASPNETCORE_URLS=http://+:5068

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Api/TestService/Api/Api.csproj", "Api/TestService/Api/"]
COPY ["Api/TestService/Infrastructure/Infrastructure.csproj", "Api/TestService/Infrastructure/"]
COPY ["Api/TestService/Service/Service.csproj", "Api/TestService/Service/"]
COPY ["Api/TestService/Domain/Domain.csproj", "Domain/"]
COPY ["../../Libs/Core/Core.csproj", "../../Libs/Core/"]
RUN dotnet restore "Api/TestService/Api/Api.csproj"
COPY . .
WORKDIR "/src/Api/TestService/Api"
RUN dotnet build "Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
