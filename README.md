# PaymentGateway

## Usage
Run locally in debug

SetPayment:
```
POST http://localhost:7071/api/Payment
{
	"cardNumber": 1234567891234567,
	"expiryYear": 2020,
	"expiryMonth": 10,
	"amount": 19.99,
	"currencyCode": "GBP",
	"CVV": 123
}
```

GetPayment:
```
GET http://localhost:7071/api/Payment/{id}
```

The GetPayment method doesn't work when published as this utilizes an in-memory dictionary to store payments and this isn't persisted bewteen functions, in reality this would be stored in a database by the acquiring bank and their API would be called from the AcquiringBankClient.

## Considerations
This was done as an Azure Function rather than a Web Application because it should scale much better and the design guidlines didn't include anything that couldn't be implemented easily (such as OAuth authentication). Obvious improvements include adding some validation of the payment data provided as well as persisting the payment details to storage.

## Additionals:
* Application logging: Serilog was used for logging, the only sink used was to the console but this should be persisted to somewhere like table storage.
* Application metrics: Application Insights was added to the published function (see performance testing).
* Containerization: Azure Functions are serverless micro-services so don't really need to be hosted in a container in order to scale but they can be if required.
* Authentication: Access to the function is authenticated by a key provided in the URL (see the load testing code), in real life we'd probably want something a bit easier to manage as scale such as OAuth.
* API client: I didn't provide one but would be easy enough to create.
* Build script/CI: Easy enough to do in Azure DevOps but I haven't done so as it's not possible to grant public access (and would a bad idea anyway).
* Performance testing: SetPayment method was load tested with a simple console application, the function happily coped with around 150 requests/s.
![performance metrics](https://blog.bitscry.com/wp-content/uploads/2019/08/payment-gateway-metrics.png)
* Encryption: Requests are transmitted over HTTPS so should be secure, any data stored by acquiring bank and payment processor should be encrypted.
* Data storage: I didn't worry about data storage for this, just using an in-memory dictionary as an example but it would be easy enough to add in if required.
* Extra: A queuing system such as Event Hub would be a good idea to ensure messages are processed in the correct order, easy enough to implement but not possible to share access.
