using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Instantly.ClientUtil.Abstract;
using Soenneker.Instantly.Dfy.Abstract;
using Soenneker.Instantly.OpenApiClient;
using Soenneker.Instantly.OpenApiClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Instantly.Dfy;

/// <inheritdoc cref="IInstantlyDfyUtil"/>
public sealed class InstantlyDfyUtil : IInstantlyDfyUtil
{
    private const int _batchSize = 100;

    private readonly IInstantlyOpenApiClientUtil _instantlyOpenApiClientUtil;
    private readonly ILogger<InstantlyDfyUtil> _logger;
    private readonly bool _log;

    public InstantlyDfyUtil(IInstantlyOpenApiClientUtil instantlyOpenApiClientUtil, ILogger<InstantlyDfyUtil> logger,
        IConfiguration config)
    {
        _instantlyOpenApiClientUtil = instantlyOpenApiClientUtil;
        _logger = logger;
        _log = config.GetValue<bool>("Instantly:LogEnabled");
    }

    public async ValueTask<CreateDfyEmailAccountOrder200Response?> Create(CreateDfyEmailAccountOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Creating Instantly DFY email account order. Simulation: {simulation}",
                request.Simulation);

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.PostAsync(request, cancellationToken: cancellationToken)
                           .NoSync();
    }

    public ValueTask<CreateDfyEmailAccountOrder200Response?> Create(
        List<CreateDfyEmailAccountOrderRequestItemsItem> items,
        CreateDfyEmailAccountOrderRequestOrderType orderType = CreateDfyEmailAccountOrderRequestOrderType.Dfy,
        bool simulation = false, CancellationToken cancellationToken = default)
    {
        var request = new CreateDfyEmailAccountOrderRequest
        {
            Items = items,
            OrderType = orderType,
            Simulation = simulation
        };

        return Create(request, cancellationToken);
    }

    public ValueTask<CreateDfyEmailAccountOrder200Response?> Simulate(CreateDfyEmailAccountOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        request.Simulation = true;

        return Create(request, cancellationToken);
    }

    public async ValueTask<ListDfyEmailAccountOrder200Response?> GetList(int? limit = null,
        string? startingAfter = null, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Getting Instantly DFY email account orders...");

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.GetAsync(config =>
        {
            config.QueryParameters.Limit = limit;
            config.QueryParameters.StartingAfter = startingAfter;
        }, cancellationToken).NoSync();
    }

    public async ValueTask<ListDfyEmailAccountOrder200Response> GetAll(string? startingAfter = null,
        CancellationToken cancellationToken = default)
    {
        var result = new ListDfyEmailAccountOrder200Response
        {
            Items = []
        };

        while (true)
        {
            ListDfyEmailAccountOrder200Response? response =
                await GetList(_batchSize, startingAfter, cancellationToken).NoSync();

            if (response?.Items == null || response.Items.Count == 0)
                break;

            result.Items.AddRange(response.Items);
            result.NextStartingAfter = response.NextStartingAfter;

            if (response.Items.Count < _batchSize || string.IsNullOrWhiteSpace(response.NextStartingAfter))
                break;

            startingAfter = response.NextStartingAfter;
        }

        return result;
    }

    public async ValueTask<ListDfyEmailAccountOrdersAccounts200Response?> GetAccountList(int? limit = null,
        string? startingAfter = null, bool? withPasswords = null, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Getting Instantly DFY email accounts...");

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.Accounts.GetAsync(config =>
        {
            config.QueryParameters.Limit = limit;
            config.QueryParameters.StartingAfter = startingAfter;
            config.QueryParameters.WithPasswords = withPasswords;
        }, cancellationToken).NoSync();
    }

    public async ValueTask<ListDfyEmailAccountOrdersAccounts200Response> GetAllAccounts(string? startingAfter = null,
        bool? withPasswords = null, CancellationToken cancellationToken = default)
    {
        var result = new ListDfyEmailAccountOrdersAccounts200Response
        {
            Items = []
        };

        while (true)
        {
            ListDfyEmailAccountOrdersAccounts200Response? response =
                await GetAccountList(_batchSize, startingAfter, withPasswords, cancellationToken).NoSync();

            if (response?.Items == null || response.Items.Count == 0)
                break;

            result.Items.AddRange(response.Items);
            result.NextStartingAfter = response.NextStartingAfter;

            if (response.Items.Count < _batchSize || string.IsNullOrWhiteSpace(response.NextStartingAfter))
                break;

            startingAfter = response.NextStartingAfter;
        }

        return result;
    }

    public async ValueTask<CancelDfyEmailAccounts200Response?> CancelAccounts(CancelDfyEmailAccountsRequest request,
        CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogWarning("Cancelling Instantly DFY email accounts. Count: {count}", request.Accounts?.Count);

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.Accounts.Cancel
                           .PostAsync(request, cancellationToken: cancellationToken).NoSync();
    }

    public ValueTask<CancelDfyEmailAccounts200Response?> CancelAccounts(List<string> accounts,
        CancellationToken cancellationToken = default)
    {
        var request = new CancelDfyEmailAccountsRequest
        {
            Accounts = accounts
        };

        return CancelAccounts(request, cancellationToken);
    }

    public async ValueTask<CheckDomainsAvailability200Response?> CheckDomainsAvailability(
        CheckDomainsAvailabilityRequest request, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Checking Instantly DFY domain availability. Count: {count}", request.Domains?.Count);

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.Domains.Check
                           .PostAsync(request, cancellationToken: cancellationToken).NoSync();
    }

    public ValueTask<CheckDomainsAvailability200Response?> CheckDomainsAvailability(List<string> domains,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckDomainsAvailabilityRequest
        {
            Domains = domains
        };

        return CheckDomainsAvailability(request, cancellationToken);
    }

    public async ValueTask<CheckDomainsAvailability200ResponseResultsItem?> CheckDomainAvailability(string domain,
        CancellationToken cancellationToken = default)
    {
        CheckDomainsAvailability200Response? response =
            await CheckDomainsAvailability([domain], cancellationToken).NoSync();

        return response?.Results?.FirstOrDefault();
    }

    public async ValueTask<GenerateSimilarDomains200Response?> GenerateSimilarDomains(
        GenerateSimilarDomainsRequest request, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Generating Instantly DFY similar domains for {domain}...", request.Domain);

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.Domains.Similar
                           .PostAsync(request, cancellationToken: cancellationToken).NoSync();
    }

    public ValueTask<GenerateSimilarDomains200Response?> GenerateSimilarDomains(string domain,
        List<GenerateSimilarDomainsRequestTldsItem?>? tlds = null, CancellationToken cancellationToken = default)
    {
        var request = new GenerateSimilarDomainsRequest
        {
            Domain = domain,
            Tlds = tlds
        };

        return GenerateSimilarDomains(request, cancellationToken);
    }

    public async ValueTask<PreWarmedUpDomainsList200Response?> GetPreWarmedUpDomains(
        PreWarmedUpDomainsListRequest request, CancellationToken cancellationToken = default)
    {
        if (_log)
            _logger.LogDebug("Getting Instantly DFY pre-warmed up domains...");

        InstantlyOpenApiClient client = await _instantlyOpenApiClientUtil.Get(cancellationToken).NoSync();

        return await client.Api.V2.DfyEmailAccountOrders.Domains.PreWarmedUpList
                           .PostAsync(request, cancellationToken: cancellationToken).NoSync();
    }

    public ValueTask<PreWarmedUpDomainsList200Response?> GetPreWarmedUpDomains(
        CancellationToken cancellationToken = default)
    {
        return GetPreWarmedUpDomains(new PreWarmedUpDomainsListRequest(), cancellationToken);
    }
}