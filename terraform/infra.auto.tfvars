# Providers
aws_region = "eu-central-1"

# Shared
prefix = "LicenseService"

# Lambda Common
runtime                      = "dotnet6"
memory_size                  = "512"
lambda_timeout               = "15"
publish                      = true
lambda_alias_current         = "current_version"

# Users Management Lambda
user_management_lambda_name        = "users-management-lambda"
user_management_lambda_description = "Automated deployment of Users Management Lambda"
user_management_lambda_handler     = "UserManagementLambda::UserManagementLambda.LambdaEntryPoint::FunctionHandlerAsync"

# Users Integration Lambda
user_integration_lambda_name        = "users-integration-lambda"
user_integration_lambda_description = "Automated deployment of Users Integration Lambda"
user_integration_lambda_handler     = "UserIntegrationLambda::UserIntegrationLambda.Function::FunctionHandler"

# License Management Lambda
license_management_lambda_name        = "license-management-lambda"
license_management_lambda_description = "Automated deployment of License Management Lambda"
license_management_lambda_handler     = "LicenseManagementLambda::LicenseManagementLambda.Function::FunctionHandlerAsync"

# Product Management Lambda
product_management_lambda_name        = "product-management-lambda"
product_management_lambda_description = "Automated deployment of Product Management Lambda"
product_management_lambda_handler     = "ProductManagementLambda::ProductManagementLambda.LambdaEntryPoint::FunctionHandlerAsync"

# Dynamo DB
billing_mode    = "PAY_PER_REQUEST"
read_capacity   = "0"
write_capacity  = "0"
hash_key        = "Id"

attributes      = [
    {
        name    = "Id"
        type    = "S"
    }
]