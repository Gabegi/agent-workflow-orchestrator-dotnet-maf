# search.tf

resource "azurerm_search_service" "main" {
  name                = "srch-workflow-maf-dev"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "basic"

  tags = azurerm_resource_group.main.tags
}
