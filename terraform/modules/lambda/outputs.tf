output "lambda_name" {
    description = "Name of the Lambda function"
    value       = aws_lambda_function.lambda_function.function_name
}

output "lambda_arn" {
    description = "Amazon Resource Name of the Lambda function"
    value       = aws_lambda_function.lambda_function.arn
}

output "lambda_invoke_arn" {
    description = "ARN used to invoke Lambda Function from API Gateway"
    value       = aws_lambda_function.lambda_function.invoke_arn 
}
