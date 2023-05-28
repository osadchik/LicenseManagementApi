output "invoke_url" {
    description = "URL to invoke API pointing to the stage"
    value       = aws_api_gateway_deployment.api_deploy.invoke_url
}