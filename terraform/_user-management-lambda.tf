module "user-management-lambda-role" {
    source          = "./modules/lambda-role"
    iam_role_name   = "${var.prefix}-role-${var.user_management_lambda_name}-${var.aws_region}"
}
 
module "user-management-lambda" {
    source = "./modules/lambda"

    prefix               = var.prefix
    name                 = var.user_management_lambda_name
    role_arn             = module.user-management-lambda-role.arn
    description          = var.user_management_lambda_description
    filename             = var.user_management_lambda_filename
    runtime              = var.runtime
    handler              = var.user_management_lambda_handler
    memory_size          = var.memory_size
    timeout              = var.lambda_timeout
    publish              = var.publish
    lambda_alias_current = var.lambda_alias_current 

    environment_variables = { 
        "SWAGGER_ENABLED"          = true
        "Serilog__MinimumLogLevel" = "Debug"
        "Parameters__SNS_Topic_ARN"  = module.user-management-lambda-sns.arn
    }  
}