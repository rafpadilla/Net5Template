### Build docker image with latest tag
docker build -t net5template:latest .

### run docker image with tag and expose port 80
docker run -p 8080:80 -p 44300:443 -d --name net5template net5template:latest

test (error because DB):

```
http://localhost:8080/api/v1/Logs
```

### kill container
docker rm --force net5template


### for use specific .env file
docker-compose --env-file ./.env.dev up -d

### for recreating image and run (recreating tag and image)
docker-compose build
docker-compose up -d --build



```BASH
docker-compose -f rabbitmq.yml up -d
```

Execute in same directory

http://localhost:15672/

admin@password -> default credentials for docker image