# Readme - emails

[![Build](https://github.com/amilochau/emails/actions/workflows/build.yml/badge.svg)](https://github.com/amilochau/emails/actions/workflows/build.yml)
[![Deploy libraries](https://github.com/amilochau/emails/actions/workflows/deploy-libraries.yml/badge.svg)](https://github.com/amilochau/emails/actions/workflows/deploy-libraries.yml)

## Introduction

`emails` let client applications send e-mails, thanks to Azure Service Bus and SendGrid.

## Getting Started

1. Installation process
From your local computer, clone the repository.

- dotnet restore
- dotnet run

2. Integration process
Please follow the development good practices, then follow the integration process.

## Configuration needed

These settings need to be defined; default value for local development are fetched from configuration providers:

- **`SendGrid:Key`**
- `Emails:AuthorizedRecipientHosts`
- `Emails:StorageAccountUri` (needs `Storage Blob Data Reader` RBAC)

These settings can not be defined from Azure App Configuration / Azure Key Vault, and should be defined in `local.settings.json` **and** in hosting configuration as environment variables:

- **`ASPNETCORE_ORGANIZATION`**
- **`ASPNETCORE_APPLICATION`**
- **`ASPNETCORE_ENVIRONMENT`**
- **`ASPNETCORE_HOST`**
- **`ASPNETCORE_KEYVAULT_VAULT`**
- **`ASPNETCORE_APPCONFIG_ENDPOINT`**
- **`ServiceBusConnectionString`**
