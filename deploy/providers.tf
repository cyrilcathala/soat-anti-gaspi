terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
  }
  backend "azurerm" {
      resource_group_name  = "rg-soat-bc22-dev-fr"
      storage_account_name = "stsoatbc22devfr"
      container_name       = "terraform"
      key                  = "terraform.tfstate"
    }
}

provider "azurerm" {
  features {}
}