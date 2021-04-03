#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY rabbitmq-pub-api/rabbitmq-pub-api.csproj rabbitmq-pub-api/
RUN dotnet restore "rabbitmq-pub-api/rabbitmq-pub-api.csproj"
COPY . .
WORKDIR "/src/rabbitmq-pub-api"
RUN dotnet build "rabbitmq-pub-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rabbitmq-pub-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rabbitmq-pub-api.dll"]
