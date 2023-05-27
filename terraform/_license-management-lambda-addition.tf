resource "aws_iam_policy" "license_management_dynamoDB_policy" {
    name = "${var.prefix}-${var.license_management_lambda_name}-dynamodb-policy"

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
          "dynamodb:GetItem"
        ]
        Resource = [ "${module.dynamodb_license_table.arn}", "${module.dynamodb_entitlements_table.arn}" ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "license-management-dynamodb-access" {
    role       = module.license-management-lambda-role.name
    policy_arn = aws_iam_policy.license_management_dynamoDB_policy.arn
}

resource "aws_iam_role_policy_attachment" "license_management_lambda_logs" {
  role         = module.license-management-lambda-role.name
  policy_arn   = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}