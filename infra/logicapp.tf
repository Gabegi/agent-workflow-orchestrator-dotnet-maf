# logicapp.tf
# Logic App (Consumption) for SharePoint → AI Search ingestion

import {
  to = azapi_resource.sharepoint_connection
  id = "/subscriptions/${data.azurerm_subscription.current.subscription_id}/resourceGroups/rg-happyliving-dev/providers/Microsoft.Web/connections/happyliving-sharepoint"
}

# SharePoint Online API connection (OAuth — authorize manually in portal after deploy)
resource "azapi_resource" "sharepoint_connection" {
  type      = "Microsoft.Web/connections@2016-06-01"
  name      = "happyliving-sharepoint"
  location  = azurerm_resource_group.main.location
  parent_id = azurerm_resource_group.main.id

  body = {
    properties = {
      displayName = "SharePoint Online — Happy Living"
      api = {
        id = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/sharepointonline"
      }
    }
  }

  tags = azurerm_resource_group.main.tags
}

# Azure AI Search API connection (managed identity)
resource "azapi_resource" "search_connection" {
  type                    = "Microsoft.Web/connections@2016-06-01"
  name                    = "happyliving-search"
  location                = azurerm_resource_group.main.location
  parent_id               = azurerm_resource_group.main.id
  schema_validation_enabled = false

  body = {
    properties = {
      displayName = "Azure AI Search — Happy Living"
      api = {
        id = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/azureaisearch"
      }
    }
  }

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
        connectionId   = azapi_resource.sharepoint_connection.id
        connectionName = azapi_resource.sharepoint_connection.name
        id             = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/sharepointonline"
      }
      azureaisearch = {
        connectionId   = azapi_resource.search_connection.id
        connectionName = azapi_resource.search_connection.name
        id             = "${data.azurerm_subscription.current.id}/providers/Microsoft.Web/locations/${azurerm_resource_group.main.location}/managedApis/azureaisearch"
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
