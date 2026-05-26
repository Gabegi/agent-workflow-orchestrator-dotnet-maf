# outputs.tf

output "logic_app_trigger_url" {
  value     = azurerm_logic_app_workflow.sharepoint_ingestion.access_endpoint
  sensitive = true
}
