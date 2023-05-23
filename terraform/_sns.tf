module "user-management-lambda-sns" {
    source = "./modules/sns"

    sns_name = "${var.prefix}-${var.user_management_lambda_name}-sns"
}