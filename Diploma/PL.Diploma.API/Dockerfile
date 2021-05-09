#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT="Production"
ENV HOSTING="Heroku"
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["PL.Diploma.API/PL.Diploma.API.csproj", "PL.Diploma.API/"]
COPY ["PL.Utils/PL.Utils.csproj", "PL.Utils/"]
COPY ["BL.DIExtension/BL.DIExtension.csproj", "BL.DIExtension/"]
COPY ["BL.Infrastructure/BL.Infrastructure.csproj", "BL.Infrastructure/"]
COPY ["BL.Interfaces/BL.Interfaces.csproj", "BL.Interfaces/"]
COPY ["BL.Models/BL.Models.csproj", "BL.Models/"]
COPY ["BL.Subdomains.Auth/BL.Subdomains.Auth.Services.csproj", "BL.Subdomains.Auth/"]
COPY ["BL.Subdomains.DataForFiles/BL.Subdomains.DataForFiles.csproj", "BL.Subdomains.DataForFiles/"]
COPY ["BL.Subdomains.FilesGeneration/BL.Subdomains.FilesGeneration.csproj", "BL.Subdomains.FilesGeneration/"]
COPY ["BL.Subdomains.FilesGeneration.FilesGenerationViaOpenXml/BL.Subdomains.FilesGeneration.FilesGenerationViaOpenXml.csproj", "BL.Subdomains.FilesGeneration.FilesGenerationViaOpenXml/"]
COPY ["BL.Utils/BL.Utils.csproj", "BL.Utils/"]
COPY ["DL.DIExtension/DL.DIExtension.csproj", "DL.DIExtension/"]
COPY ["DL.EF.Context/DL.EF.Context.csproj", "DL.EF.Context/"]
COPY ["DL.Entities/DL.Entities.csproj", "DL.Entities/"]
COPY ["DL.EF.EntitiyConfigurations/DL.EF.EntitiyConfigurations.csproj", "DL.EF.EntitiyConfigurations/"]
COPY ["DL.Infrastructure/DL.Infrastructure.csproj", "DL.Infrastructure/"]
COPY ["DL.Repositories/DL.Repositories.csproj", "DL.Repositories/"]
COPY ["DL.Interfaces/DL.Interfaces.csproj", "DL.Interfaces/"]
COPY ["DL.UnitOfWork/DL.UnitOfWork.csproj", "DL.UnitOfWork/"]
COPY ["DL.EF.Migrations/DL.EF.Migrations.csproj", "DL.EF.Migrations/"]
COPY ["Common.Configurations/Common.Configurations.csproj", "Common.Configurations/"]
COPY ["Common.Infrastructure/Common.Infrastructure.csproj", "Common.Infrastructure/"]
RUN dotnet restore "PL.Diploma.API/PL.Diploma.API.csproj"
COPY . .
WORKDIR "/src/PL.Diploma.API"
RUN dotnet build "PL.Diploma.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PL.Diploma.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PL.Diploma.API.dll"] # for local testing
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet PL.Diploma.API.dll # for deployment to heroku