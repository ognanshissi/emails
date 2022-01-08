# Readme - Milochau.Emails.Sdk

## Introduction

Milochau.Emails.Sdk let client application connect to Emails microservice, to send e-mails with an asynchronous (Service Bus) protocol.

## Setup

First install the Milochau.Emails.Sdk as a NuGet package in your application:

```ps
Install-Package Milochau.Emails.Sdk
```

Then you must register the SDK, typically in your `Startup` class:

```csharp
public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Register clients
        services.AddEmailsClients(settings =>
        {
            // Configure settings here
        });

        // More configuration...
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        // More configuration...
    }
}
```

You should configure these settings values - usually retrieved from configuration:

| Key | Description | Default value |
| --- | ----------- | ------------- |
| `ServiceBusNamespace` | Namespace of the Service Bus used by the Emails microservice, to send emails; it should be formatted as `xxx.servicebus.windows.net` | None |
| `StorageAccountUri` | URI of the Storage Account used by the Emails microservice store emails attachments; it should be formatted as `https://xxx.blob.core.windows.net` | None |

Clients need the `Azure Service Bus Data Sender` RBAC role to send messages through Azure Service Bus queues.
Clients need the `Storage Blob Data Contributor` RBAC role to send attachment files through Azure Storage Accounts.

## Usage

You can send an email with this code sample:

```csharp
public class EmailsService : IEmailsService
{
    private readonly IEmailsClient emailsClient;

    public EmailsService(IEmailsClient emailsClient)
    {
        this.emailsClient = emailsClient;
    }

    public async Task SendEmailsAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        var email = MapEmailMessageToEmail(emailMessage); // Convert from your custom 'EmailMessage' class to the 'Email' class expected from the microservice
        await emailsClient.SendEmailAsync(email, cancellationToken);
    }
}
```
