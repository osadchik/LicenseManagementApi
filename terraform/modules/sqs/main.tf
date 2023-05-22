resource "aws_sqs_queue" "users_integration_queue" {
    name = var.sqs_name
}