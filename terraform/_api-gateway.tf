module "api_gateway" {
    source      = "./modules/api_gateway"
    lambda_name = module.user-management-lambda.lambda_name
    uri         = module.user-management-lambda.lambda_invoke_arn
}