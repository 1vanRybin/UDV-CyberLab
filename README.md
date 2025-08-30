# 🚀 Быстрое развёртывание UDV-CyberLab

## 📋 Шаги развёртывания

### 1. Установка Jenkins
```bash
# Обновляем систему
sudo apt update && sudo apt upgrade -y

# Устанавливаем Java и Jenkins
sudo apt install -y openjdk-11-jdk
curl -fsSL https://pkg.jenkins.io/debian-stable/jenkins.io-2023.key | sudo tee /usr/share/keyrings/jenkins-keyring.asc > /dev/null
echo deb [signed-by=/usr/share/keyrings/jenkins-keyring.asc] https://pkg.jenkins.io/debian-stable binary/ | sudo tee /etc/apt/sources.list.d/jenkins.list > /dev/null
sudo apt update
sudo apt install -y jenkins
echo "jenkins ALL=(ALL) NOPASSWD: ALL" | sudo tee /etc/sudoers.d/jenkins

# Запускаем Jenkins
sudo systemctl start jenkins
sudo systemctl enable jenkins
```

### 2. Клонирование проекта
```bash
git clone https://github.com/1vanRybin/UDV-CyberLab.git
cd UDV-CyberLab
```

### 3. Настройка Jenkins
```bash
# Получаем пароль администратора
sudo cat /var/lib/jenkins/secrets/initialAdminPassword

# Открываем http://YOUR_SERVER_IP:8080
# Устанавливаем рекомендуемые плагины
# Создаём администратора
```

### 4. Создание Pipeline Job
1. Создаём новый Pipeline job
2. В "Pipeline" выбираем "Pipeline script from SCM"
3. Выбираем Git и указываем URL репозитория, ветка */main
4. В "Script Path" указываем `Jenkinsfile`

### 5. Добавление Credentials
В Jenkins → Manage Jenkins → Credentials → System → Global credentials → Secret text:

**Добавляем:**
- `YANDEX_ACCESS_KEY` (Secret text) - ключ доступа к Yandex Cloud
- `YANDEX_SECRET_KEY` (Secret text) - секретный ключ Yandex Cloud
- `DOMAIN` (Secret text) - ваш домен (например: neolab-backend.aydlioh.ru)
- `SSL_EMAIL` (Secret text) - email для SSL сертификата

### 6. Запуск Pipeline
Нажимаем "Build Now" в Jenkins!

* Техническое задание [тут](https://docs.google.com/document/d/1msiwzmNE4nWXhuL47TzWZ8p1dBNNvBnC/edit?usp=sharing&ouid=109486232149773167278&rtpof=true&sd=true)

* Прототип в [Figma](https://www.figma.com/file/K7xsEAO4at35LtiuS9M4g9/Untitled?type=design&node-id=0-1&mode=design&t=tjH81ceAnPX03sMa-0)