# variables.tf

variable "admin_object_id" {
  description = "Object ID of the admin identity (deployer) to grant Key Vault access"
  type        = string
}
