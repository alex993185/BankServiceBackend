#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BankService.Backend.API/BankService.Backend.API.csproj", "BankService.Backend.API/"]
COPY ["BankService.Backend.Persistance/BankService.Backend.Persistance.csproj", "BankService.Backend.Persistance/"]
COPY ["BankService.Backend.BusinessLogic/BankService.Backend.BusinessLogic.csproj", "BankService.Backend.BusinessLogic/"]
RUN dotnet restore "BankService.Backend.API/BankService.Backend.API.csproj"
COPY . .
WORKDIR "/src/BankService.Backend.API"
RUN dotnet build "BankService.Backend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankService.Backend.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankService.Backend.API.dll"]