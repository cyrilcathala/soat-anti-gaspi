output "resource_group_name" {
  value = azurerm_resource_group.default.name
}

output "web_app_name" {
  value = azurerm_windows_web_app.default.name
}

output "postgresql_name" {
  value = azurerm_postgresql_flexible_server.default.name
}

output "postgresql_connection_string" {
  value = "Server=${azurerm_postgresql_flexible_server.default.name}.postgres.database.azure.com;Database=${azurerm_postgresql_flexible_server_database.default.name};Username=${azurerm_postgresql_flexible_server.default.administrator_login};Password=${azurerm_postgresql_flexible_server.default.administrator_password}"
  sensitive = true
}

output "postgresql_url" {
  value = "${azurerm_postgresql_flexible_server.default.name}.postgres.database.azure.com"
}

output "postgresql_database_name" {
  value = azurerm_postgresql_flexible_server_database.default.name
}

output "postgresql_user" {
  value = azurerm_postgresql_flexible_server.default.administrator_login
}

output "postgresql_password" {
  value = azurerm_postgresql_flexible_server.default.administrator_password
  sensitive = true
}

