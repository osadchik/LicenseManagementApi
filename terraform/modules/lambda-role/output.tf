output "name" {
    description = "AWS IAM Role name"
    value = aws_iam_role.lambda_role.name
}

output "arn" {
    description = "AWS IAM Role arn"
    value = aws_iam_role.lambda_role.arn
}