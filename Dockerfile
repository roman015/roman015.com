﻿# https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_USERNAME=test
ARG BUILD_TOKEN=test
WORKDIR /source

# Copy source and build App
COPY . ./Roman015API/
WORKDIR /source/Roman015API
RUN dotnet restore
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Roman015API.dll"]
