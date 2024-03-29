module "api_gateway" {
    source      = "./modules/api_gateway"

    name         = "${var.prefix}-api-gateway"
    stage_name   = "dev" 
    lambda_names = [
        module.user-management-lambda.lambda_name,
        module.license-management-lambda.lambda_name,
        module.product-management-lambda.lambda_name
    ]
    users_uri    = module.user-management-lambda.lambda_invoke_arn
    license_uri  = module.license-management-lambda.lambda_invoke_arn
    products_uri = module.product-management-lambda.lambda_invoke_arn
}