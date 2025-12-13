namespace O24OpenAPI.O24OpenAPIClient.Configuration;

/// <summary>
/// The enum variable name enum
/// </summary>
public enum EnumVariableName
{
    /// <summary>
    /// The data limit record in searching enum variable name
    /// </summary>
    DATA_LIMIT_RECORD_IN_SEARCHING,
    /// <summary>
    /// The info service id enum variable name
    /// </summary>
    INFO_SERVICE_ID,
    /// <summary>
    /// The grpc active enum variable name
    /// </summary>
    GRPC_ACTIVE,
    /// <summary>
    /// The grpc url enum variable name
    /// </summary>
    GRPC_URL,
    /// <summary>
    /// The grpc timeout in seconds enum variable name
    /// </summary>
    GRPC_TIMEOUT_IN_SECONDS,
    /// <summary>
    /// The grpc max receive message size in mb enum variable name
    /// </summary>
    GRPC_MAX_RECEIVE_MESSAGE_SIZE_IN_MB,
    /// <summary>
    /// The grpc max send message size in mb enum variable name
    /// </summary>
    GRPC_MAX_SEND_MESSAGE_SIZE_IN_MB,
    /// <summary>
    /// The neptune ping interval in seconds enum variable name
    /// </summary>
    NEPTUNE_PING_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The neptune console logger enabled enum variable name
    /// </summary>
    NEPTUNE_CONSOLE_LOGGER_ENABLED,
    /// <summary>
    /// The message broker hostname enum variable name
    /// </summary>
    MESSAGE_BROKER_HOSTNAME,
    /// <summary>
    /// The message broker password enum variable name
    /// </summary>
    MESSAGE_BROKER_PASSWORD,
    /// <summary>
    /// The message broker password encryption method enum variable name
    /// </summary>
    MESSAGE_BROKER_PASSWORD_ENCRYPTION_METHOD,
    /// <summary>
    /// The message broker port enum variable name
    /// </summary>
    MESSAGE_BROKER_PORT,
    /// <summary>
    /// The message broker ssl active enum variable name
    /// </summary>
    MESSAGE_BROKER_SSL_ACTIVE,
    /// <summary>
    /// The message broker ssl cert pass pharse enum variable name
    /// </summary>
    MESSAGE_BROKER_SSL_CERT_PASS_PHARSE,
    /// <summary>
    /// The message broker ssl cert path enum variable name
    /// </summary>
    MESSAGE_BROKER_SSL_CERT_PATH,
    /// <summary>
    /// The message broker ssl cert servername enum variable name
    /// </summary>
    MESSAGE_BROKER_SSL_CERT_SERVERNAME,
    /// <summary>
    /// The message broker sub queue name enum variable name
    /// </summary>
    MESSAGE_BROKER_SUB_QUEUE_NAME,
    /// <summary>
    /// The message broker username enum variable name
    /// </summary>
    MESSAGE_BROKER_USERNAME,
    /// <summary>
    /// The message broker virtualhost enum variable name
    /// </summary>
    MESSAGE_BROKER_VIRTUALHOST,
    /// <summary>
    /// The message broker timetolive event enum variable name
    /// </summary>
    MESSAGE_BROKER_TIMETOLIVE_EVENT,
    /// <summary>
    /// The message broker timetolive workflow enum variable name
    /// </summary>
    MESSAGE_BROKER_TIMETOLIVE_WORKFLOW,
    /// <summary>
    /// The message broker timetolive text enum variable name
    /// </summary>
    MESSAGE_BROKER_TIMETOLIVE_TEXT,
    /// <summary>
    /// The message broker reconnect interval in seconds enum variable name
    /// </summary>
    MESSAGE_BROKER_RECONNECT_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The service expiration grant in seconds enum variable name
    /// </summary>
    SERVICE_EXPIRATION_GRANT_IN_SECONDS,
    /// <summary>
    /// The service ping interval in seconds enum variable name
    /// </summary>
    SERVICE_PING_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The service grpc timeout in seconds enum variable name
    /// </summary>
    SERVICE_GRPC_TIMEOUT_IN_SECONDS,
    /// <summary>
    /// The workflow concurrent receiving threads enum variable name
    /// </summary>
    WORKFLOW_CONCURRENT_RECEIVING_THREADS,
    /// <summary>
    /// The workflow concurrent running threads enum variable name
    /// </summary>
    WORKFLOW_CONCURRENT_RUNNING_THREADS,
    /// <summary>
    /// The workflow concurrent sending threads enum variable name
    /// </summary>
    WORKFLOW_CONCURRENT_SENDING_THREADS,
    /// <summary>
    /// The db purge interval in seconds enum variable name
    /// </summary>
    DB_PURGE_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db purge backwards in seconds enum variable name
    /// </summary>
    DB_PURGE_BACKWARDS_IN_SECONDS,
    /// <summary>
    /// The db purge start utc time in hhmmss enum variable name
    /// </summary>
    DB_PURGE_START_UTC_TIME_IN_HHMMSS,
    /// <summary>
    /// The db purge command timeout seconds enum variable name
    /// </summary>
    DB_PURGE_COMMAND_TIMEOUT_SECONDS,
    /// <summary>
    /// The db purge max workflows per processing enum variable name
    /// </summary>
    DB_PURGE_MAX_WORKFLOWS_PER_PROCESSING,
    /// <summary>
    /// The db purge apilog backwards in seconds enum variable name
    /// </summary>
    DB_PURGE_APILOG_BACKWARDS_IN_SECONDS,
    /// <summary>
    /// The db purge apilog interval in seconds enum variable name
    /// </summary>
    DB_PURGE_APILOG_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db purge grpclog backwards in seconds enum variable name
    /// </summary>
    DB_PURGE_GRPCLOG_BACKWARDS_IN_SECONDS,
    /// <summary>
    /// The db purge grpclog interval in seconds enum variable name
    /// </summary>
    DB_PURGE_GRPCLOG_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db purge jwt interval in seconds enum variable name
    /// </summary>
    DB_PURGE_JWT_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db purge instance interval in seconds enum variable name
    /// </summary>
    DB_PURGE_INSTANCE_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db purge eventlog backwards in seconds enum variable name
    /// </summary>
    DB_PURGE_EVENTLOG_BACKWARDS_IN_SECONDS,
    /// <summary>
    /// The db purge eventlog interval in seconds enum variable name
    /// </summary>
    DB_PURGE_EVENTLOG_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The password policy minimum length enum variable name
    /// </summary>
    PASSWORD_POLICY_MINIMUM_LENGTH,
    /// <summary>
    /// The password policy maximum length enum variable name
    /// </summary>
    PASSWORD_POLICY_MAXIMUM_LENGTH,
    /// <summary>
    /// The password policy meet complexity enum variable name
    /// </summary>
    PASSWORD_POLICY_MEET_COMPLEXITY,
    /// <summary>
    /// The password policy minimum age in day enum variable name
    /// </summary>
    PASSWORD_POLICY_MINIMUM_AGE_IN_DAY,
    /// <summary>
    /// The password policy maximum age in day enum variable name
    /// </summary>
    PASSWORD_POLICY_MAXIMUM_AGE_IN_DAY,
    /// <summary>
    /// The password policy allow use history enum variable name
    /// </summary>
    PASSWORD_POLICY_ALLOW_USE_HISTORY,
    /// <summary>
    /// The password policy number login failure to block enum variable name
    /// </summary>
    PASSWORD_POLICY_NUMBER_LOGIN_FAILURE_TO_BLOCK,
    /// <summary>
    /// The password policy encryption method enum variable name
    /// </summary>
    PASSWORD_POLICY_ENCRYPTION_METHOD,
    /// <summary>
    /// The db archive interval in seconds enum variable name
    /// </summary>
    DB_ARCHIVE_INTERVAL_IN_SECONDS,
    /// <summary>
    /// The db archive backwards in seconds enum variable name
    /// </summary>
    DB_ARCHIVE_BACKWARDS_IN_SECONDS,
    /// <summary>
    /// The db archive start utc time in hhmmss enum variable name
    /// </summary>
    DB_ARCHIVE_START_UTC_TIME_IN_HHMMSS,
    /// <summary>
    /// The db archive server name enum variable name
    /// </summary>
    DB_ARCHIVE_SERVER_NAME,
    /// <summary>
    /// The db archive server port enum variable name
    /// </summary>
    DB_ARCHIVE_SERVER_PORT,
    /// <summary>
    /// The db archive name enum variable name
    /// </summary>
    DB_ARCHIVE_NAME,
    /// <summary>
    /// The db archive schema enum variable name
    /// </summary>
    DB_ARCHIVE_SCHEMA,
    /// <summary>
    /// The db archive rdbms type enum variable name
    /// </summary>
    DB_ARCHIVE_RDBMS_TYPE,
    /// <summary>
    /// The db archive username enum variable name
    /// </summary>
    DB_ARCHIVE_USERNAME,
    /// <summary>
    /// The db archive password enum variable name
    /// </summary>
    DB_ARCHIVE_PASSWORD,
    /// <summary>
    /// The db archive password encryption method enum variable name
    /// </summary>
    DB_ARCHIVE_PASSWORD_ENCRYPTION_METHOD,
    /// <summary>
    /// The db archive max workflows per processing enum variable name
    /// </summary>
    DB_ARCHIVE_MAX_WORKFLOWS_PER_PROCESSING,
    /// <summary>
    /// The db archive command timeout seconds enum variable name
    /// </summary>
    DB_ARCHIVE_COMMAND_TIMEOUT_SECONDS,
    /// <summary>
    /// The db general command timeout seconds enum variable name
    /// </summary>
    DB_GENERAL_COMMAND_TIMEOUT_SECONDS,
    /// <summary>
    /// The workflow default tx timeout in milisecond enum variable name
    /// </summary>
    WORKFLOW_DEFAULT_TX_TIMEOUT_IN_MILISECOND,
    /// <summary>
    /// The workflow max number message in queue enum variable name
    /// </summary>
    WORKFLOW_MAX_NUMBER_MESSAGE_IN_QUEUE,
    /// <summary>
    /// The workflow apply role authorization enum variable name
    /// </summary>
    WORKFLOW_APPLY_ROLE_AUTHORIZATION,
    /// <summary>
    /// The log write grpc log enum variable name
    /// </summary>
    LOG_WRITE_GRPC_LOG,
    /// <summary>
    /// The redis server name enum variable name
    /// </summary>
    REDIS_SERVER_NAME,
    /// <summary>
    /// The redis server port enum variable name
    /// </summary>
    REDIS_SERVER_PORT,
    /// <summary>
    /// The redis user name enum variable name
    /// </summary>
    REDIS_USER_NAME,
    /// <summary>
    /// The redis user password enum variable name
    /// </summary>
    REDIS_USER_PASSWORD,
    /// <summary>
    /// The redis user password encryption method enum variable name
    /// </summary>
    REDIS_USER_PASSWORD_ENCRYPTION_METHOD,
}
