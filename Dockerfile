# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy fsproj and restore as distinct layers
COPY src/xBh/*.fsproj xBh/
COPY src/xBh.Core/*.fsproj xBh.Core/
RUN dotnet restore xBh/xBh.fsproj

# Copy and build app and libraries
COPY src/xBh/ xBh/
COPY src/xBh.Core/ xBh.Core/

FROM build AS publish
WORKDIR /src/xBh
RUN dotnet publish --no-restore -o /app

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "xBh.dll"]
EXPOSE 5000/tcp
