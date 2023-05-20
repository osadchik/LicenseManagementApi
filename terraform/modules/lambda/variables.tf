variable "prefix" {
  description   = "Prefix for project name"
  type          = string
}

variable "name" {
    description   = "Prefix for the name of the lambda"
    type          = string
}

variable "description" {
    description   = "Description for the lambda (displayed in AWS console)"
    type          = string
}

variable "filename" {
    description   = "The path for the function's deployment package within the local filesystem"
    type          = string
    default       = null
}

variable "handler" {
    description   = "Name of the handler function in the code"
    type          = string
}

variable "runtime" {
    description   = "Name of the runtime. Defaults to 'dotnetcore2.1'"
    type          = string
    default       = "dotnetcore2.1"
}

variable "memory_size" {
    description   = "Memory allocation for the Lambda in Megabytes"
    type          = number
    default       = 256
}

variable "timeout" {
    description   = "Timeout period in seconds"
    type          = number 
}

variable "role_arn" {
    description   = "ARN of the role under which the Lambda should execute"
    type          = string
}

variable "publish" {
    description   = "Whether to publish creation/change as new Lambda Function Version"
    type          = bool
    default       = false
}

variable "environment_variables" {
    description = "Environment variables for the lambda. Use to supply configuration information"
    type        = map
    default     = {} 
}

variable "lambda_alias_current" {
    description = "Name of the alias being created."
    type        = string
}