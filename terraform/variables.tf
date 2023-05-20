# Provider
variable "aws_region" {
  type = string
}

# Lambda
variable "users-management-lambda-name" {
  type = string
}
variable "lambda_description" {
  type = string
}
variable "filename" {
  type = string
}
variable "runtime" {
  type = string
  default = "dotnet6"
}
variable "handler" {
  type = string
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