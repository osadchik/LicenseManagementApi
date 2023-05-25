resource "aws_cloudwatch_event_rule" "lambda_schedule" {
    name                = "${var.prefix}-schedule"
    is_enabled          = false
    schedule_expression = "rate(300 days)"
}

resource "aws_cloudwatch_event_target" "lambda_target" {
    rule      = aws_cloudwatch_event_rule.lambda_schedule.name
    target_id = aws_cloudwatch_event_rule.lambda_schedule.name
    arn       = module.user-integration-lambda.lambda_arn
    input     = jsonencode({
        "IsMaintenance" = true,
        "Action": "CircuitBreakerTrial"
    })
}

resource "aws_lambda_permission" "allow_cloudwatch_to_call_lambda" {
    statement_id  = "AllowExecutionFromCloudWatch"
    action        = "lambda:InvokeFunction"
    function_name = module.user-integration-lambda.lambda_name
    principal     = "events.amazonaws.com"
    source_arn    = aws_cloudwatch_event_rule.lambda_schedule.arn
}