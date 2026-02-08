## How to run
-	Run: docker compose up -d --build

This will run 4 containers : api, worker, redis, postgres.

## Testing the API

- The controller endpoint is `POST /orders`.

```
POST http://localhost:5000/orders
Accept: application/json
Content-Type: application/json

{
  "customerId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "totalAmount": 123.45,
  "items": [
    {
      "itemId": "11111111-1111-1111-1111-111111111111",
      "quantity": 2,
      "amount": 50.00,
      "totalAmount": 100.00
    },
    {
      "itemId": "22222222-2222-2222-2222-222222222222",
      "quantity": 1,
      "amount": 23.45,
      "totalAmount": 23.45
    }
  ]
}
```

## Design Decisions

- Create endpoint applies some basic validation, stores the order and publishes order created event.
- The worker is a separate console app.
- I chose redis because its easier to setup and it works well for simple queue functionality.