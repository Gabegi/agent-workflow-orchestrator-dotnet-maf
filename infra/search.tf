# search.tf
# Azure AI Search — shared across all agents

resource "azurerm_search_service" "main" {
  name                = "srch-happyliving-dev"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "standard"

  identity {
    type = "SystemAssigned"
  }

  timeouts {
    create = "30m"
  }

  tags = azurerm_resource_group.main.tags
}
