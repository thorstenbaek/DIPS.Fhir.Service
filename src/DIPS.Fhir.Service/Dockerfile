# Build stage
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set timezone
ENV TZ=Europe/Oslo

# COPY ./Certificates/*.pem /etc/ssl/certs/
# COPY ./Certificates/*.crt /etc/ssl/certs/
# RUN update-ca-certificates
# COPY ./Certificates/*.p12 /etc/ssl/certs/

# https://stackoverflow.com/questions/48669548/why-does-aspnet-core-start-on-port-80-from-within-docker
ENV ASPNETCORE_URLS=http://+:80
ENV IN_CONTAINER=1

# Health check using health status endpoint
HEALTHCHECK --interval=30s --timeout=10s --retries=3 --start-period=10s CMD curl --silent --fail localhost:80/status/health || exit 1

COPY --from=build-env /src/out .

ENTRYPOINT ["dotnet", "DIPS.Fhir.Service.dll"]