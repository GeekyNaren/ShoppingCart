# Multi-stage Dockerfile for building and running ASP.NET Core application on Render.com
# This Dockerfile is placed inside the API project folder (ShoppingCart/)
# Build ARG default set to 10.0 to match your project TFM (net10.0)
ARG DOTNET_VERSION=10.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy the project file and restore. When Dockerfile is inside the project folder,
# copy the project file directly (ShoppingCart.csproj) for better layer caching.
COPY ["ShoppingCart.csproj", "./"]
RUN dotnet restore "ShoppingCart.csproj"

# Copy everything else and publish (no apphost for smaller image). Use --no-restore
# because we've already restored above.
COPY . .
RUN dotnet publish "ShoppingCart.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Runtime image (smaller)
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish ./

# Do not enable HTTPS or certificates inside the container; Render handles TLS termination.
# Entrypoint uses /bin/sh -c so ${PORT} is expanded at container runtime, sets a sensible
# default (5000) when PORT is not provided, exports ASPNETCORE_URLS, and execs dotnet
ENTRYPOINT ["/bin/sh", "-c", "PORT=${PORT:-5000}; export ASPNETCORE_URLS=\"http://+:${PORT}\"; exec dotnet ShoppingCart.dll --urls \"http://+:${PORT}\""]
