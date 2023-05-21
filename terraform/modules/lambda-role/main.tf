data "aws_iam_policy_document" "lambda_can_assume_role" {
    statement {
      actions = [ "sts:AssumeRole" ]
      principals {
        type = "Service"
        identifiers = [ "lambda.amazonaws.com" ]
      }
    }
}

resource "aws_iam_role" "lambda_role" {
  name                  = var.iam_role_name
  assume_role_policy    = data.aws_iam_policy_document.lambda_can_assume_role.json
  force_detach_policies = true
  tags                  = var.tags
}

resource "aws_iam_role_policy_attachment" "lambda_logs" {
  role                  = aws_iam_role.lambda_role.name
  policy_arn            = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}