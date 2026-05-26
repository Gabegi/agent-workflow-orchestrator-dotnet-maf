# providers.tf
# Terraform and provider version constraints

terraform {
  required_version = ">= 1.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }

  backend "azurerm" {
    storage_account_name = "therealbestnamesa"
    container_name       = "maf-agent-workflow"
    key                  = "terraform.tfstate"
    use_azuread_auth     = true
  }
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
}

resource "azurerm_resource_group" "main" {
  name     = "rg-happyliving-dev"
  location = "eastus2"

  tags = {
    project     = "happyliving"
    environment = "dev"
  }
}
