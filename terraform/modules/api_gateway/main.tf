resource "aws_api_gateway_rest_api" "lambda_api" {
    name        = var.lambda_name
    description = "Automated deployment of API Gateway for Lambda"
}

resource "aws_api_gateway_resource" "proxy" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    parent_id   = aws_api_gateway_rest_api.lambda_api.root_resource_id
    path_part   = "{proxy+}" 
}

resource "aws_api_gateway_method" "proxy" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_resource.proxy.id
    http_method   = "ANY"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "lambda" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.proxy.resource_id
    http_method = aws_api_gateway_method.proxy.http_method

    integration_http_method = "POST"
    type                      = "AWS_PROXY"
    uri                       = var.uri
}

resource "aws_api_gateway_method" "proxy_root" {
    rest_api_id   = aws_api_gateway_rest_api.lambda_api.id
    resource_id   = aws_api_gateway_rest_api.lambda_api.root_resource_id
    http_method   = "ANY"
    authorization = "NONE"
}

resource "aws_api_gateway_integration" "lambda_root" {
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
    resource_id = aws_api_gateway_method.proxy_root.resource_id
    http_method = aws_api_gateway_method.proxy_root.http_method

    integration_http_method = "POST"
    type                      = "AWS_PROXY"
    uri                       = var.uri
}

resource "aws_api_gateway_deployment" "api_deploy" {
    depends_on  = [ aws_api_gateway_integration.lambda, aws_api_gateway_integration.lambda_root ]
    rest_api_id = aws_api_gateway_rest_api.lambda_api.id
}

resource "aws_lambda_permission" "function_permission_for_apigateway" {
    statement_id  = "AllowAPIGatewayInvoke"
    action        = "lambda:InvokeFunction"
    function_name = var.lambda_name
    principal     = "apigateway.amazonaws.com"

    source_arn    = "${aws_api_gateway_rest_api.lambda_api.execution_arn}/*/*" 
}