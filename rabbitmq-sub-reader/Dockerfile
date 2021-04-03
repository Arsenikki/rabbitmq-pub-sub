#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY rabbitmq-sub-reader/rabbitmq-sub-reader.csproj rabbitmq-sub-reader/
RUN dotnet restore "rabbitmq-sub-reader/rabbitmq-sub-reader.csproj"
COPY . .
WORKDIR "/src/rabbitmq-sub-reader"
RUN dotnet build "rabbitmq-sub-reader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "rabbitmq-sub-reader.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "rabbitmq-sub-reader.dll"]
