yc_token     = "y0__xCine6bAhjB3RMgkrHKvRKhAgErlsZyVrXOabGZBgNs52MnIg"
cloud_id     = "b1ggs33li91omh22uqkk"
folder_id    = "b1gvng28tc50l6cqqhfb"
default_zone = "ru-central1-a"

vpc_name     = "neolab-network"
subnet_cidr  = "10.0.0.0/16"

vm_name      = "neolab-server"
vm_image_id  = "fd841mrf59dg3qut0om3"
vm_cores     = 2
vm_memory    = 4
vm_disk_size = 10

ssh_key_path = "~/.ssh/id_rsa.pub"

identity_db_name = "identityNEOLab"
projects_db_name = "projectsNEOLab"
tests_db_name    = "testsNEOLab"
db_user          = "user1"
db_password      = "aA258007195" # Лучше заменить позже на секрет из Vault или переменной окружения
db_port          = 6432

alb_name  = "neolab-main-lb"
alb_port  = 80

dashboard_name         = "NEO Lab System Overview"
service_account_name   = "neolab-service-account"
