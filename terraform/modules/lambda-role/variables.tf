variable "iam_role_name" {
    description = "IAM role name"
    type        = string
}

variable "tags" {
    description = "Addition tags for resources"
    type = map
    default = {}
}