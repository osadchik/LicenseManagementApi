terraform {
    required_providers {
      aws = {
        source = "hashicorp/aws"
      }
    }

    backend "s3" {
      bucket  = "licensemanagement-tfstate"
      key     = "state/terraform.tfstate"
      region  = "eu-central-1"
      encrypt = true
  }
}

provider "aws" {
    region =  var.aws_region
    shared_credentials_files = [ "~/.aws/credentials" ]
}