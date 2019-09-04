FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY PaymentGateway/PaymentGateway.csproj PaymentGateway/
RUN dotnet restore PaymentGateway/PaymentGateway.csproj
COPY . .
WORKDIR /src/PaymentGateway
RUN dotnet build PaymentGateway.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish PaymentGateway.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.dll"]
