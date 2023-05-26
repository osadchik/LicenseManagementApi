variable "name" {
    description = "Name of the API Gateway"
    type        = string 
}

variable "lambda_names" {
    description = "Lambda name for integration with API Gateway"
    type        = set(string) 
}

variable "users_uri" {
    description = "The users management lambda input URI"
    type        = string
}

variable "products_uri" {
    description = "The products management lambda input URI"
    type        = string
}

variable "license_uri" {
    description = "The license management lambda input URI"
    type        = string
}