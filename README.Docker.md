# Docker Setup

This repository contains two ASP.NET Core applications:

- `eCommerce.API` (Web API)
- `eCommerce.UI` (ASP.NET Core MVC)

Both projects target `.NET 10` and are containerized with multi-stage Docker builds.

## Files

- `eCommerce.API/Dockerfile`
- `eCommerce.UI/Dockerfile`
- `.dockerignore`
- `docker-compose.yml`
- `.env.example`

## Quick Start

1. Copy `.env.example` to `.env`.
2. Fill in required values such as `API_CONNECTION_STRING` and `JWT_SECRET`.
3. Build and run with Docker Compose.

```bash
docker compose build
docker compose up -d
```

## URLs (default)

- UI: `http://localhost:5204`
- API: `http://localhost:5285`

## Service-to-service URL

Inside the Compose network, the UI should call the API at:

- `http://ecommerce-api:8080`

This is exposed in the UI container as `Api__BaseUrl`.

## Stop

```bash
docker compose down
```

