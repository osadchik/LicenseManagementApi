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
        "SWAGGER_ENABLED"          = true
        "Serilog__MinimumLogLevel" = "Debug"
    }  
}

# SQS Lambda Event Source
resource "aws_lambda_event_source_mapping" "event_source_mapping" {
    event_source_arn = module.user-integration-lambda-sqs.arn
    function_name    = module.user-integration-lambda.lambda_arn 
    batch_size       = 1
}