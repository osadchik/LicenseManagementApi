# Providers
aws_region = "eu-central-1"

# Users Management Lambda
users-management-lambda-name = "users-management-lambda"
lambda_description           = "Automated deployment of Users Management Lambda"
runtime                      = "dotnet6"
handler                      = "UserManagementLambda::UserManagementLambda.LambdaEntryPoint::FunctionHandlerAsync"
memory_size                  = "512"
lambda_timeout               = "15"
publish                      = true
lambda_alias_current         = "current_version"
filename                     = null

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