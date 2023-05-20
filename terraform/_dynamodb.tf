module "dynamodb_users_table" {
    source = "./modules/dynamodb"
    
    name = "LicenseManagement-Users"
    billing_mode = var.billing_mode
    read_capacity = var.read_capacity
    write_capacity = var.write_capacity
    hash_key = "Uuid"

    attributes = [
        {
            name = "Uuid"
            type = "S"
        }
    ]
}

module "dynamodb_product_table" {
    source = "./modules/dynamodb"
    
    name = "LicenseManagement-Products"
    billing_mode = var.billing_mode
    read_capacity = var.read_capacity
    write_capacity = var.write_capacity
    hash_key = "ProductId"

    attributes = [
        {
            name = "ProductId"
            type = "S"
        }
    ]
}

module "dynamodb_license_table" {
    source = "./modules/dynamodb"
    
    name = "LicenseManagement-Licenses"
    billing_mode = var.billing_mode
    read_capacity = var.read_capacity
    write_capacity = var.write_capacity
    hash_key = "LicenseId"

    attributes = [
        {
            name = "LicenseId"
            type = "S"
        }
    ]
}

module "dynamodb_state_table" {
    source = "./modules/dynamodb"
    
    name = "LicenseManagement-CircuitBreakerState"
    billing_mode = var.billing_mode
    read_capacity = var.read_capacity
    write_capacity = var.write_capacity
    hash_key = var.hash_key

    attributes = var.attributes
}