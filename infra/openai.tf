# openai.tf
# Azure OpenAI + model deployments

resource "azurerm_cognitive_account" "openai" {
  name                  = "oai-happyliving-dev"
  location              = azurerm_resource_group.main.location
  resource_group_name   = azurerm_resource_group.main.name
  kind                  = "OpenAI"
  sku_name              = "S0"
  custom_subdomain_name = "oai-happyliving-dev"

  identity {
    type = "SystemAssigned"
  }

  tags = azurerm_resource_group.main.tags
}

resource "azurerm_cognitive_deployment" "gpt41" {
  name                 = "gpt-41"
  cognitive_account_id = azurerm_cognitive_account.openai.id

  model {
    format  = "OpenAI"
    name    = "gpt-4.1"
    version = "2025-04-14"
  }

  sku {
    name     = "Standard"
    capacity = 10
  }
}

resource "azurerm_cognitive_deployment" "embedding" {
  name                 = "text-embedding-3-large"
  cognitive_account_id = azurerm_cognitive_account.openai.id

  model {
    format  = "OpenAI"
    name    = "text-embedding-3-large"
    version = "1"
  }

  sku {
    name     = "Standard"
    capacity = 50
  }
}

# AI Search can call OpenAI for vectorisation (no API keys)
resource "azurerm_role_assignment" "search_openai" {
  scope                = azurerm_cognitive_account.openai.id
  role_definition_name = "Cognitive Services OpenAI User"
  principal_id         = azurerm_search_service.main.identity[0].principal_id
}
