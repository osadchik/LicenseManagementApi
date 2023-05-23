resource "aws_iam_policy" "user_management_dynamoDB_policy" {
    name = "${var.prefix}-${var.user_management_lambda_name}-dynamodb-policy"

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
        Resource = [ "${module.dynamodb_users_table.arn}" ]
      }
    ]
  })
}

resource "aws_iam_policy" "user_management_snsPublish_policy" {
  name = "${var.prefix}-${var.user_management_lambda_name}-sns-policy"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Effect = "Allow"
        Action = [
          "sns:Publish"
        ]
        Resource = [ "${module.user-management-lambda-sns.arn}" ]
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "dynamodb-access" {
    role       = module.user-management-lambda-role.name
    policy_arn = aws_iam_policy.user_management_dynamoDB_policy.arn
}

resource "aws_iam_role_policy_attachment" "sns-access" {
    role       = module.user-management-lambda-role.name
    policy_arn = aws_iam_policy.user_management_snsPublish_policy.arn
}

resource "aws_iam_role_policy_attachment" "user_management_lambda_logs" {
  role         = module.user-management-lambda-role.name
  policy_arn   = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}