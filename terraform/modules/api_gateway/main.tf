resource "aws_api_gateway_rest_api" "lambda_api" {
    name        = var.name
    description = "Automated deployment of API Gateway for Lambda"
}

# Users API endpoint
resource "aws_api_gateway_resource" "users-api" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_rest_api.lambda_api.root_resource_id
    path_part   = "users-api" 
}

resource "aws_api_gateway_method" "users-api-get-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.users-api.id
    http_method   = "GET"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "users-api-root-get-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.users-api-get-method.resource_id
    http_method = aws_api_gateway_method.users-api-get-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.users_uri
}

resource "aws_api_gateway_resource" "users-api-proxy" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_resource.users-api.id
    path_part   = "{proxy+}" 
}

resource "aws_api_gateway_method" "users-api-proxy-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.users-api-proxy.id
    http_method   = "ANY"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "users-api-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.users-api-proxy-method.resource_id
    http_method = aws_api_gateway_method.users-api-proxy-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.users_uri
}

# Products API endpoint
resource "aws_api_gateway_resource" "products-api" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_rest_api.lambda_api.root_resource_id
    path_part   = "products-api" 
}

resource "aws_api_gateway_method" "products-api-get-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.products-api.id
    http_method   = "GET"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "products-api-root-get-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.products-api-get-method.resource_id
    http_method = aws_api_gateway_method.products-api-get-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.products_uri
}

resource "aws_api_gateway_resource" "products-api-proxy" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_resource.products-api.id
    path_part   = "{proxy+}" 
}

resource "aws_api_gateway_method" "products-api-proxy-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.products-api-proxy.id
    http_method   = "ANY"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "products-api-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.products-api-proxy-method.resource_id
    http_method = aws_api_gateway_method.products-api-proxy-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.products_uri
}

#License API endpoint
resource "aws_api_gateway_resource" "license-api" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_rest_api.lambda_api.root_resource_id
    path_part   = "license-api" 
}

resource "aws_api_gateway_method" "license-api-get-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.license-api.id
    http_method   = "GET"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "license-api-root-get-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.license-api-get-method.resource_id
    http_method = aws_api_gateway_method.license-api-get-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.license_uri
}

resource "aws_api_gateway_resource" "license-api-proxy" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_resource.license-api.id
    path_part   = "{proxy+}" 
}

resource "aws_api_gateway_method" "license-api-proxy-method" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.license-api-proxy.id
    http_method   = "ANY"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "license-api-integration" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.license-api-proxy-method.resource_id
    http_method = aws_api_gateway_method.license-api-proxy-method.http_method

    integration_http_method = "POST"
    type                    = "AWS_PROXY"
    uri                     = var.license_uri
}

# REST API Deployment in AWS
resource "aws_api_gateway_deployment" "api_deploy" {
    depends_on = [ 
        aws_api_gateway_method.users-api-get-method,
        aws_api_gateway_method.users-api-proxy-method,
        aws_api_gateway_method.license-api-get-method,
        aws_api_gateway_method.license-api-proxy-method,
        aws_api_gateway_method.products-api-get-method,
        aws_api_gateway_method.products-api-proxy-method
    ]

    rest_api_id = aws_api_gateway_rest_api.lambda_api.id

    triggers = {
        redeployment = sha1(jsonencode(aws_api_gateway_rest_api.lambda_api.body))
    }

    lifecycle {
      create_before_destroy = true
    }
}

resource "aws_api_gateway_stage" "development_stage" {
    deployment_id = aws_api_gateway_deployment.api_deploy.id
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    stage_name    = var.stage_name 
}

# Add permissions for API Gateway to call all lambdas
resource "aws_lambda_permission" "function_permission_for_apigateway" {
    for_each      = var.lambda_names

    statement_id  = "AllowAPIGatewayInvoke"
    action        = "lambda:InvokeFunction"
    function_name = each.key
    principal     = "apigateway.amazonaws.com"

    source_arn    = "${aws_api_gateway_rest_api.lambda_api.execution_arn}/*/*" 
}