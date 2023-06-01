module "license-management-lambda-role" {
    source          = "./modules/lambda-role"
    iam_role_name   = "${var.prefix}-role-${var.license_management_lambda_name}-${var.aws_region}"
}
 
module "license-management-lambda" {
    source = "./modules/lambda"

    prefix               = var.prefix
    name                 = var.license_management_lambda_name
    role_arn             = module.license-management-lambda-role.arn
    description          = var.license_management_lambda_description
    filename             = var.license_management_lambda_filename
    runtime              = var.runtime
    handler              = var.license_management_lambda_handler
    memory_size          = var.memory_size
    timeout              = var.lambda_timeout
    publish              = var.publish
    lambda_alias_current = var.lambda_alias_current 

    environment_variables = {
        "SWAGGER_ENABLED" = true
        "Parameters__ProductsApiUrl" = "${module.api_gateway.invoke_url}${var.products-prefix}/"
        "Parameters__UsersApiUrl"    = "${module.api_gateway.invoke_url}${var.users-prefix}/"
    }  
}

# SQS Lambda Event Source
resource "aws_lambda_event_source_mapping" "license-management-sqs-event-source" {
    event_source_arn = module.license-management-lambda-sqs.arn
    function_name    = module.license-management-lambda.lambda_arn 
    batch_size       = 1
}