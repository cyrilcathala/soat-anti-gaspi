# SOAT Anti Gaspi

## Installation

Lancer Docker Compose et la migration de base de données Entity Framework Core.

```ps
cd src
# Lancer Docker
docker compose up -d
# Lancer la création de base de données
dotnet ef database update --project Soat.AntiGaspi.Api
# Lancer l'API
dotnet run --project Soat.AntiGaspi.Api/Soat.AntiGaspi.Api.csproj
```

## Exemples
Le fichier `api.rest` contient tous les appels d'API accompagné d'exemples.

Pour voir l'intégralité de la documentation OpenAPI/Swagger:
https://localhost:7201/swagger/index.html

