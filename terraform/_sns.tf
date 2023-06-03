module "user-management-lambda-sns" {
    source = "./modules/sns"

    sns_name = "${var.prefix}-${var.user_management_lambda_name}-sns"
}

module "product-management-lambda-sns" {
    source = "./modules/sns"

    sns_name = "${var.prefix}-${var.product_management_lambda_name}-sns"
}

data "aws_iam_policy_document" "users_integration_put_to_sqs" {
  statement {
    sid    = "PutMessagesToSQS"
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["sns.amazonaws.com"]
    }

    actions   = ["sqs:SendMessage"]
    resources = [ module.user-integration-lambda-sqs.arn ]

    condition {
      test     = "ArnEquals"
      variable = "aws:SourceArn"
      values   = [ module.user-management-lambda-sns.arn]
    }
  }
}

resource "aws_sqs_queue_policy" "users_subscription" {
  queue_url = module.user-integration-lambda-sqs.url
  policy = data.aws_iam_policy_document.users_integration_put_to_sqs.json
}

data "aws_iam_policy_document" "license_management_put_to_sqs" {
  statement {
    sid    = "PutMessagesToSQS"
    effect = "Allow"

    principals {
      type        = "Service"
      identifiers = ["sns.amazonaws.com"]
    }

    actions   = ["sqs:SendMessage"]
    resources = [ module.license-management-lambda-sqs.arn ]

    condition {
      test     = "ArnEquals"
      variable = "aws:SourceArn"
      values   = [ module.user-management-lambda-sns.arn, product-management-lambda-sns.arn]
    }
  }
}

resource "aws_sqs_queue_policy" "license_subscription" {
  queue_url = module.license-management-lambda-sqs.url
  policy = data.aws_iam_policy_document.license_management_put_to_sqs.json
}