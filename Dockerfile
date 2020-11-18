FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

FROM build-env AS build
COPY BankService.Backend.API/*.csproj ./BankService.Backend.API/
COPY BankService.Backend.BusinessLogic/*.csproj ./BankService.Backend.BusinessLogic/
COPY BankService.Backend.Persistance/*.csproj ./BankService.Backend.Persistance/
COPY BankService.Backend.Tests/*.csproj ./BankService.Backend.Tests/
COPY BankService.Backend.sln ./
RUN dotnet restore ./BankService.Backend.sln

COPY . ./
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BankService.Backend.API.dll"]