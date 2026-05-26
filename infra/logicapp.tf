# logicapp.tf
# Logic App (Consumption) for SharePoint → AI Search ingestion

# SharePoint Online managed API connection (requires manual OAuth authorization in portal)
resource "azurerm_api_connection" "sharepoint" {
  name                = "happyliving-sharepoint"
  resource_group_name = azurerm_resource_group.main.name
  managed_api_id      = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/sharepointonline"
  display_name        = "SharePoint Online — Happy Living"

  tags = azurerm_resource_group.main.tags
}

# Azure AI Search managed API connection (managed identity — no credentials needed)
resource "azurerm_api_connection" "search" {
  name                = "happyliving-search"
  resource_group_name = azurerm_resource_group.main.name
  managed_api_id      = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/azureaisearch"
  display_name        = "Azure AI Search — Happy Living"

  parameter_values = {
    authType         = "ManagedServiceIdentity"
    searchServiceUrl = "https://${azurerm_search_service.main.name}.search.windows.net"
  }

  tags = azurerm_resource_group.main.tags
}

# Logic App Consumption workflow
resource "azurerm_logic_app_workflow" "sharepoint_ingestion" {
  name                = "happyliving-sharepoint-ingestion"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name

  identity {
    type = "SystemAssigned"
  }

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

# Logic App identity → AI Search (write documents to index)
resource "azurerm_role_assignment" "logicapp_search" {
  scope                = azurerm_search_service.main.id
  role_definition_name = "Search Index Data Contributor"
  principal_id         = azurerm_logic_app_workflow.sharepoint_ingestion.identity[0].principal_id
}

data "azurerm_subscription" "current" {}
