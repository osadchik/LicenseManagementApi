module "product-management-lambda-role" {
    source          = "./modules/lambda-role"
    iam_role_name   = "${var.prefix}-role-${var.product_management_lambda_name}-${var.aws_region}"
}
 
module "product-management-lambda" {
    source = "./modules/lambda"

    prefix               = var.prefix
    name                 = var.product_management_lambda_name
    role_arn             = module.product-management-lambda-role.arn
    description          = var.product_management_lambda_description
    filename             = var.product_management_lambda_filename
    runtime              = var.runtime
    handler              = var.product_management_lambda_handler
    memory_size          = var.memory_size
    timeout              = var.lambda_timeout
    publish              = var.publish
    lambda_alias_current = var.lambda_alias_current 

    environment_variables = { 
        "SWAGGER_ENABLED" = true
    }  
}