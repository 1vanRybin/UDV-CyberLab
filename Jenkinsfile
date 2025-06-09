pipeline {
    agent any

    environment {
        YANDEX_BUCKET_NAME = 'udv-bucket'
        YANDEX_SERVICE_URL = 'https://storage.yandexcloud.net'
        YANDEX_ACCESS_KEY = credentials('YANDEX_ACCESS_KEY')
        YANDEX_SECRET_KEY = credentials('YANDEX_SECRET_KEY')
    }

    stages {
        stage('Clean Docker') {
            steps {
                sh '''
                    docker rm -f udv-cyberlab-tests-1 udv-cyberlab-identity-1 udv-cyberlab-projects-1
                    docker system prune -af --volumes
                '''
            }
        }

        stage('Checkout') {
            steps {
                sh '''
                cd /UDV-CyberLab
                    git pull
                    cat docker-compose.yml
                '''
            }
        }

        stage('Build and Deploy') {
                    environment {
                        YC_BUCKET = "${YANDEX_BUCKET_NAME}"
                        YC_SERVICE_URL = "${YANDEX_SERVICE_URL}"
                        YC_ACCESS_KEY = "${YANDEX_ACCESS_KEY}"
                        YC_SECRET_KEY = "${YANDEX_SECRET_KEY}"
                    }
            steps {
                sh '''
                cd /UDV-CyberLab
                    docker-compose up -d --build
                '''
            }
        }
    }
}
