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

# Azure AI Search managed API connection
# Auth configured manually in portal after deploy (managed identity)
resource "azurerm_api_connection" "search" {
  name                = "happyliving-search"
  resource_group_name = azurerm_resource_group.main.name
  managed_api_id      = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/azureaisearch"
  display_name        = "Azure AI Search — Happy Living"

  tags = azurerm_resource_group.main.tags
}

# Logic App Consumption workflow — daily recurrence at 2am CET
resource "azurerm_logic_app_workflow" "sharepoint_ingestion" {
  name                = "happyliving-sharepoint-ingestion"
  location            = azurerm_resource_group.main.location
  resource_group_name = azurerm_resource_group.main.name
  workflow_schema     = "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"
  workflow_version    = "1.0.0.0"

  identity {
    type = "SystemAssigned"
  }

  workflow_parameters = {
    "$definition" = jsonencode({
      "$schema" = "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#"
      "triggers" = {
        "recurrence" = {
          "type" = "Recurrence"
          "recurrence" = {
            "frequency" = "Day"
            "interval"  = 1
            "schedule" = {
              "hours"   = ["2"]
              "minutes" = ["0"]
            }
            "timeZone" = "W. Europe Standard Time"
          }
        }
        "manual" = {
          "type" = "Request"
          "kind" = "Http"
          "inputs" = {
            "schema" = {}
          }
        }
      }
      "actions" = {
        "list_files" = {
          "type" = "ApiConnection"
          "inputs" = {
            "host" = {
              "connection" = {
                "name" = "@parameters('$connections')['sharepoint']['connectionId']"
              }
            }
            "method" = "get"
            "path"   = "/datasets/@{encodeURIComponent('https://yourtenant.sharepoint.com/sites/happyliving')}/tables/@{encodeURIComponent('Shared Documents')}/items"
          }
          "runAfter" = {}
        }
        "for_each_file" = {
          "type"    = "Foreach"
          "foreach" = "@body('list_files')?['value']"
          "runAfter" = {
            "list_files" = ["Succeeded"]
          }
          "actions" = {
            "get_file_content" = {
              "type" = "ApiConnection"
              "inputs" = {
                "host" = {
                  "connection" = {
                    "name" = "@parameters('$connections')['sharepoint']['connectionId']"
                  }
                }
                "method" = "get"
                "path"   = "/datasets/@{encodeURIComponent('https://yourtenant.sharepoint.com/sites/happyliving')}/files/@{encodeURIComponent(items('for_each_file')?['ID'])}/content"
              }
              "runAfter" = {}
            }
            "push_to_search" = {
              "type" = "ApiConnection"
              "inputs" = {
                "host" = {
                  "connection" = {
                    "name" = "@parameters('$connections')['azureaisearch']['connectionId']"
                  }
                }
                "method" = "post"
                "path"   = "/indexes/happyliving-sharepoint-index/docs/index"
                "body" = {
                  "value" = [{
                    "@search.action" = "mergeOrUpload"
                    "id"          = "@{items('for_each_file')?['ID']}"
                    "content"     = "@{body('get_file_content')}"
                    "source_file" = "@{items('for_each_file')?['Path']}"
                    "title"       = "@{items('for_each_file')?['Name']}"
                  }]
                }
              }
              "runAfter" = {
                "get_file_content" = ["Succeeded"]
              }
            }
          }
        }
      }
    })
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
