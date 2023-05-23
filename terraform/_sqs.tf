module "user-integration-lambda-sqs" {
    source         = "./modules/sqs"

    sqs_name       = "${var.prefix}-${var.user_integration_lambda_name}-sqs"
    redrive_policy = "{\"deadLetterTargetArn\" : \"${module.user-integration-lambda-dlq.arn}\", \"maxReceiveCount\": 4}"
}

module "user-integration-lambda-dlq" {
    source = "./modules/sqs"

    sqs_name = "${var.prefix}-${var.user_integration_lambda_name}-deadletterqueue"
}

resource "aws_sns_topic_subscription" "user_integration_sqs_target" {
    topic_arn = module.user-management-lambda-sns.arn
    protocol  = "sqs"
    endpoint = module.user-integration-lambda-sqs.arn
}
