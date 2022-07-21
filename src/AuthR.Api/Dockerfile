#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/AuthR.Api/AuthR.Api.csproj", "src/AuthR.Api/"]
COPY ["src/AuthR.BusinessLogic/AuthR.BusinessLogic.csproj", "src/AuthR.BusinessLogic/"]
COPY ["src/AuthR.DataAccess.Entities/AuthR.DataAccess.Entities.csproj", "src/AuthR.DataAccess.Entities/"]
COPY ["src/AuthR.DataAccess/AuthR.DataAccess.csproj", "src/AuthR.DataAccess/"]
COPY ["src/AuthR.DataAccess.Abstractions/AuthR.DataAccess.Abstractions.csproj", "src/AuthR.DataAccess.Abstractions/"]
COPY ["src/AuthR.BusinessLogic.Abstractions/AuthR.BusinessLogic.Abstractions.csproj", "src/AuthR.BusinessLogic.Abstractions/"]
COPY ["src/AuthR.BusinessLogic.Models/AuthR.BusinessLogic.Models.csproj", "src/AuthR.BusinessLogic.Models/"]
RUN dotnet restore "src/AuthR.Api/AuthR.Api.csproj"
COPY . .
WORKDIR "/src/src/AuthR.Api"
RUN dotnet build "AuthR.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthR.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthR.Api.dll"]