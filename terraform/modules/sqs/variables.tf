variable "sqs_name" {
    description = "SQS name"
    type        = string 
}

variable "visibility_timeout_seconds" {
    description = "The visibility timeout for the queue. Default is 30."
    type        = number
    default     = 30
}

variable "redrive_policy" {
    description = "JSON policy to set up the Dead Letter Queue"
    type        = string 
    default     = null
}