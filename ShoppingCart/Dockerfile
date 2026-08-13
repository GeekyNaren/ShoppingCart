# Multi-stage Dockerfile for building and running ASP.NET Core application on Render.com
# Default DOTNET_VERSION arg set to 8.0; change to 10.0 if your project targets net10.0 and an image is available
ARG DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy csproj and restore (adjust the path if your .csproj is in a different folder)
COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
RUN dotnet restore "ShoppingCart/ShoppingCart.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR /src/ShoppingCart

# Publish the app (no apphost to keep image portable)
RUN dotnet publish "ShoppingCart.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime image (smaller)
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime
WORKDIR /app

# Bind to the PORT injected by Render.com
# Render injects PORT at runtime; ASP.NET Core will use this environment variable
ENV ASPNETCORE_URLS="http://+:$PORT"

# Copy published output from build stage
COPY --from=build /app/publish ./

# Do not enable HTTPS or certificates in container; Render handles TLS termination
# Entrypoint: replace ShoppingCart.dll with your project's main DLL if different
ENTRYPOINT ["dotnet", "ShoppingCart.dll"]
