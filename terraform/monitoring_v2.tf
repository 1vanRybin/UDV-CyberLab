# Виртуальная машина для Prometheus

resource "yandex_compute_instance" "prometheus" {
  name        = "prometheus-instance"
  platform_id = "standard-v1"
  zone        = var.default_zone
  boot_disk {
    initialize_params {
      image_id = var.vm_image_id
    }
  }
  network_interface {
    subnet_id  = yandex_vpc_subnet.main.id
    nat        = true
  }

    resources {
      cores    = var.vm_cores
      memory   = var.vm_memory
    core_fraction = var.vm_disk_size
    }

  metadata = {
    ssh-keys = "kirill:${file("~/.ssh/id_ed25519.pub")}"
    user_data = <<-EOF
      #!/bin/bash
      apt-get update
      apt-get install -y prometheus

      # Конфигурация Prometheus
      cat > /etc/prometheus/prometheus.yml <<EOL
      global:
        scrape_interval: 15s
      scrape_configs:
        - job_name: 'prometheus'
          static_configs:
            - targets: ['localhost:9090']
      EOL

      # Запуск Prometheus
      systemctl start prometheus
      systemctl enable prometheus
    EOF
  }

  service_account_id = yandex_iam_service_account.neolab_sa.id
}

# Виртуальная машина для Grafana

resource "yandex_compute_instance" "grafana" {
  name        = "grafana-instance"
  platform_id = "standard-v1"
  zone        = var.default_zone
  boot_disk {
    initialize_params {
      image_id = var.vm_image_id
    }
  }
  network_interface {
    subnet_id  = yandex_vpc_subnet.main.id
    nat        = true
  }

    resources {
      cores    = var.vm_cores
      memory   = var.vm_memory
    core_fraction = var.vm_disk_size
    }

  metadata = {
    ssh-keys = "kirill:${file("~/.ssh/id_ed25519.pub")}"
    user_data = <<-EOF
      #!/bin/bash
      apt-get update
      apt-get install -y software-properties-common
      add-apt-repository "deb https://packages.grafana.com/oss/deb stable main"
      apt-get update
      apt-get install grafana

      # Запуск Grafana
      systemctl start grafana-server
      systemctl enable grafana-server
    EOF
  }

  service_account_id = yandex_iam_service_account.neolab_sa.id
}