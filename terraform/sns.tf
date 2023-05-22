module "user-management-lambda-sns" {
    source = "./modules/sns"

    sns_name = var.user_management_sns_name
}