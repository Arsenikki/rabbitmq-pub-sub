# rabbitmq-pub-sub
Trying out rabbitMQ with long running tasks (defaults to 5s), which are processed one by one. This means no requests are cached in the clients and ACKs are sent only after the processing has completed. This allows for scaling to be implemented based on queue length with for example KEDA (TBD).


## Try with Docker

Use docker-compose to spin up rabbitmq + publish API + multiple workers:
```
docker-compose up
```

Access Swagger at `http://localhost/swagger` to post messages to queue.
Access Rabbitmq management UI at `http://localhost:15672/`
