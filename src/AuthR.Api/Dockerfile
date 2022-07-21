FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AuthR.Api/AuthR.Api.csproj", "AuthR.Api/"]
RUN dotnet restore "src/AuthR.Api/AuthR.Api.csproj"
COPY . .
WORKDIR "/src/AuthR.Api"
RUN dotnet build "AuthR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthR.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthR.Api.dll"]
