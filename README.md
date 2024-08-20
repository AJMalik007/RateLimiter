# Rate Limiting and Caching Microservice

This microservice is designed to handle rate-limiting for API requests and implements caching for frequently accessed data. Built using ASP.NET Core 8, Docker, and Redis, it is a simple yet scalable solution suitable for managing API request loads efficiently.

## Features

- **Basic API Endpoint:**
  - Exposes a single endpoint **/api/data** that fetches data from a mock external API.
- **Rate-Limiting:**
  - **Global limit:** 100 requests per minute across all clients.
  - **Client-specific limit:** 10 requests per minute per client, identified by user ID or API key.
  - **Returns** HTTP 429 (Too Many Requests) with a Retry-After header when limits are exceeded.
- **Caching:**
  - Caches responses from the external API for 30 seconds using Redis.
  - Implements cache invalidation to ensure data freshness.
- **Scalability:**
  - Uses Redis to store rate-limiting and cache data, supporting horizontal scaling.

## Technologies Used

- ASP.NET Core 8
- Redis
- Docker & Docker Compose
- C#

## Prerequisites

- .NET 8 SDK
- Docker Desktop

## Getting Started

- Clone the repository:

  ```console
  git clone https://github.com/AJMalik007/RateLimiter.git
  cd RateLimiter
  ```

- Build and run the application using Docker Compose:

  ```console
  docker-compose up --build
  ```

  This command will start the ASP.NET Core service and Redis in separate containers.

## API Endpoints

**/api/data**

This endpoint simulates fetching real-time data (e.g., network connectivity status) from a mock external API. (5 seconds delay)

```console
    curl --location 'https://localhost:5001/api/data' \
    --header 'X-API-KEY: c918c339-d768-4a9a-8f54-13b47f00373a' \
    --data ''
```

### Example Response

```json
[
  {
    "id": 1,
    "firstName": "Helene",
    "lastName": "Anderson",
    "email": "Noel_Hills87@hotmail.com",
    "phoneNumber": "780-798-7100"
  },
  {
    "id": 2,
    "firstName": "Adell",
    "lastName": "Cronin",
    "email": "Monte.Schroeder@yahoo.com",
    "phoneNumber": "372-401-0033 x689"
  },
  {
    "id": 3,
    "firstName": "Nola",
    "lastName": "Hauck",
    "email": "Dante20@gmail.com",
    "phoneNumber": "1-446-957-2484"
  },
  {
    "id": 4,
    "firstName": "Jared",
    "lastName": "Crona",
    "email": "Kory_Davis98@hotmail.com",
    "phoneNumber": "225-522-7243 x177"
  },
  {
    "id": 5,
    "firstName": "Caleb",
    "lastName": "Lang",
    "email": "Josue.Ziemann13@yahoo.com",
    "phoneNumber": "(268) 893-3056"
  },
  {
    "id": 6,
    "firstName": "Idell",
    "lastName": "Ebert",
    "email": "Daija63@yahoo.com",
    "phoneNumber": "840-565-8101"
  },
  {
    "id": 7,
    "firstName": "Monserrat",
    "lastName": "Kunze",
    "email": "Maryam.Tillman@hotmail.com",
    "phoneNumber": "981-862-2931 x4157"
  },
  {
    "id": 8,
    "firstName": "Jenifer",
    "lastName": "Stokes",
    "email": "Jimmie_Hermiston@yahoo.com",
    "phoneNumber": "934-940-5057 x1599"
  },
  {
    "id": 9,
    "firstName": "Freddie",
    "lastName": "Kirlin",
    "email": "Ansel.Effertz35@hotmail.com",
    "phoneNumber": "677-562-7709 x56315"
  },
  {
    "id": 10,
    "firstName": "Caterina",
    "lastName": "Hyatt",
    "email": "Zachariah.Kuvalis0@gmail.com",
    "phoneNumber": "878-790-6310 x549"
  }
]
```

## Rate-Limiting Implementation

The service implements both global and client-specific rate-limiting:

- **Global Limit:** Enforced across all clients, capped at 100 requests per minute.
- **Client-Specific Limit:** Enforced per client (identified by an API key), capped at 10 requests per minute.
- **Exceeded Limits:** If a client exceeds the rate limit, the service returns a HTTP 429 status with a Retry-After header indicating when to retry.

## Caching with Redis

- **Cache Duration:** 30 seconds.
- **Cache Invalidation:** Ensures data remains up-to-date while reducing the load on the external API.

## Configuring Rate-Limiting and Caching Policies

You can adjust the rate-limiting and caching policies using environment variables in the **docker-compose.yml** file. The following environment variables are available for customization:

```yaml
environment:
  - RedisConnection=redis:6379
  - GlobalRequestLimit=100
  - UserRequestLimit=10
  - RateLimitExpiryTime=60
  - GlobalClient=global
  - CacheExpiryTime=30
```

## Environment Variable Descriptions

- **RedisConnection:** Connection string for the Redis server.
- **GlobalRequestLimit:** Maximum number of requests per minute allowed globally across all clients.
- **UserRequestLimit:** Maximum number of requests per minute allowed per client.
- **RateLimitExpiryTime:** Time in seconds for rate-limiting data to expire in Redis.
- **GlobalClient:** Identifier used for global rate-limiting in Redis.
- **CacheExpiryTime**: Time in seconds for cached data to expire in Redis.

To modify these values, update the environment variables in **docker-compose.yml** and restart the service.

## Scalability Considerations

- **Redis Storage:** Both rate-limiting data and cache entries are stored in Redis, making the service ready for horizontal scaling.
- **Containerization:** The use of Docker ensures that the service can be easily deployed and scaled.

# Documentation

## Architecture Overview

The microservice is structured around the following components:

- **RateLimitingMiddleware**: Handles rate-limiting logic.
- **RedisCacheService**: Manages caching operations using Redis.
- **ExternalService**: Simulates calls to an external API.

## Key Design Decisions

- **Middleware Pattern:** Rate-limiting is implemented as middleware, allowing it to intercept requests before they reach the controller.
- **Distributed Caching:** Redis is used to provide distributed caching, ensuring that the service can scale out across multiple instances.

## How to Extend

- **Adding More Endpoints**: Implement new controllers and register them in the Program.cs.
- **Custom Rate-Limiting Rules**: Modify the **_RateLimitingMiddleware.cs_** to introduce new rate-limiting policies.

# Conclusion

This project provides a foundation for building more complex microservices with rate-limiting and caching capabilities. The use of Redis ensures that the service can handle high loads efficiently, while Docker Compose simplifies deployment.
