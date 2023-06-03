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
          "dynamodb:GetItem",
          "dynamodb:Scan"
        ]
        Resource = [ "${module.dynamodb_state_table.arn}", "${module.dynamodb_users_table.arn}" ]
      }
    ]
  })
}

resource "aws_iam_policy" "user_integration_sqs_policy" {
  name = "${var.prefix}-${var.user_integration_lambda_name}-sqs-policy"

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
      },
      {
        Effect = "Allow"
        Action = [
          "sqs:SendMessage"
        ]
        Resource = [ "${module.user-integration-lambda-dlq.arn}" ]
      }
    ]
  })
}

resource "aws_iam_policy" "user_integration_event_mapping_policy" {
  name = "${var.prefix}-${var.user_integration_lambda_name}-event-mapping-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "lambda:ListEventSourceMappings",
          "lambda:UpdateEventSourceMapping"
        ]
        Resource = [ "*" ]
      },
      {
        Effect = "Allow"
        Action = [
          "events:PutRule"
        ]
        Resource = [ aws_cloudwatch_event_rule.lambda_schedule.arn ]
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

resource "aws_iam_role_policy_attachment" "user_integration_event_mapping" {
  role         = module.user-integration-lambda-role.name
  policy_arn   = aws_iam_policy.user_integration_event_mapping_policy.arn
}