using LinKit.Core.Abstractions;
using LinqToDB;
using O24OpenAPI.Client.Lib.Encryption;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Data;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.WFO.Domain.AggregateModels.ServiceAggregate;
using O24OpenAPI.WFO.Infrastructure.Configurations;
using O24OpenAPI.WFO.Infrastructure.Services;
using System.Net;
using System.Text.RegularExpressions;

namespace O24OpenAPI.WFO.Infrastructure.Repositories;

[RegisterService(Lifetime.Scoped)]
public class ServiceInstanceRepository(
    IO24OpenAPIDataProvider dataProvider,
    IStaticCacheManager staticCacheManager,
    WFOSetting wfoSetting
) : EntityRepository<ServiceInstance>(dataProvider, staticCacheManager), IServiceInstanceRepository
{
    public async Task<ServiceInfo> QueryServiceInfo(string pServiceCode, string pInstanceID)
    {
        ServiceInfo serviceInfo = new();
        if (!string.IsNullOrWhiteSpace(pServiceCode))
        {
            ServiceInstance serviceInstance =
                await GetByServiceCodeAsync(pServiceCode)
                ?? throw new Exception("Service code [" + pServiceCode + "] does not exist.");

            serviceInfo.service_code = serviceInstance.ServiceCode;
            serviceInfo.service_name = serviceInstance.ServiceCode;
            serviceInfo.service_grpc_url = serviceInstance.GrpcUrl;
            serviceInfo.service_grpc_timeout_seconds =
                serviceInstance.GrpcTimeout <= 0 ? 60L : serviceInstance.GrpcTimeout;

            serviceInfo.broker_queue_name = serviceInstance.CommandQueueName;
            serviceInfo.event_queue_name = serviceInstance.EventQueueName;
            serviceInfo.concurrent_threads = serviceInstance.ConcurrentLimit;
        }
        await BuildServiceInfo(serviceInfo);

        return serviceInfo;
    }

    private async Task BuildServiceInfo(ServiceInfo serviceInfo)
    {
        ServiceInfoConfiguration wfoServiceInfo =
            Singleton<AppSettings>.Instance.Get<ServiceInfoConfiguration>();
        serviceInfo.broker_hostname = wfoServiceInfo.BrokerHostname;
        serviceInfo.broker_port = wfoServiceInfo.BrokerPort;
        serviceInfo.broker_user_name = wfoServiceInfo.BrokerUsername;
        serviceInfo.broker_user_password = wfoServiceInfo.BrokerPassword;
        if (wfoSetting.MessageBrokerPasswordEncryptionMethod?.StartsWithOrdinalIgnoreCase("AES") == true)
        {
            serviceInfo.broker_user_password = O24OpenAPITextEncryptor.AESDecryptString(
                serviceInfo.broker_user_password
            );
        }
        serviceInfo.broker_virtual_host = wfoServiceInfo.BrokerVirtualHost.Coalesce("/");
        serviceInfo.o24openapi_server_time = Common.GetCurrentDateAsLongNumber();
        serviceInfo.service_ping_interval_seconds = wfoSetting.ServerPingIntervalInSecond;
        serviceInfo.o24openapi_grpc_url = Singleton<AppSettings>
            .Instance.Get<O24OpenAPIConfiguration>()
            .YourGrpcURL;
        serviceInfo.o24openapi_grpc_timeout_seconds = wfoSetting.GrpcTimeoutInSeconds;
        serviceInfo.workflow_execution_timeout_seconds = wfoSetting.WorkflowTimeoutInSeconds;
        serviceInfo.ssl_active = wfoSetting.MessageBrokerSSLActive.ToString();
        serviceInfo.broker_reconnect_interval_in_seconds =
            wfoSetting.MessageBrokerReconnectIntervalInSecond;
        serviceInfo.redis_server_name = wfoSetting.RedisServerName;
        serviceInfo.redis_server_port = wfoSetting.RedisServerPort;
    }

    public async Task<ServiceInfo> GetServiceInstanceByServiceHandleName(string serviceHandleName)
    {
        ServiceInstance serviceInstance =
            await GetByServiceHanleNameAsync(serviceHandleName)
            ?? throw new Exception("Service for [" + serviceHandleName + "] does not exist.");
        ServiceInfo serviceInfo = new()
        {
            service_code = serviceInstance.ServiceCode,
            service_name = serviceInstance.ServiceCode,
            service_grpc_url = serviceInstance.GrpcUrl,
            service_grpc_timeout_seconds =
                serviceInstance.GrpcTimeout <= 0 ? 60L : serviceInstance.GrpcTimeout,

            broker_queue_name = serviceInstance.CommandQueueName,
            event_queue_name = serviceInstance.EventQueueName,
            concurrent_threads = serviceInstance.ConcurrentLimit,
        };
        await BuildServiceInfo(serviceInfo);
        return serviceInfo;
    }

    public async Task<ServiceInstance?> GetByServiceCodeAsync(string serviceCode)
    {
        try
        {
            IQueryable<ServiceInstance> query = Table.Where(x => x.ServiceCode == serviceCode);

            string? currentHost = GetHostFromGrpcUrl(
                Singleton<O24OpenAPIConfiguration>.Instance.YourGrpcURL
            );
            Console.WriteLine($"[DEBUG] currentHost = {currentHost}");

            List<ServiceInstance> allMatches = await query.ToListAsync();

            if (string.IsNullOrEmpty(currentHost))
            {
                Console.WriteLine("[WARN] Cannot detect currentHost from YourGrpcURL");
                return allMatches.FirstOrDefault();
            }

            ServiceInstance? exactMatch = allMatches.FirstOrDefault(x =>
            {
                string? host = GetHostFromGrpcUrl(x.GrpcUrl);
                return string.Equals(host, currentHost, StringComparison.OrdinalIgnoreCase);
            });

            if (exactMatch == null)
            {
                Console.WriteLine(
                    $"[WARN] No exact match for host = {currentHost}, returning first available instance"
                );
            }

            return exactMatch ?? allMatches.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    public async Task<ServiceInstance?> GetByServiceHanleNameAsync(string serviceHandleName)
    {
        try
        {
            IQueryable<ServiceInstance> query = Table.Where(x => x.ServiceHandleName == serviceHandleName);

            string? currentHost = GetHostFromGrpcUrl(
                Singleton<O24OpenAPIConfiguration>.Instance.YourGrpcURL
            );
            Console.WriteLine($"[DEBUG] currentHost = {currentHost}");

            List<ServiceInstance> allMatches = await query.ToListAsync();

            if (string.IsNullOrEmpty(currentHost))
            {
                Console.WriteLine("[WARN] Cannot detect currentHost from YourGrpcURL");
                return allMatches.FirstOrDefault();
            }

            ServiceInstance? exactMatch = allMatches.FirstOrDefault(x =>
            {
                string? host = GetHostFromGrpcUrl(x.GrpcUrl);
                return string.Equals(host, currentHost, StringComparison.OrdinalIgnoreCase);
            });

            if (exactMatch == null)
            {
                Console.WriteLine(
                    $"[WARN] No exact match for host = {currentHost}, returning first available instance"
                );
            }

            return exactMatch ?? allMatches.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            throw;
        }
    }

    private static string? GetHostFromGrpcUrl(string? grpcUrl)
    {
        if (string.IsNullOrEmpty(grpcUrl))
        {
            return null;
        }
        Console.WriteLine($"[INFO] grpcUrl instance = {grpcUrl}");
        try
        {
            Uri uri = new(grpcUrl);
            string host = uri.Host;

            if (IPAddress.TryParse(host, out _))
            {
                return host; // IP
            }

            if (host.All(c => char.IsLetterOrDigit(c) || c == '-'))
            {
                return host; // Docker name
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    static string GetIpFromUrl(string url)
    {
        Match match = Regex.Match(url, @"https?:\/\/([\d\.]+)");
        return match.Success ? match.Groups[1].Value : url;
    }
}
