using System.Net;
using System.Text.RegularExpressions;
using LinqToDB;
using O24OpenAPI.Contracts.Configuration.Client;
using O24OpenAPI.Core.Configuration;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient.Lib.Encryption;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.WFO.Domain;
using O24OpenAPI.WFO.Infrastructure;
using O24OpenAPI.WFO.Lib;
using O24OpenAPI.WFO.Services.Interfaces;

namespace O24OpenAPI.WFO.Services;

public class ServiceInstanceService(
    IRepository<ServiceInstance> serviceInstanceRepository,
    WFOSetting wfoSetting,
    IWorkflowService workflowService
) : IServiceInstanceService
{
    private readonly IRepository<ServiceInstance> _serviceInstanceRepository =
        serviceInstanceRepository;
    private readonly WFOSetting _wfoSetting = wfoSetting;
    private readonly IWorkflowService _workflowService = workflowService;

    public async Task AddAsync(ServiceInstance serviceInstance)
    {
        try
        {
            var instance = await GetByServiceCodeAndGrpcUrlAsync(
                serviceInstance.ServiceCode,
                serviceInstance.GrpcUrl
            );
            if (instance != null)
            {
                instance.SetServiceInstance(serviceInstance);
                await UpdateAsync(instance);
            }
            else
            {
                await _serviceInstanceRepository.Insert(serviceInstance);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<ServiceInstance> GetByServiceCodeAndGrpcUrlAsync(
        string serviceCode,
        string grpcUrl
    )
    {
        return await _serviceInstanceRepository
            .Table.Where(x => x.ServiceCode == serviceCode && x.GrpcUrl == grpcUrl)
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceInstance> GetByServiceCodeAsync(string serviceCode)
    {
        try
        {
            var query = _serviceInstanceRepository.Table.Where(x => x.ServiceCode == serviceCode);

            var currentHost = GetHostFromGrpcUrl(
                Singleton<O24OpenAPIConfiguration>.Instance.YourGrpcURL
            );
            Console.WriteLine($"[DEBUG] currentHost = {currentHost}");

            var allMatches = await query.ToListAsync();

            if (string.IsNullOrEmpty(currentHost))
            {
                Console.WriteLine("[WARN] Cannot detect currentHost from YourGrpcURL");
                return allMatches.FirstOrDefault();
            }

            var exactMatch = allMatches.FirstOrDefault(x =>
            {
                var host = GetHostFromGrpcUrl(x.GrpcUrl);
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

    public async Task<ServiceInstance> GetByServiceHanleNameAsync(string serviceHandleName)
    {
        try
        {
            var query = _serviceInstanceRepository.Table.Where(x =>
                x.ServiceHandleName == serviceHandleName
            );

            var currentHost = GetHostFromGrpcUrl(
                Singleton<O24OpenAPIConfiguration>.Instance.YourGrpcURL
            );
            Console.WriteLine($"[DEBUG] currentHost = {currentHost}");

            var allMatches = await query.ToListAsync();

            if (string.IsNullOrEmpty(currentHost))
            {
                Console.WriteLine("[WARN] Cannot detect currentHost from YourGrpcURL");
                return allMatches.FirstOrDefault();
            }

            var exactMatch = allMatches.FirstOrDefault(x =>
            {
                var host = GetHostFromGrpcUrl(x.GrpcUrl);
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

    private static string GetHostFromGrpcUrl(string grpcUrl)
    {
        Console.WriteLine($"[INFO] grpcUrl instance = {grpcUrl}");
        try
        {
            var uri = new Uri(grpcUrl);
            var host = uri.Host;

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

    public async Task UpdateAsync(ServiceInstance serviceInstance)
    {
        await _serviceInstanceRepository.Update(serviceInstance);
    }

    public async Task<ServiceInfo> QueryServiceInfo(string pServiceCode, string pInstanceID)
    {
        ServiceInfo serviceInfo = new();
        if (pServiceCode != "")
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

        //serviceInfo.props.Add(
        //    ServiceInfo.EnumServiceProps.NeptuneClientVersion,
        //    typeof(GrpcClient).Assembly.GetName().Version.ToString()
        //);
        return serviceInfo;
    }

    private async Task BuildServiceInfo(ServiceInfo serviceInfo)
    {
        var wfoServiceInfo = Singleton<AppSettings>.Instance.Get<ServiceInfoConfiguration>();
        serviceInfo.broker_hostname = wfoServiceInfo.BrokerHostname;
        serviceInfo.broker_port = wfoServiceInfo.BrokerPort;
        serviceInfo.broker_user_name = wfoServiceInfo.BrokerUsername;
        serviceInfo.broker_user_password = wfoServiceInfo.BrokerPassword;
        if (_wfoSetting.MessageBrokerPasswordEncryptionMethod.StartsWithOrdinalIgnoreCase("AES"))
        {
            serviceInfo.broker_user_password = O24OpenAPITextEncryptor.AESDecryptString(
                serviceInfo.broker_user_password
            );
        }
        serviceInfo.broker_virtual_host = wfoServiceInfo.BrokerVirtualHost.Coalesce("/");
        serviceInfo.o24openapi_server_time = Common.GetCurrentDateAsLongNumber();
        serviceInfo.service_ping_interval_seconds = _wfoSetting.ServerPingIntervalInSecond;
        serviceInfo.o24openapi_grpc_url = Singleton<AppSettings>
            .Instance.Get<O24OpenAPIConfiguration>()
            .YourGrpcURL;
        serviceInfo.o24openapi_grpc_timeout_seconds = _wfoSetting.GrpcTimeoutInSeconds;
        serviceInfo.workflow_execution_timeout_seconds = await _workflowService.GetMaxTimeOut();
        serviceInfo.ssl_active = _wfoSetting.MessageBrokerSSLActive.ToString();
        serviceInfo.broker_reconnect_interval_in_seconds =
            _wfoSetting.MessageBrokerReconnectIntervalInSecond;
        serviceInfo.redis_server_name = _wfoSetting.RedisServerName;
        serviceInfo.redis_server_port = _wfoSetting.RedisServerPort;
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
}
