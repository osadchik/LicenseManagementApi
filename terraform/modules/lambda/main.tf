resource "aws_lambda_function" "lambda_function"{
    function_name     = "${var.prefix}-${var.name}"
    description       = var.description
    filename          = var.filename
    handler           = var.handler
    runtime           = var.runtime
    memory_size       = var.memory_size
    timeout           = var.timeout
    role              = var.role_arn
    publish           = var.publish

    tracing_config {
        mode          = "Active"
    }

    dynamic "environment" {
        for_each = length(var.environment_variables) > 0 ? [var.environment_variables] : []
        content {
          variables = environment.value
        }
    }

    depends_on = [ aws_cloudwatch_log_group.log_group ]
}

resource "aws_lambda_alias" "lambda_alias_current" {
  name                = var.lambda_alias_current
  function_name       = aws_lambda_function.lambda_function.arn
  function_version    = aws_lambda_function.lambda_function.version 
}

resource "aws_cloudwatch_log_group" "log_group" {
    name              = "/aws/lambda/${var.prefix}-${var.name}"
    retention_in_days = 365
}