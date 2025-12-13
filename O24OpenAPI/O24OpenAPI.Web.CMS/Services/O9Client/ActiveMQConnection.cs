using Apache.NMS.ActiveMQ;

namespace O24OpenAPI.Web.CMS.Services.O9;

public class ActiveMQConnection
{
    /// <summary>
    /// 
    /// </summary>
    public Connection connection { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int Queue { get; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsInit { get; set; } = false;
    private string UserName { get; set; }
    private string Password { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ActiveMQConnection(string userName, string password, int queue)
    {
        UserName = userName;
        Password = password;
        Queue = queue;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Init(ConnectionFactory connectionFactory)
    {
        connection = (Connection)connectionFactory.CreateConnection(UserName, Password);
        IsInit = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connectionFactory"></param>
    public async Task InitAsync(ConnectionFactory connectionFactory)
    {
        connection = (Connection) await connectionFactory.CreateConnectionAsync(UserName, Password);
        IsInit = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsStart()
    {
        if (connection != null)
        {
            return connection.IsStarted;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        if (connection != null && !connection.IsStarted)
        {
            connection.Start();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task StartAsync()
    {
        if (connection != null && !connection.IsStarted)
        {
            await connection.StartAsync();
        }
    }
}