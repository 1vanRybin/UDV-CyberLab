# Авторизация
variable "yc_token" {
    default = "y0__xCine6bAhjB3RMgkrHKvRKhAgErlsZyVrXOabGZBgNs52MnIg"
}
variable "cloud_id" {
    default = "b1ggs33li91omh22uqkk"
}
variable "folder_id" {
    default = "b1gvng28tc50l6cqqhfb"
}

variable "default_zone" {
    default = "ru-central1-a"
}

# Сеть
variable "vpc_name" {
  default = "neolab-network"
}
variable "subnet_cidr" {
  default = "10.0.0.0/16"
}

# VM параметры
variable "vm_name" {
  default = "neolab-server"
}
variable "vm_image_id" {
  default = "fd8do9vdccgdhgt5lodk" # Ubuntu 20.04
}
variable "vm_cores" {
  default = 2
}
variable "vm_memory" {
  default = 4
}
variable "vm_disk_size" {
  default = 10
}

# SSH
variable "ssh_key_path" {
  default = "~/.ssh/id_rsa.pub"
}

# БД
variable "identity_db_name" {
  default = "identityNEOLab"
}
variable "projects_db_name" {
  default = "projectsNEOLab"
}
variable "tests_db_name" {
  default = "testsNEOLab"
}
variable "db_user" {
  default = "user1"
}
variable "db_password" {
  default = "aA258007195"
}
variable "db_port" {
  default = 6432
}

# Балансировщик
variable "alb_name" {
  default = "neolab-main-lb"
}
variable "alb_port" {
  default = 80
}

# Мониторинг
variable "dashboard_name" {
  default = "NEO Lab System Overview"
}

# IAM
variable "service_account_name" {
  default = "neolab-service-account"
}
