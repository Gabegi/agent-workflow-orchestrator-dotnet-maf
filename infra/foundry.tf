# foundry.tf
# Azure AI Foundry Hub and Project

data "azurerm_client_config" "current" {}

resource "azurerm_ai_foundry" "hub" {
  name                = "aih-workflow-maf-dev"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  storage_account_id  = azurerm_storage_account.foundry.id
  key_vault_id        = azurerm_key_vault.foundry.id

  identity {
    type = "SystemAssigned"
  }

  tags = azurerm_resource_group.main.tags
}

resource "azurerm_ai_foundry_project" "main" {
  name               = "aip-workflow-maf-dev"
  location           = azurerm_resource_group.main.location
  ai_services_hub_id = azurerm_ai_foundry.hub.id

  identity {
    type = "SystemAssigned"
  }

  tags = azurerm_resource_group.main.tags
}
