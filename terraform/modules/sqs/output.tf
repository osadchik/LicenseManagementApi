output "arn" {
    description = "SQS Amazon Resource Number"
    value       = aws_sqs_queue.sqs.arn 
}

output "url" {
    description = "The URL for the created Amazon SQS queue"
    value       = aws_sqs_queue.sqs.id 
}