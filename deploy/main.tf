variable "SendGridApiKey" {
  type = string
  sensitive = true
}

resource "azurerm_resource_group" "default" {
  name     = "rg-${var.name_suffix}"
  location = var.location
}

resource "azurerm_log_analytics_workspace" "default" {
  name                = "log-${var.name_suffix}"
  resource_group_name = azurerm_resource_group.default.name
  location            = azurerm_resource_group.default.location
}

resource "azurerm_application_insights" "default" {

  name                = "appi-${var.name_suffix}"
  location            = azurerm_resource_group.default.location
  resource_group_name = azurerm_resource_group.default.name
  workspace_id        = azurerm_log_analytics_workspace.default.id
  application_type    = "web"
}

resource "azurerm_service_plan" "default" {
  name                = "plan-${var.name_suffix}"
  location            = azurerm_resource_group.default.location
  resource_group_name = azurerm_resource_group.default.name
  os_type             = "Windows"
  sku_name            = "F1"
}

resource "azurerm_windows_web_app" "default" {
  name                = "app-${var.name_suffix}"
  location            = azurerm_resource_group.default.location
  resource_group_name = azurerm_resource_group.default.name
  service_plan_id     = azurerm_service_plan.default.id

  site_config {
    always_on        = false
    cors {
      allowed_origins = ["*"]
    }
    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v6.0"
    }
  }

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY"             = azurerm_application_insights.default.instrumentation_key
    "APPLICATIONINSIGHTS_CONNECTION_STRING"      = azurerm_application_insights.default.connection_string
    "ApplicationInsightsAgent_EXTENSION_VERSION" = "~3"
    "XDT_MicrosoftApplicationInsights_Mode"      = "recommended"
    "SendGridMailSender"                         = "cyril.cathala@soat.fr"
    "SendGridApiKey"                             = var.SendGridApiKey
    "FrontUrl"                                   = "https://app-soat-bc22-front-dev-fr.azurewebsites.net/"
  }

  connection_string {
    name  = "AntiGaspi"
    type  = "PostgreSQL"
    value = "Server=${azurerm_postgresql_flexible_server.default.name}.postgres.database.azure.com;Database=${azurerm_postgresql_flexible_server_database.default.name};Username=${azurerm_postgresql_flexible_server.default.administrator_login};Password=${azurerm_postgresql_flexible_server.default.administrator_password}"
  }
}

resource "azurerm_postgresql_flexible_server" "default" {
  name                   = "psql-${var.name_suffix}"
  resource_group_name    = azurerm_resource_group.default.name
  location               = azurerm_resource_group.default.location
  version                = "14"
  administrator_login    = "user"
  administrator_password = var.database_password
  zone                   = "1"
  storage_mb             = 32768
  sku_name               = "B_Standard_B1ms"
  backup_retention_days  = 7
}

resource "azurerm_postgresql_flexible_server_configuration" "example" {
  name      = "client_min_messages"
  server_id = azurerm_postgresql_flexible_server.default.id
  value     = "WARNING"
}

resource "azurerm_postgresql_flexible_server_database" "default" {
  name      = "antigaspi"
  server_id = azurerm_postgresql_flexible_server.default.id
  collation = "en_US.UTF8"
  charset   = "UTF8"
}

resource "azurerm_postgresql_flexible_server_firewall_rule" "default" {
  name                = "all"
  server_id           = azurerm_postgresql_flexible_server.default.id
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}