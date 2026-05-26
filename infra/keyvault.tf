# keyvault.tf

resource "azurerm_key_vault" "foundry" {
  name                = "kv-happyliving-dev"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  tenant_id           = data.azurerm_client_config.current.tenant_id
  sku_name            = "standard"

  tags = azurerm_resource_group.main.tags
}
