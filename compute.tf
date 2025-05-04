# Виртуальная машина для сервиса Identity
resource "yandex_compute_instance" "identity_vm" {
  name        = "identity-service"
  platform_id = "standard-v2"
  zone        = var.default_zone

  resources {
    cores  = var.vm_cores
    memory = var.vm_memory
  }

  boot_disk {
    initialize_params {
      image_id = var.vm_image_id
      size     = var.vm_disk_size
    }
  }

  network_interface {
    subnet_id = yandex_vpc_subnet.main.id
    nat       = true
  }

  metadata = {
    ssh-keys = "ubuntu:${file(var.ssh_key_path)}"
  }
}

# Виртуальная машина для сервиса Projects
resource "yandex_compute_instance" "projects_vm" {
  name        = "projects-service"
  platform_id = "standard-v2"
  zone        = var.default_zone

  resources {
    cores  = var.vm_cores
    memory = var.vm_memory
  }

  boot_disk {
    initialize_params {
      image_id = var.vm_image_id
      size     = var.vm_disk_size
    }
  }

  network_interface {
    subnet_id = yandex_vpc_subnet.main.id
    nat       = true
  }

  metadata = {
    ssh-keys = "ubuntu:${file(var.ssh_key_path)}"
  }
}

# Виртуальная машина для сервиса Tests
resource "yandex_compute_instance" "tests_vm" {
  name        = "tests-service"
  platform_id = "standard-v2"
  zone        = var.default_zone

  resources {
    cores  = var.vm_cores
    memory = var.vm_memory
  }

  boot_disk {
    initialize_params {
      image_id = var.vm_image_id
      size     = var.vm_disk_size
    }
  }

  network_interface {
    subnet_id = yandex_vpc_subnet.main.id
    nat       = true
  }

  metadata = {
    ssh-keys = "ubuntu:${file(var.ssh_key_path)}"
  }
}