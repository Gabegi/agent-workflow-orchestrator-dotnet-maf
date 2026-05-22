# keyvault.tf

resource "azurerm_key_vault" "foundry" {
  name                       = "kv-foundry-maf-dev"
  resource_group_name        = azurerm_resource_group.main.name
  location                   = azurerm_resource_group.main.location
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
  enable_rbac_authorization  = true

  tags = azurerm_resource_group.main.tags
}

# Foundry hub can read/write secrets
resource "azurerm_role_assignment" "foundry_hub_kv" {
  scope                = azurerm_key_vault.foundry.id
  role_definition_name = "Key Vault Secrets Officer"
  principal_id         = azurerm_ai_foundry.hub.identity[0].principal_id
}

# Your own identity (deployer) gets full admin access
resource "azurerm_role_assignment" "admin_kv" {
  scope                = azurerm_key_vault.foundry.id
  role_definition_name = "Key Vault Administrator"
  principal_id         = var.admin_object_id
}
