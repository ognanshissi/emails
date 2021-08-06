[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- Attachments are now sended via a unique Azure Storage Account
- Attachments Storage Account now uses MI/RBAC instead of connection strings
- The new method `StoreAttachments` is now exposed from the `IEmailsClient`

### Migration

- `StorageAttachment` has been renamed `EmailAttachment`
- `EmailsServiceBusClient` is now internal into SDK ; you can freely use the `IEmailsClient` as before

## New features

- A new `IAttachmentsClient` is now exposed from the SDK, to help you upload documents as attachments
