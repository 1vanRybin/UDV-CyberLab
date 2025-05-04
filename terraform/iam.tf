# Создание сервисного аккаунта
resource "yandex_iam_service_account" "neolab_sa" {
  name        = var.service_account_name
  description = "Service Account for NEOLab services"
}

resource "yandex_resourcemanager_folder_iam_member" "neolab_roles" {
  for_each = toset([
    "container-registry.images.puller", # Для скачивания образов контейнеров
    "mdb.viewer",                       # Для подключения к кластеру PostgreSQL (без администрирования)
    "serverless.containers.invoker",    # Для выполнения (запуска) serverless-контейнеров
    "serverless.containers.editor",     # Для создания и обновления контейнеров
    "vpc.publicAdmin",                  # Для NAT и публичного доступа ВМ
    "compute.editor",                   # Для создания/управления ВМ (Prometheus, Grafana)
    "monitoring.editor",                # Для мониторинга (дашборды и метрики)
    "storage.uploader",                 # Для загрузки данных в Object Storage
    "api-gateway.editor"                # Для управления API Gateway
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
