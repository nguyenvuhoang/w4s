using System.Collections.Concurrent;
using Apache.NMS.ActiveMQ;

namespace O24OpenAPI.Web.CMS.Services.O9;

    public class PooledActiveMQ
    {
        private ConnectionFactory _connectionFactory;
        private readonly BlockingCollection<ActiveMQConnection> _connection;
        /// <summary>
        /// 
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Urlrequest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isAllowDirect { get; set; } = false;
        /// <summary>
        /// 
        /// </summary>
        public int TimeOut { get; set; } = 60;
        /// <summary>
        /// 
        /// </summary>
        public int MinConnection { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int MaxConnection { get; set; } = 20;

        private int queueCount = 1;
        private int IsusingCount = 0;

        /// <summary>
        /// 
        /// </summary>
        public PooledActiveMQ(string username, string password, string urlrequest)
        {
            Username = username;
            Password = password;
            Urlrequest = urlrequest;
            _connection = new BlockingCollection<ActiveMQConnection>(MaxConnection);
        }

        /// <summary>
        /// 
        /// </summary>
        public PooledActiveMQ(string username, string password, string urlrequest, int minConnect, int maxConnect)
        {
            Username = username;
            Password = password;
            Urlrequest = urlrequest;
            MinConnection = minConnect;
            MaxConnection = maxConnect;
            _connection = new BlockingCollection<ActiveMQConnection>(MaxConnection);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            ActiveMQConnection m_connect;
            _connectionFactory = new ConnectionFactory(Urlrequest);
            for (int i = 0; i < MaxConnection; i++)
            {
                m_connect = InitConnection();
                if (i <= MinConnection)
            {
                m_connect.Init(_connectionFactory);
            }

            _connection.Add(m_connect);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActiveMQConnection InitConnection()
        {
            ActiveMQConnection m_connect;
            m_connect = new ActiveMQConnection(Username, Password, queueCount);
            Interlocked.Increment(ref queueCount);
            return m_connect;
        }

        /// <summary>
        /// 
        /// </summary>
        public ActiveMQConnection GetConnection()
        {
            ActiveMQConnection m_connect;

            if (_connection.TryTake(out m_connect, TimeSpan.FromSeconds(TimeOut)))
            {
                Interlocked.Increment(ref IsusingCount);
                if (!m_connect.IsInit)
            {
                m_connect.Init(_connectionFactory);
            }

            return m_connect;
            }
            else
            {
                m_connect = isAllowDirect ? InitConnection() : null;
                if (m_connect != null)
            {
                m_connect.Init(_connectionFactory);
            }
        }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ActiveMQConnection> GetConnectionAsync()
        {
            if (_connection.TryTake(out ActiveMQConnection m_connect, TimeSpan.FromSeconds(TimeOut)))
            {
                Interlocked.Increment(ref IsusingCount);
                if (!m_connect.IsInit)
            {
                await m_connect.InitAsync(_connectionFactory);
            }

            return m_connect;
            }
            else
            {
                m_connect = isAllowDirect ? InitConnection() : null;
                if (m_connect != null)
            {
                await m_connect.InitAsync(_connectionFactory);
            }
        }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReleaseConnection(ActiveMQConnection con)
        {
            if (con != null)
            {
                if (IsusingCount > 0)
                {
                    if (_connection.TryAdd(con))
                {
                    Interlocked.Decrement(ref IsusingCount);
                }
            }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TotalCount()
        {
            return _connection.Count + IsusingCount;
        }

        /// <summary>
        /// 
        /// </summary>
        public int AvailableCount()
        {
            return _connection.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public int InUsingCount()
        {
            return IsusingCount;
        }
    }