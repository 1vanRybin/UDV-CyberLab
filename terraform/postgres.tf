resource "yandex_mdb_postgresql_cluster" "db" {
  name        = "neolab-postgres"
  environment = "PRODUCTION"
  network_id  = yandex_vpc_network.main.id

  config {
    version = 14
    resources {
      resource_preset_id = "s2.micro"
      disk_type_id       = "network-hdd"
      disk_size          = 10
    }
  }


  host {
    zone      = var.default_zone
    subnet_id = yandex_vpc_subnet.main.id
  }

  database {
    name = var.identity_db_name
    owner = var.db_user
      extension {
          name = "hstore"
      }
  }

  database {
    name = var.tests_db_name
    owner = var.db_user
      extension {
          name = "hstore"
      }
  }

  database {
    name = var.projects_db_name
    owner = var.db_user
      extension {
          name = "hstore"
      }
  }

  user {
    name     = var.db_user
    password = var.db_password
  }
}
