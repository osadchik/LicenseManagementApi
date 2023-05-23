resource "aws_sqs_queue" "sqs" {
    name                       = var.sqs_name
    visibility_timeout_seconds = var.visibility_timeout_seconds
    redrive_policy             = var.redrive_policy
}