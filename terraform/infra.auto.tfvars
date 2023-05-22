# Providers
aws_region = "eu-central-1"

# Shared
prefix = "LicenseManagement"

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