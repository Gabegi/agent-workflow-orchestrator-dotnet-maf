# search.tf

resource "azurerm_search_service" "main" {
  name                = "srch-workflow-maf-dev"
  resource_group_name = azurerm_resource_group.main.name
  location            = "eastus"
  sku                 = "basic"

  tags = azurerm_resource_group.main.tags
}
