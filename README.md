[![](https://img.shields.io/nuget/v/soenneker.instantly.dfy.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.instantly.dfy/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.instantly.dfy/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.instantly.dfy/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.instantly.dfy.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.instantly.dfy/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.instantly.dfy/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.instantly.dfy/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Instantly.Dfy

Place or simulate Instantly DFY email-account orders, inspect ordered accounts, cancel accounts, and discover available domains.

## Install

```bash
dotnet add package Soenneker.Instantly.Dfy
```

## Configure and register

```json
{
  "Instantly": {
    "ApiKey": "<API key>",
    "LogEnabled": false
  }
}
```

```csharp
using Soenneker.Instantly.Dfy.Registrars;

services.AddInstantlyDfyUtilAsScoped();
```

The scoped DFY service deliberately uses the singleton generated-client provider. Use `AddInstantlyDfyUtilAsSingleton()` when the operation layer should also live for the application lifetime.

## Simulate before ordering

```csharp
using Soenneker.Instantly.Dfy.Abstract;
using Soenneker.Instantly.OpenApiClient.Models;

var request = new CreateDfyEmailAccountOrderRequest
{
    Items = orderItems,
    OrderType = CreateDfyEmailAccountOrderRequestOrderType.Dfy
};

CreateDfyEmailAccountOrder200Response? quote = await dfy.Simulate(
    request,
    cancellationToken);
```

`Simulate` sets `request.Simulation` to `true` and sends the request without placing the order or charging the card. It mutates the supplied request object.

## Place an order

```csharp
CreateDfyEmailAccountOrder200Response? order = await dfy.Create(
    orderItems,
    orderType: CreateDfyEmailAccountOrderRequestOrderType.Dfy,
    simulation: false,
    cancellationToken: cancellationToken);
```

`Create` with `simulation: false` places the order and may charge the Instantly account. Use `Simulate` or set `simulation: true` when you only want validation and pricing.

## List orders and accounts

```csharp
ListDfyEmailAccountOrder200Response orders = await dfy.GetAll(
    cancellationToken: cancellationToken);

ListDfyEmailAccountOrdersAccounts200Response accounts =
    await dfy.GetAllAccounts(
        withPasswords: false,
        cancellationToken: cancellationToken);
```

`GetAll` and `GetAllAccounts` request batches of 100 and follow `next_starting_after`. Pass a cursor to resume. If Instantly repeats a cursor, the methods throw instead of looping indefinitely.

Set `withPasswords: true` only when the caller needs credentials, and avoid logging or retaining the returned passwords unnecessarily.

## Domains and cancellation

```csharp
CheckDomainsAvailability200Response? availability =
    await dfy.CheckDomainsAvailability(domains, cancellationToken);

GenerateSimilarDomains200Response? alternatives =
    await dfy.GenerateSimilarDomains(domain, cancellationToken: cancellationToken);

PreWarmedUpDomainsList200Response? preWarmed =
    await dfy.GetPreWarmedUpDomains(cancellationToken);

CancelDfyEmailAccounts200Response? cancelled =
    await dfy.CancelAccounts(accountsToCancel, cancellationToken);
```

Cancellation is destructive for the selected ordered accounts. API and deserialization failures are surfaced to the caller; nullable results indicate that Instantly returned no response body or no matching item.
