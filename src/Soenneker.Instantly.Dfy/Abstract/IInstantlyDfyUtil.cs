namespace Soenneker.Instantly.Dfy.Abstract;

using Soenneker.Instantly.OpenApiClient.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// A .NET typesafe implementation of Instantly.ai's DFY API
/// </summary>
public interface IInstantlyDfyUtil
{
    /// <summary>
    /// Places a DFY email account order.
    /// </summary>
    ValueTask<CreateDfyEmailAccountOrder200Response?> Create(CreateDfyEmailAccountOrderRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Places a DFY email account order from order items.
    /// </summary>
    ValueTask<CreateDfyEmailAccountOrder200Response?> Create(List<CreateDfyEmailAccountOrderRequestItemsItem> items,
        CreateDfyEmailAccountOrderRequestOrderType orderType = CreateDfyEmailAccountOrderRequestOrderType.Dfy, bool simulation = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Runs a DFY email account order simulation without placing the order or charging the card.
    /// </summary>
    ValueTask<CreateDfyEmailAccountOrder200Response?> Simulate(CreateDfyEmailAccountOrderRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a page of DFY email account orders.
    /// </summary>
    ValueTask<ListDfyEmailAccountOrder200Response?> GetList(int? limit = null, string? startingAfter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all DFY email account orders using pagination.
    /// </summary>
    ValueTask<ListDfyEmailAccountOrder200Response> GetAll(string? startingAfter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a page of ordered DFY email accounts.
    /// </summary>
    ValueTask<ListDfyEmailAccountOrdersAccounts200Response?> GetAccountList(int? limit = null, string? startingAfter = null, bool? withPasswords = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all ordered DFY email accounts using pagination.
    /// </summary>
    ValueTask<ListDfyEmailAccountOrdersAccounts200Response> GetAllAccounts(string? startingAfter = null, bool? withPasswords = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels ordered DFY email accounts.
    /// </summary>
    ValueTask<CancelDfyEmailAccounts200Response?> CancelAccounts(CancelDfyEmailAccountsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels ordered DFY email accounts by email address.
    /// </summary>
    ValueTask<CancelDfyEmailAccounts200Response?> CancelAccounts(List<string> accounts, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks domain availability for DFY ordering.
    /// </summary>
    ValueTask<CheckDomainsAvailability200Response?> CheckDomainsAvailability(CheckDomainsAvailabilityRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks domain availability for DFY ordering.
    /// </summary>
    ValueTask<CheckDomainsAvailability200Response?> CheckDomainsAvailability(List<string> domains, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks a single domain's availability for DFY ordering.
    /// </summary>
    ValueTask<CheckDomainsAvailability200ResponseResultsItem?> CheckDomainAvailability(string domain, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates similar available domains for DFY ordering.
    /// </summary>
    ValueTask<GenerateSimilarDomains200Response?> GenerateSimilarDomains(GenerateSimilarDomainsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Generates similar available domains for DFY ordering.
    /// </summary>
    ValueTask<GenerateSimilarDomains200Response?> GenerateSimilarDomains(string domain, List<GenerateSimilarDomainsRequestTldsItem?>? tlds = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available pre-warmed up domains.
    /// </summary>
    ValueTask<PreWarmedUpDomainsList200Response?> GetPreWarmedUpDomains(PreWarmedUpDomainsListRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets available pre-warmed up domains.
    /// </summary>
    ValueTask<PreWarmedUpDomainsList200Response?> GetPreWarmedUpDomains(CancellationToken cancellationToken = default);
}
