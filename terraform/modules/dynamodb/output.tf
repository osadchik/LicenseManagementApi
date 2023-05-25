output "arn" {
    description = "Dynamo DB table ARN"
    value       = aws_dynamodb_table.dynamoDB.arn 
}

output "name" {
    description = "Dynamo DB table name"
    value       = aws_dynamodb_table.dynamoDB.id 
}