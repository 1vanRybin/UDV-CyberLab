
# Сервис для Identity
resource "yandex_serverless_container" "identity" {
  name        = "identity-service"
  description = "Identity microservice"
  memory      = 512
  cores       = 1
  execution_timeout = "10s"
  service_account_id = yandex_iam_service_account.neolab_sa.id

  image {
    url = "cr.yandex/crpkkgbssht60t8ch9sd/identity:latest"
    environment = {
      ConnectionStrings__DefaultConnection = "User ID=${var.db_user};Password=${var.db_password};Host=${yandex_mdb_postgresql_cluster.db.host[0].fqdn};Port=${var.db_port};Database=${var.identity_db_name};Pooling=true;"
    }
  }

  connectivity {
    network_id = yandex_vpc_network.main.id
  }
}

# Сервис для Projects
resource "yandex_serverless_container" "projects" {
  name        = "projects-service"
  description = "Projects microservice"
  memory      = 512
  cores       = 1
  execution_timeout = "10s"
  service_account_id = yandex_iam_service_account.neolab_sa.id

  image {
    url = "cr.yandex/crpkkgbssht60t8ch9sd/projects:latest"
    environment = {
      ConnectionStrings__DefaultConnection = "User ID=${var.db_user};Password=${var.db_password};Host=${yandex_mdb_postgresql_cluster.db.host[0].fqdn};Port=${var.db_port};Database=${var.projects_db_name};Pooling=true;",
      YandexStorage__BucketName = "udv-bucket",
      YandexStorage__ServiceUrl = "https://storage.yandexcloud.net",
      YandexStorage__AccessKey = "***",
      YandexStorage__SecretKey = "***"
    }
  }

  connectivity {
    network_id = yandex_vpc_network.main.id
  }
}

# Сервис для Tests
resource "yandex_serverless_container" "tests" {
  name        = "tests-service"
  description = "Tests microservice"
  memory      = 512
  cores       = 1
  execution_timeout = "30s"
  service_account_id = yandex_iam_service_account.neolab_sa.id

  image {
    url = "cr.yandex/crpkkgbssht60t8ch9sd/tests:latest"
    environment     = {
      ConnectionStrings__DefaultConnection = "User ID=${var.db_user};Password=${var.db_password};Host=${yandex_mdb_postgresql_cluster.db.host[0].fqdn};Port=${var.db_port};Database=${var.tests_db_name};Pooling=true;"
    }
  }


  connectivity {
    network_id = yandex_vpc_network.main.id
  }
}
