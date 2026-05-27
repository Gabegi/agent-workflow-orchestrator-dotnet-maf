# variables.tf

variable "admin_object_id" {
  description = "Object ID of the admin identity granted Key Vault access"
  type        = string
  sensitive   = true
}
