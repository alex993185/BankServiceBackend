FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

FROM build-env AS build
COPY BankServiceBackend/*.csproj ./BankServiceBackend/
COPY BankServiceBackend.BusinessLogic/*.csproj ./BankServiceBackend.BusinessLogic/
COPY BankServiceBackend.Persistance/*.csproj ./BankServiceBackend.Persistance/
COPY BankServiceBackend.Tests/*.csproj ./BankServiceBackend.Tests/
COPY BankServiceBackend.sln ./
RUN dotnet restore ./BankServiceBackend.sln

COPY . ./
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS run
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "BankServiceBackend.dll"]