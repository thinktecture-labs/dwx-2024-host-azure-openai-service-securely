variable "location" {
  type    = string
  default = "swedencentral"
}

variable "suffix" {
  type = string
}

variable "resource_group_name" {
  type = string
}

variable "acr_name" {
  type = string
}

variable "image_tag" {
  type = string
}

variable "ai_model_name" {
  type = string
  default = "gpt-4o"
}

variable "ai_model_version" {
  type    = string
  default = "2024-05-13"
}
