variable "name_suffix" {
  default     = "soat-bc22-dev-fr"
  description = "Suffix of the resource name."
}

variable "location" {
  default     = "francecentral"
  description = "Location of the resource."
}

variable "database_password" {
  default     = "P@ssw0rd"
  description = "Password for the PostgreSQL database"
  sensitive   = true
}