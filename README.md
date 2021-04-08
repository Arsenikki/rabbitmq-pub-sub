# rabbitmq-pub-sub
Trying out rabbitMQ with long running tasks (defaults to 5s), which are processed one by one. This means no requests are cached in the clients and ACKs are sent only after the processing has completed. This allows for scaling to be implemented based on queue length with for example KEDA (TBD).


## Try with Docker

1. Use docker-compose to spin up rabbitmq + publish API + multiple workers:
    ```
    docker-compose up
    ```
2. Access Swagger at `http://localhost/swagger` to post messages to queue.

3. Access Rabbitmq management UI at `http://localhost:15672/`

## Try with k3d (Kubernetes-in-Docker)

1. Install k3d from https://k3d.io/
2. Create cluster with 2 nodes, and map cluster port 80 to localhost 8081:
    ```
    k3d cluster create --api-port 6550 -p "8081:80@loadbalancer" --agents 2
    ```
3. Install keda to the cluster:
    ```
    kubectl apply -f https://github.com/kedacore/keda/releases/download/v2.2.0/keda-2.2.0.yaml
    ```
4. Apply all deployment manifests to configure rabbitmq + publish api + subscribe module + keda ScaledObject for autoscaling:
    ```
    cd deployment
    ```
    ```
    kubectl apply -f .
    ```
5. As the port forwarding is configured in the step 2, we can access the swagger endpoint at `localhost:8081/swagger`
6. Send some messages to the queue through Swagger
7. Keda should automatically scale the Subscribe module instances based on the queue length. Check this out with: 
    ```
    kubectl get pods
    ```