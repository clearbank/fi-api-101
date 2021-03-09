# ClearBank® API 101

Sample .NET 5 application to call ClearBank API and receive webhooks.

It is not production code, it was simplified for brevity.

## Overview

The application has one controller with 2 endpoints:

- `POST /sample/api` to make API request to `POST /v1/test` endpoint in ClearBank API
- `POST /sample/webhook` to receive webhooks from ClearBank

It does payload signing for API calls and signature verification for webhooks

### Prerequisites

To be able to run application and receive webhooks you will need:

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [ngrok](https://ngrok.com/) Version 2.3.35 was used at the moment of writing

## Set up on ClearBank portal

You will need:

- Your private key and CSR created
- API token, upload your CSR to `Certificates and Tokens` page to get token
- Public key, download from `Webhook Management` page

Copy your public key, auth token and ClearBank public key into the SampleController constructor.

## Build and run

```cmd
dotnet build
dotnet run
```

Application starts listening on port 5000.
You can test it like so:

```cmd
curl --location --request POST 'http://localhost:5000/sample/api' \
--header 'Content-Type: application/json' \
--data-raw '{
    "fieldName" : "test"
}'
```

Now set up ngrok

```cmd
.\ngrok.exe http 5000
```

You will get output similar to

```cmd
Forwarding https://91bf871fa2aa.ngrok.io -> http://localhost:5000 
```

Copy that url and add `/sample/webhook` to the end so full url will be `https://91bf871fa2aa.ngrok.io/sample/webhook`.
On webhooks management page, find test webhook, enable it and set URL.
Click test button and you should receive a webhook into the application and see that in console:

```cmd
Received webhook FITestEvent
```
