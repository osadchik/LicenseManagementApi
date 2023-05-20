resource "aws_dynamodb_table" "this" {
    name = var.name
    billing_mode = var.billing_mode
    hash_key = var.hash_key
    read_capacity = var.read_capacity
    write_capacity = var.write_capacity

    dynamic attribute {
      for_each = var.attributes

      content {
        name = attribute.value.name
        type = attribute.value.type
      }
    }
}