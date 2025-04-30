FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

USER app
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Api/IdentityService/Api/Api.csproj", "Api/IdentityService/Api/"]
COPY ["Api/IdentityService/Infrastucture/Infrastucture.csproj", "Api/IdentityService/Infrastucture/"]
COPY ["Api/IdentityService/Services/Services.csproj", "Api/IdentityService/Services/"]
COPY ["Api/IdentityService/Domain/Domain.csproj", "Domain/"]
COPY ["./Libs/Core/Core.csproj", "../../Libs/Core/"]
RUN dotnet restore "Api/IdentityService/Api/Api.csproj"
COPY . .
WORKDIR "/src/Api/IdentityService/Api"
RUN dotnet build "Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
