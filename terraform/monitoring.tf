resource "yandex_monitoring_dashboard" "neolab_dashboard" {
  name        = "neolab-dashboard"
  title       = "Neolab Monitoring"
  description = "Дашборд для мониторинга микросервисов и базы данных"

  widgets {
    chart {
      chart_id       = "chart1id"
      title       = "Оперативка на Кластер"
      queries {
        target {
        query = <<-EOT
          mem.used_bytes{
            service="managed-postgresql",
            host="${yandex_mdb_postgresql_cluster.db.host[0].fqdn}"
          }
        EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_AVG"
        show_labels = true
      }

    }
    position {
      x = 0
      y = 0
      w = 10
      h = 10
    }
  }

  widgets {
    chart {
      chart_id       = "chart2id"
      title       = "Диск на кластер"
      queries {
        target {
        query = <<-EOT
          disk.used_bytes{
            service="managed-postgresql",
            host="${yandex_mdb_postgresql_cluster.db.host[0].fqdn}"
          }
        EOT
          text_mode = true
        }
      }
      visualization_settings {
        type        = "VISUALIZATION_TYPE_LINE"
        aggregation = "SERIES_AGGREGATION_AVG"
        show_labels = true
      }

    }
    position {
      x = 10
      y = 0
      w = 10
      h = 10
    }
  }

    widgets {
      chart {
        chart_id       = "chart3id"
        title       = "CPU на Графану"
        queries {
          target {
          query = <<-EOT
            cpu_usage{
              service="compute",
              resource_type="vm",
              resource_id="${yandex_compute_instance.grafana.id}"
            }
          EOT
            text_mode = true
          }
        }
        visualization_settings {
          type        = "VISUALIZATION_TYPE_LINE"
          aggregation = "SERIES_AGGREGATION_AVG"
          show_labels = true
        }

      }
      position {
        x = 0
        y = 10
        w = 10
        h = 10
      }
    }

    widgets {
      chart {
        chart_id       = "chart4id"
        title       = "CPU на Прометеус"
        queries {
          target {
          query = <<-EOT
            cpu_usage{
              service="compute",
              resource_type="vm",
              resource_id="${yandex_compute_instance.prometheus.id}"
            }
          EOT
            text_mode = true
          }
        }
        visualization_settings {
          type        = "VISUALIZATION_TYPE_LINE"
          aggregation = "SERIES_AGGREGATION_AVG"
          show_labels = true
        }

      }
      position {
        x = 10
        y = 10
        w = 10
        h = 10
      }
    }

    widgets {
      chart {
        chart_id       = "chart5id"
        title       = "Ошибки на Gateway"
        queries {
          target {
          query = <<-EOT
            api_gateway.errors_count_per_second{
              service="serverless-apigateway"
            }
          EOT
            text_mode = true
          }
        }
        visualization_settings {
          type        = "VISUALIZATION_TYPE_LINE"
          aggregation = "SERIES_AGGREGATION_AVG"
          show_labels = true
        }

      }
      position {
        x = 0
        y = 20
        w = 10
        h = 10
      }
    }

    widgets {
      chart {
        chart_id       = "chart6id"
        title       = "Время выполнения функции в мс"
        queries {
          target {
          query = <<-EOT
            serverless.containers.execution_time_milliseconds{
              service="serverless-containers"
            }
          EOT
            text_mode = true
          }
        }
        visualization_settings {
          type        = "VISUALIZATION_TYPE_LINE"
          aggregation = "SERIES_AGGREGATION_AVG"
          show_labels = true
        }

      }
      position {
        x = 10
        y = 20
        w = 10
        h = 10
      }
    }
}
