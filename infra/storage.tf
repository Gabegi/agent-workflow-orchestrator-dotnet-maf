# storage.tf

resource "azurerm_storage_account" "foundry" {
  name                     = "sahappylivingdev"
  resource_group_name      = azurerm_resource_group.main.name
  location                 = azurerm_resource_group.main.location
  account_tier             = "Standard"
  account_replication_type = "LRS"

  timeouts {
    create = "30m"
  }

  tags = azurerm_resource_group.main.tags
}
