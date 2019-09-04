FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BankMockAPI/BankMockAPI.csproj", "BankMockAPI/"]
RUN dotnet restore "BankMockAPI/BankMockAPI.csproj"
COPY . .
WORKDIR "/src/BankMockAPI"
RUN dotnet build "BankMockAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BankMockAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BankMockAPI.dll"]