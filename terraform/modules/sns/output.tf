output "arn" {
    description = "SNS topic ARN"
    value       = aws_sns_topic.sns.arn 
}