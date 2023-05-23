resource "aws_iam_policy" "user_integration_dynamoDB_policy" {
    name = "${var.prefix}-${var.user_integration_lambda_name}-dynamodb-policy"

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
        Resource = [ "${module.dynamodb_state_table.arn}" ]
      }
    ]
  })
}

resource "aws_iam_policy" "user_integration_sqs_policy" {
  name = "${var.prefix}-${var.user_integration_lambda_name}-sns-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "sqs:ReceiveMessage",
          "sqs:DeleteMessage",
          "sqs:GetQueueAttributes"
        ]
        Resource = [ "${module.user-integration-lambda-sqs.arn}" ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "user_integration_dynamodb_access" {
    role       = module.user-integration-lambda-role.name
    policy_arn = aws_iam_policy.user_integration_dynamoDB_policy.arn
}

resource "aws_iam_role_policy_attachment" "user_integration_sqs_access" {
    role       = module.user-integration-lambda-role.name
    policy_arn = aws_iam_policy.user_integration_sqs_policy.arn
}

resource "aws_iam_role_policy_attachment" "user_integration_lambda_logs" {
  role         = module.user-integration-lambda-role.name
  policy_arn   = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}