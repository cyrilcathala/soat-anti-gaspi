# SOAT Anti Gaspi

## Installation

Lancer Docker Compose et la migration de base de données Entity Framework Core.

```
cd src
docker compose up -d
dotnet ef database update --project .\Soat.AntiGaspi.Api
```