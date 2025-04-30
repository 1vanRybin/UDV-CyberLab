# Создание сервисного аккаунта
resource "yandex_iam_service_account" "neolab_sa" {
  name        = var.service_account_name
  description = "Service Account for NEOLab services"
}

resource "yandex_resourcemanager_folder_iam_member" "neolab_roles" {
  for_each = toset([
    "editor",
    "container-registry.images.puller",
    "monitoring.editor",
    "mdb.admin",
    "storage.admin"
  ])
  folder_id = var.folder_id
  role      = each.key
  member    = "serviceAccount:${yandex_iam_service_account.neolab_sa.id}"
}

# Создание статического ключа для сервисного аккаунта
resource "yandex_iam_service_account_static_access_key" "neolab_sa_key" {
  service_account_id = yandex_iam_service_account.neolab_sa.id
  description        = "Static key for accessing services via Terraform or SDK"
}
