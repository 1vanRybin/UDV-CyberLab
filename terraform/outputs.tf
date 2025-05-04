output "identity_db_host" {
    value = yandex_mdb_postgresql_cluster.db.host[0].fqdn
}

output "prometheus_ip" {
  value = yandex_compute_instance.prometheus.network_interface[0].ip_address
}

output "grafana_ip" {
  value = yandex_compute_instance.grafana.network_interface[0].ip_address
}