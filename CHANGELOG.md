# Changelog

## Introduction

This file lists all the public features of `Milochau.Emails.Sdk` library.

## Version 4.0.0

Breaking changes:

- Rename SDK to `Milochau.Emails.Sdk`

## Version 3.0.0

New features:

- Email validation is performed from the SDK, before sending the email

Breaking changes:

- HTTP protocol is not supported anymore to send emails; please use service bus instead
- `IEmailsClientFactory` does not exist anymore; please use `IEmailsClient` from DI
- `HttpEndpoint`, `HttpAccessKey`, `HttpRetrySleepDurationInMilliseconds`, `HttpRetryCount` have been removed from settings

## Version 2.0.0

Breaking changes:

- Health checks are automatically included when registring microservice
- Now references dependencies for .NET Core 3.1+

New features:

- SDK now uses `System.Text.Json` instead of `Newtonsoft.Json` to serialize JSON messages

## Version 1.3.1

Bug fixes:

- Add proper `AssemblyVersion` in DLLs

## Version 1.3.0

New features:

- Support `Importance`, `Priority` and `TemplateId` as email metadata

## Version 1.2.4

Bug fixes:

- Fix `IEmailsClientFactory` registration in SDK (could cause bad registration errors in some cases)

## Version 1.2.0

New features:

- Support asynchronous protocol via Service Bus, use `ClientType.Asynchronous` from SDK to use it
- Add a retry policy for synchronous emails send, in the SDK. See `HttpRetryCount` and `HttpRetrySleepDurationInMilliseconds` settings in the documentation
- Add XML documentation
- Support models, like `Email`, `EmailAddress`, `StorageAccount` and `StorageAttachment`, used with the HTTP endpoint
- Propose an implementation of an `IEmailsClientFactory`, where you can choose whether you want to use synchronous or asynchronous protocol
- Propose extension methods for clients & health checks registration
