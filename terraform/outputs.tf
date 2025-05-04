output "identity_db_host" {
    value = yandex_mdb_postgresql_cluster.db.host[0].fqdn
}