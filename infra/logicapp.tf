# logicapp.tf
# Logic App (Consumption) for SharePoint → AI Search ingestion

# SharePoint Online managed API connection
resource "azurerm_api_connection" "sharepoint" {
  name                = "happyliving-sharepoint"
  resource_group_name = azurerm_resource_group.main.name
  managed_api_id      = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/sharepointonline"

  display_name = "SharePoint Online — Happy Living"

  tags = azurerm_resource_group.main.tags
}

# Azure AI Search managed API connection
resource "azurerm_api_connection" "search" {
  name                = "happyliving-search"
  resource_group_name = azurerm_resource_group.main.name
  managed_api_id      = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/azureaisearch"

  display_name = "Azure AI Search — Happy Living"

  tags = azurerm_resource_group.main.tags
}

# Logic App Consumption workflow (shell — definition completed via Foundry wizard)
resource "azurerm_logic_app_workflow" "sharepoint_ingestion" {
  name                = "happyliving-sharepoint-ingestion"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name

  parameters = {
    "$connections" = jsonencode({
      sharepoint = {
        connectionId   = azurerm_api_connection.sharepoint.id
        connectionName = azurerm_api_connection.sharepoint.name
        id             = azurerm_api_connection.sharepoint.managed_api_id
      }
      azureaisearch = {
        connectionId   = azurerm_api_connection.search.id
        connectionName = azurerm_api_connection.search.name
        id             = azurerm_api_connection.search.managed_api_id
      }
    })
  }

  tags = azurerm_resource_group.main.tags
}

data "azurerm_subscription" "current" {}
