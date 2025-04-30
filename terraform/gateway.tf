resource "yandex_api_gateway" "api_gateway" {
  name        = "services-gateway"
  description = "API Gateway for serverless containers"
  execution_timeout = 10

  spec = <<-EOT
    openapi: "3.0.0"
    info:
      title: "Microservices Gateway"
      version: "1.0"
    paths:
      /api/auth/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.identity.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/user/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.identity.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/Questions/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.tests.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/Test/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.tests.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/TestPassing/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.tests.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/Files/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.projects.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"

      /api/ProjectCard/{path+}:
          x-yc-apigateway-any-method:
            parameters:
            - name: path
              in: path
              required: false

            x-yc-apigateway-integration:
                type: "serverless_containers"
                container_id: "${yandex_serverless_container.projects.id}"
                service_account_id: "${yandex_iam_service_account.neolab_sa.id}"
  EOT
}