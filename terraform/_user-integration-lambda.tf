module "user-integration-lambda-role" {
    source          = "./modules/lambda-role"
    iam_role_name   = "${var.prefix}-role-${var.user_integration_lambda_name}-${var.aws_region}"
}
 
module "user-integration-lambda" {
    source = "./modules/lambda"

    prefix               = var.prefix
    name                 = var.user_integration_lambda_name
    role_arn             = module.user-integration-lambda-role.arn
    description          = var.user_integration_lambda_description
    filename             = var.user_integration_lambda_filename
    runtime              = var.runtime
    handler              = var.user_integration_lambda_handler
    memory_size          = var.memory_size
    timeout              = var.lambda_timeout
    publish              = var.publish
    lambda_alias_current = var.lambda_alias_current 

    environment_variables = { 
        "SWAGGER_ENABLED"                       = true
        "Serilog__MinimumLogLevel"              = "Debug"
        "Parameters__UsersTableName"            = module.dynamodb_users_table.name
        "EventBridge__RuleName"                 = aws_cloudwatch_event_rule.lambda_schedule.id
        "CircuitBreaker__CircuitStateTableName" = module.dynamodb_state_table.name
        "CircuitBreaker__ErrorThreshold"        = 1
        "CircuitBreaker__SamplingDuration"      = "00:05:00"
        "CircuitBreaker__Timeout"               = "00:01:00"
        "CircuitBreaker__DeadLetterQueueUrl"    = module.user-integration-lambda-dlq.url
        "CircuitBreaker__SourceQueueUrl"        = module.user-integration-lambda-sqs.url
    }  
}

# SQS Lambda Event Source
resource "aws_lambda_event_source_mapping" "users-integration-sqs-event-source" {
    event_source_arn = module.user-integration-lambda-sqs.arn
    function_name    = module.user-integration-lambda.lambda_arn 
    batch_size       = 1
}