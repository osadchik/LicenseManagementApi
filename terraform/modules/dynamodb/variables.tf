variable "name" {
  description = "Name of the DynamoDB table"
  type = string
}

variable "hash_key" {
  description = "The attribute to use as the hash (partition) key. Must also be defined as an attribute"
  type = string
}

variable "billing_mode" {
  description = "Controls how you are billed for read/write capacity"
  type = string
  default = "PAY_PER_REQUEST"
}

variable "read_capacity" {
  description = "The number of read units for this table"
  type = number
  default = 0
}

variable "write_capacity" {
  description = "The number of write unites for this table"
  type = number
  default = 0
}

variable "attributes" {
  description = "List of nested attribute definitions. Only required for hash_key and range_key attributes"
  type = list(map(string))
  default = []
}
