[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- SDK now uses Managed Identity to connect to Azure Service Bus. Clients should provide a `ServiceBusNamespace`, instead of `ServiceBusConnectionString`
- Emails service now uses a fixed Service Bus queue name: `emails` 
