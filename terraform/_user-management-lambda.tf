module users-management-lambda-role {
    source          = "./modules/lambda-role"
    iam_role_name   = "${local.prefix}-role-${var.users-management-lambda-name}-${var.aws_region}"
}

module "users-management-lambda" {
    source = "./modules/lambda"

    prefix               = local.prefix
    name                 = var.users-management-lambda-name
    role_arn             = module.users-management-lambda-role.arn
    description          = var.lambda_description
    filename             = var.users-management-lambda-filename
    runtime              = var.runtime
    handler              = var.handler
    memory_size          = var.memory_size
    timeout              = var.lambda_timeout
    publish              = var.publish
    lambda_alias_current = var.lambda_alias_current 

    environment_variables = {
        "SWAGGER_ENABLED"          = true
        "Serilog__MinimumLogLevel" = "Debug"
    }  
}

data "archive_file" "lambda_archive" {
    type = "zip"
    source_dir = "${path.module}/../UserManagementLambda"
    output_path = "${path.module}/../${var.users-management-lambda-name}"
}