namespace O24OpenAPI.WFO.Constant;

public class ServiceCode
{
    public const string CMS = "CMS";
    public const string CBGateway = "CBG";
    public const string SMS = "SMS";
    public const string Report = "RPT";
    public const string Logger = "LOG";
    public const string DataWarehouse = "DWH";
    public const string O24DTS = "DTS";
    public const string ControlHub = "CTH";
    public const string WorkflowOrchestrator = "WFO";
    public const string NotificationHub = "NCH";
    public const string SmartTellerApp = "STL";
    public static readonly List<string> AllServices =
    [
        CMS,
        CBGateway,
        SMS,
        Report,
        Logger,
        DataWarehouse,
        O24DTS,
        ControlHub,
        WorkflowOrchestrator,
        NotificationHub,
        SmartTellerApp
    ];
}
