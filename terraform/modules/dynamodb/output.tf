output "arn" {
    description = "Dynamo DB table ARN"
    value       = aws_dynamodb_table.dynamoDB.arn 
}