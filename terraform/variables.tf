# Provider
variable "aws_region" {
  type = string
}

# Shared
variable "prefix" {
  type = string
}

# User Management Lambda
variable "user_management_lambda_name" {
  type = string
}
variable "user_management_lambda_description" {
  type = string
}
variable "user_management_lambda_handler" {
  type = string
}
variable "user_management_lambda_filename" {
  type = string
}

#User Integration Lambda
variable "user_integration_lambda_name" {
  type = string
}
variable "user_integration_lambda_description" {
  type = string
}
variable "user_integration_lambda_handler" {
  type = string
}
variable "user_integration_lambda_filename" {
  type = string
}

# Lambda Common
variable "runtime" {
  type = string
  default = "dotnet6"
}
variable "memory_size" {
  type = string
  default = "512"
}
variable "lambda_timeout" {
  type = string
  default = "15"
}
variable "publish" {
  type = bool
  default = true
}
variable "lambda_alias_current" {
  type = string
  default = "current_version"
}

# DynamoDB
 variable "billing_mode" {
  type = string   
 }
 variable "read_capacity" {
  type = string
 }
 variable "write_capacity" {
  type = string
 }
 variable "hash_key" {
  type = string
 }
 variable "attributes" {
  type = any
 }