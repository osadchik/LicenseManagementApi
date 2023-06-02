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
    topic_arn            = module.user-management-lambda-sns.arn
    protocol             = "sqs"
    endpoint             = module.user-integration-lambda-sqs.arn
    raw_message_delivery = true
}

module "license-management-lambda-sqs" {
    source         = "./modules/sqs"

    sqs_name       = "${var.prefix}-${var.license_management_lambda_name}-sqs"
    redrive_policy = "{\"deadLetterTargetArn\" : \"${module.license-management-lambda-dlq.arn}\", \"maxReceiveCount\": 4}"
}

module "license-management-lambda-dlq" {
    source = "./modules/sqs"

    sqs_name = "${var.prefix}-${var.license_management_lambda_name}-deadletterqueue"
}

resource "aws_sns_topic_subscription" "license-management_sqs_target" {
    topic_arn            = module.user-management-lambda-sns.arn
    protocol             = "sqs"
    endpoint             = module.license-management-lambda-sqs.arn
    raw_message_delivery = true
    filter_policy   	 = "${jsonencode(map("Action", list("Delete", "Update")))}"
}
