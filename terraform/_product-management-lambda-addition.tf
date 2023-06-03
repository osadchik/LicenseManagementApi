resource "aws_iam_policy" "product_management_dynamoDB_policy" {
    name = "${var.prefix}-${var.product_management_lambda_name}-dynamodb-policy"

    policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "dynamodb:DescribeTable",
          "dynamodb:DeleteItem",
          "dynamodb:UpdateItem",
          "dynamodb:PutItem",
          "dynamodb:GetItem",
          "dynamodb:Scan"
        ]
        Resource = [ "${module.dynamodb_product_table.arn}" ]
      }
    ]
  })
}

resource "aws_iam_policy" "product_management_snsPublish_policy" {
  name = "${var.prefix}-${var.product_management_lambda_name}-sns-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "sns:Publish"
        ]
        Resource = [ "${module.product-management-lambda-sns.arn}" ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "product_management_dynamodb_access" {
    role       = module.product-management-lambda-role.name
    policy_arn = aws_iam_policy.product_management_dynamoDB_policy.arn
}

resource "aws_iam_role_policy_attachment" "product_management_sns_access" {
    role       = module.product-management-lambda-role.name
    policy_arn = aws_iam_policy.product_management_snsPublish_policy.arn
}

resource "aws_iam_role_policy_attachment" "product_management_lambda_logs" {
  role         = module.product-management-lambda-role.name
  policy_arn   = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}