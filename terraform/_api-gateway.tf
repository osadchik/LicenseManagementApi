module "api_gateway" {
    source      = "./modules/api_gateway"
    lambda_name = module.users-management-lambda.lambda_name
    uri         = module.users-management-lambda.lambda_invoke_arn
}