# Сеть (VPC)
resource "yandex_vpc_network" "main" {
  name = var.vpc_name
}

# Подсеть
resource "yandex_vpc_subnet" "main" {
  name           = "${var.vpc_name}-subnet"
  zone           = var.default_zone
  network_id     = yandex_vpc_network.main.id
  v4_cidr_blocks = [var.subnet_cidr]
}

