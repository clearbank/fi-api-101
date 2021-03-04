# ClearBank® API 101

Sample .NET 5 application to call ClearBank API and receive webhooks

## Overview

Application has one controller with 2 endpoints:

- `POST /sample/api` to make API request to `POST /v1/test` endpoint in ClearBank API
- `POST /sample/webhook` to receive webhooks from ClearBank

It does payload signing for API calls and signature verification for webhooks

### Prerequisites

To be able to run application and receive webhooks you will need:

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [ngrok](https://ngrok.com/) Version 2.3.35 was used at the moment of writing

## Set up on ClearBank portal

Upload your CSR and get auth token, download public key from webhooks management page. Copy auth token, your private key and clearbank public key into the SampleController constructor.

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
    "body" : "test"
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

Copy that url and go to webhooks management page, find test webhook and set URL to `https://91bf871fa2aa.ngrok.io/sample/webhook`
Click test button and you should receive a webhook into the application and see that in console:

```cmd
Received webhook FITestEvent
```
