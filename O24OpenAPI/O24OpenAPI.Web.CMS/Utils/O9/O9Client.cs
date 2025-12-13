using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.Util;
using Jits.Neptune.Web.CMS.LogicOptimal9.Common;
using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.O9;
using O24OpenAPI.Web.CMS.Services.O9;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace O24OpenAPI.Web.CMS.Utils;

public class O9Client
{
    public static JsonLoginResponse CoreBankingSession = new JsonLoginResponse();

    /// <summary>
    ///
    /// </summary>
    public static PooledActiveMQ activeMQ;

    /// <summary>
    ///
    /// </summary>
    public static O9MemCached memCached;

    /// <summary>
    ///
    /// </summary>
    public static CoreConfig coreConfig;

    /// <summary>
    ///
    /// </summary>
    public static Optimal9Settings appSettings;

    /// <summary>
    ///
    /// </summary>
    public static string ipmemCached;

    /// <summary>
    ///
    /// </summary>
    public static string O9_GLOBAL_SEPARATE_ADDRESS { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public static string OP_MCKEY_FMAP { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public static string Comcode { get; set; }

    private static string m_userName;
    private static string m_passWord;

    /// <summary>
    ///
    /// </summary>
    public static string m_urlRequest;

    private static string m_requestQueueName;
    private static string m_requestQueueNameASynchronize;
    private static string m_replyQueueName;
    private static string m_replyNotificationQueueName;
    private static string m_functionRequestID;
    private static string m_functionReplyID;
    private static string m_functionAsynchronizeRequestID;
    private static string m_functionAsynchronizeReplyID;
    private static int m_messageClienTimeOut;
    private static int m_messageClientTimeOut;
    private static int m_sizeSessionID;

    /// <summary>
    ///
    /// </summary>
    public static bool isInit { get; private set; } = false;

    private ActiveMQConnection m_connection;
    private Session m_messageSession;
    private MessageProducer m_messageProducer;
    private ActiveMQDestination m_destination;
    private ActiveMQDestination m_destinationAsynchronize;
    private ActiveMQTextMessage m_message;
    private ActiveMQDestination m_destinationReply;
    private MessageConsumer m_messageConsumerReply;

    /// <summary>
    ///
    /// </summary>
    public static void Init(string ipmemcached, Optimal9Settings app)
    {
        if (coreConfig == null)
        {
            appSettings = app;
            coreConfig = app.Configure;
            ipmemCached = ipmemcached;
            memCached = new O9MemCached(ipmemcached);
        }

        try
        {
            InitParam();
            if (string.IsNullOrEmpty(m_urlRequest))
            {
                return;
            }

            activeMQ = new PooledActiveMQ(
                m_userName,
                m_passWord,
                m_urlRequest,
                5,
                app.PoolConnection
            );
            activeMQ.Init();
            GlobalVariable.TIME_UPDATE_TXDT = coreConfig.WKDTimes;
            //GlobalVariable.O9_PERIOD_LOGIN = app.PeriodLogin;
            GlobalVariable.O9_GLOBAL_COREAPILB = app.LbName;
            if (!string.IsNullOrEmpty(m_urlRequest))
            {
                isInit = true;
            }
        }
        catch (Exception)
        {
            isInit = false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void InitParam()
    {
        O9_GLOBAL_SEPARATE_ADDRESS = memCached.GetValue("OP_SERVER_MESSAGE_SEPARATE_ADDRESS");
        OP_MCKEY_FMAP = memCached.GetValue("OP_MCKEY_FMAP");
        m_userName = memCached.GetValue("OP_CLIENT_MESSAGE_USERNAME");
        m_passWord = memCached.GetValue("OP_CLIENT_MESSAGE_PASSWORD");
        m_userName = O9Encrypt.Decrypt(m_userName);
        m_passWord = O9Encrypt.Decrypt(m_passWord);
        m_urlRequest = memCached.GetValue("OP_CLIENT_MESSAGE_BROKER_URL");
        m_requestQueueName = memCached.GetValue("OP_CLIENT_MESSAGE_QUEUE_NAME");
        m_requestQueueNameASynchronize = memCached.GetValue(
            "OP_CLIENT_MESSAGE_ASYNCHRONIZE_QUEUE_NAME"
        );
        m_replyQueueName = memCached.GetValue("OP_CLIENT_MESSAGE_REPLY_NAME");
        m_replyQueueName = m_replyQueueName + O9_GLOBAL_SEPARATE_ADDRESS;
        m_replyNotificationQueueName = memCached.GetValue(
            "OP_CLIENT_MESSAGE_NOTIFICATION_REPLY_NAME"
        );
        m_functionRequestID = memCached.GetValue("OP_CLIENT_FUNCTION_REQUEST_ID");
        m_functionReplyID = memCached.GetValue("OP_CLIENT_FUNCTION_REPLY_ID");
        m_functionAsynchronizeRequestID = memCached.GetValue(
            "OP_CLIENT_FUNCTION_ASYNCHRONIZE_REQUEST_ID"
        );
        m_functionAsynchronizeReplyID = memCached.GetValue(
            "OP_CLIENT_FUNCTION_ASYNCHRONIZE_REPLY_ID"
        );
        _ = int.TryParse(
            memCached.GetValue("OP_CLIENT_MESSAGE_CLIENT_TIMEOUT"),
            out m_messageClienTimeOut
        );
        _ = int.TryParse(
            memCached.GetValue("OP_CLIENT_MESSAGE_CLIENT_TIMEOUT"),
            out m_messageClientTimeOut
        );
        _ = int.TryParse(memCached.GetValue("OP_SIZE_OF_SESSIONID"), out m_sizeSessionID);

        if (!GetMemcachedKey())
        {
            throw new Exception("Error at GetMemcachedKey");
        }

        if (!GetHeadOfficeParam())
        {
            throw new Exception("An error occur while GetHeadOfficeParam!");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static bool GetMemcachedKey()
    {
        try
        {
            if (GlobalVariable.O9_GLOBAL_MEMCACHED_KEY == null)
            {
                GlobalVariable.O9_GLOBAL_MEMCACHED_KEY = new MemcachedKey();
                SetValueToParam(GlobalVariable.O9_GLOBAL_MEMCACHED_KEY, "OP_MCKEY_", null, false);
            }

            if (GlobalVariable.O9_GLOBAL_MEMCACHED_KEY != null)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="comCode"></param>
    /// <returns></returns>
    public static bool GetHeadOfficeParam(string comCode = "")
    {
        try
        {
            comCode =
                (string.IsNullOrEmpty(comCode) ? O9Constants.O9_CONSTANT_COM_DEFAULT : comCode)
                + "_";
            GlobalVariable.O9_GLOBAL_HEADOFFICE_PARAM = SetValueToParam<HeadOfficeParam>(
                comCode + GlobalVariable.O9_GLOBAL_MEMCACHED_KEY.Param
            );

            if (GlobalVariable.O9_GLOBAL_HEADOFFICE_PARAM != null)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T SetValueToParam<T>(string key)
    {
        try
        {
            string value = O9Client.memCached.GetValue(key);
            if (!string.IsNullOrEmpty(value))
            {
                return O9Utils.JSONDeserializeObject<T>(value);
            }
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
        }

        return default(T);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="key"></param>
    /// <param name="resManager"></param>
    /// <param name="valueIsJson"></param>
    /// <returns></returns>
    public static bool SetValueToParam(
        object obj,
        string key,
        ResourceManager resManager = null,
        bool valueIsJson = true
    )
    {
        try
        {
            CultureInfo cltInfo = new CultureInfo("en");

            foreach (PropertyInfo pri in obj.GetType().GetProperties())
            {
                string value = "";
                string keyOfMemcached = key + pri.Name.ToUpper();

                if (resManager != null)
                {
                    keyOfMemcached = resManager
                        .GetResourceSet(cltInfo, true, true)
                        .GetString(keyOfMemcached);
                }

                if (!string.IsNullOrEmpty(keyOfMemcached))
                {
                    value = O9Client.memCached.GetValue(keyOfMemcached);
                }

                if (!string.IsNullOrEmpty(value))
                {
                    if (valueIsJson)
                    {
                        JObject jsObj = JObject.Parse(value);

                        if (jsObj != null && jsObj.Count > 0)
                        {
                            JValue jsValue = (JValue)jsObj.SelectToken(O9Constants.O9_KEY_DEFAULT);
                            if (jsValue != null)
                            {
                                value = jsValue.Value.ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (pri.PropertyType == typeof(string))
                        {
                            pri.SetValue(obj, value.ToString(), null);
                        }
                        else if (
                            pri.PropertyType == typeof(int)
                            || pri.PropertyType == typeof(short)
                            || pri.PropertyType == typeof(int)
                            || pri.PropertyType == typeof(long)
                        )
                        {
                            pri.SetValue(obj, int.Parse(value.ToString()), null);
                        }
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            O9Utils.ConsoleWriteLine(ex);
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<string> SendStringAsync(
        string text,
        string functionID,
        string userId,
        string sessionID,
        EnmCacheAction enmCacheAction,
        EnmSendTypeOption sendType,
        MsgPriority priority
    )
    {
        string strReturn = string.Empty;

        try
        {
            if (activeMQ is null)
            {
                throw new Exception("ActiveMQ is not initialized");
            }
            m_connection = await activeMQ.GetConnectionAsync();
            if (m_connection == null)
            {
                throw new Exception("Timeout while waiting for a connection to ActiveMQ!");
            }

            if (!m_connection.IsStart())
            {
                await m_connection.StartAsync();
            }

            m_messageSession = (Session)await m_connection.connection.CreateSessionAsync();
            m_destination = (ActiveMQDestination)
                SessionUtil.GetDestination(m_messageSession, m_requestQueueName);
            m_destinationAsynchronize = (ActiveMQDestination)
                SessionUtil.GetDestination(m_messageSession, m_requestQueueNameASynchronize);
            m_destinationReply = (ActiveMQDestination)
                SessionUtil.GetDestination(
                    m_messageSession,
                    coreConfig.GetReplyString(
                        m_replyQueueName,
                        O9_GLOBAL_SEPARATE_ADDRESS,
                        m_connection.Queue
                    )
                );
            m_messageConsumerReply = (MessageConsumer)
                await m_messageSession.CreateConsumerAsync(m_destinationReply);
            m_messageProducer = (MessageProducer)await m_messageSession.CreateProducerAsync();
            m_message = (ActiveMQTextMessage)await m_messageSession.CreateTextMessageAsync();

            if (sendType == EnmSendTypeOption.Synchronize)
            {
                m_message.NMSCorrelationID =
                    m_functionRequestID
                    + "-"
                    + Guid.NewGuid().ToString()
                    + O9Encrypt.GenerateRandomString();
            }
            else if (sendType == EnmSendTypeOption.AsSynchronize)
            {
                m_message.NMSCorrelationID =
                    m_functionAsynchronizeRequestID + "-" + Guid.NewGuid().ToString();
            }

            string strSession = "";
            var strIsCaching = "N";
            if (enmCacheAction == EnmCacheAction.Cached)
            {
                strIsCaching = "C";
            }

            strSession = sessionID.PadRight(m_sizeSessionID);
            m_message.Content = O9Compression.SetCompressText(strIsCaching + strSession + text);

            m_message.UserID = userId;
            m_message.NMSPriority = priority;
            m_message.NMSType = functionID;
            m_message.ReplyTo = m_destinationReply;
            m_messageProducer.DeliveryMode = MsgDeliveryMode.NonPersistent;

            if (sendType == EnmSendTypeOption.Synchronize)
            {
                await m_messageProducer.SendAsync(m_destination, m_message);
            }
            else if (sendType == EnmSendTypeOption.AsSynchronize)
            {
                await m_messageProducer.SendAsync(m_destinationAsynchronize, m_message);
            }

            IMessage receivedMsg = null;
            bool flag = false;

            while (flag == false)
            {
                receivedMsg = await m_messageConsumerReply.ReceiveAsync(
                    TimeSpan.FromSeconds(m_messageClientTimeOut)
                );
                if (receivedMsg == null)
                {
                    break;
                }
                else if (
                    receivedMsg != null
                    && receivedMsg.NMSCorrelationID.Equals(m_message.NMSCorrelationID)
                )
                {
                    flag = true;
                }
            }

            if (receivedMsg != null)
            {
                m_message = (ActiveMQTextMessage)receivedMsg;
                strReturn = O9Compression.GetTextFromCompressBytes(m_message.Content);
            }
            else
            {
                throw new Exception(
                    "[CORE_ERROR_SYSTEM]: Timeout occurred while sending a message to O9Hosting!"
                );
            }
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            m_messageProducer?.Close();
            m_messageConsumerReply?.Close();
            m_destination?.Dispose();
            m_destinationAsynchronize?.Dispose();
            m_destinationReply?.Dispose();
            m_messageSession?.Close();
            if (m_connection != null)
            {
                activeMQ.ReleaseConnection(m_connection);
            }
        }

        return strReturn;
    }

    /// <summary>
    ///
    /// </summary>
    public string SendString(
        string text,
        string functionID,
        string userId,
        string sessionID,
        EnmCacheAction enmCacheAction,
        EnmSendTypeOption sendType,
        MsgPriority priority
    )
    {
        string strReturn = string.Empty;

        try
        {
            m_connection = activeMQ.GetConnection();
            if (m_connection != null)
            {
                if (!m_connection.IsStart())
                {
                    m_connection.Start();
                }

                m_messageSession = (Session)m_connection.connection.CreateSession();
                m_destination = (ActiveMQDestination)
                    SessionUtil.GetDestination(m_messageSession, m_requestQueueName);
                m_destinationAsynchronize = (ActiveMQDestination)
                    SessionUtil.GetDestination(m_messageSession, m_requestQueueNameASynchronize);
                m_destinationReply = (ActiveMQDestination)
                    SessionUtil.GetDestination(
                        m_messageSession,
                        coreConfig.GetReplyString(
                            m_replyQueueName,
                            O9_GLOBAL_SEPARATE_ADDRESS,
                            m_connection.Queue
                        )
                    );
                m_messageConsumerReply = (MessageConsumer)
                    m_messageSession.CreateConsumer(m_destinationReply);
                m_messageProducer = (MessageProducer)m_messageSession.CreateProducer();
                m_message = (ActiveMQTextMessage)m_messageSession.CreateTextMessage();

                if (sendType == EnmSendTypeOption.Synchronize)
                {
                    m_message.NMSCorrelationID =
                        m_functionRequestID
                        + "-"
                        + Guid.NewGuid().ToString()
                        + O9Encrypt.GenerateRandomString();
                }
                else if (sendType == EnmSendTypeOption.AsSynchronize)
                {
                    m_message.NMSCorrelationID =
                        m_functionAsynchronizeRequestID + "-" + Guid.NewGuid().ToString();
                }

                string strSession = "";
                var strIsCaching = "N";
                if (enmCacheAction == EnmCacheAction.Cached)
                {
                    strIsCaching = "C";
                }

                strSession = sessionID.PadRight(m_sizeSessionID);
                m_message.Content = O9Compression.SetCompressText(strIsCaching + strSession + text);

                m_message.UserID = userId;
                m_message.NMSPriority = priority;
                m_message.NMSType = functionID;
                m_message.ReplyTo = m_destinationReply;
                m_messageProducer.DeliveryMode = MsgDeliveryMode.NonPersistent;

                if (sendType == EnmSendTypeOption.Synchronize)
                {
                    m_messageProducer.Send(m_destination, m_message);
                }
                else if (sendType == EnmSendTypeOption.AsSynchronize)
                {
                    m_messageProducer.Send(m_destinationAsynchronize, m_message);
                }

                IMessage receivedMsg = null;
                bool flag = false;

                while (flag == false)
                {
                    receivedMsg = m_messageConsumerReply.Receive(
                        TimeSpan.FromSeconds(m_messageClienTimeOut)
                    );
                    if (receivedMsg == null)
                    {
                        break;
                    }
                    else if (
                        receivedMsg != null
                        && receivedMsg.NMSCorrelationID.Equals(m_message.NMSCorrelationID)
                    )
                    {
                        flag = true;
                    }
                }

                if (receivedMsg != null)
                {
                    m_message = (ActiveMQTextMessage)receivedMsg;
                    strReturn = O9Compression.GetTextFromCompressBytes(m_message.Content);
                }
                else
                {
                    strReturn = "CORE_ERROR_SYSTEM Timeout when sending message to O9Hosting";
                }
            }
            else
            {
                strReturn = "TIMEOUTAQ";
            }
        }
        catch (Exception ex)
        {
            strReturn = "COREERRORSYSTEM " + ex.Message + " " + ex.StackTrace;
        }
        finally
        {
            if (m_messageProducer != null)
            {
                m_messageProducer.Close();
            }

            if (m_messageConsumerReply != null)
            {
                m_messageConsumerReply.Close();
            }

            if (m_destination != null)
            {
                m_destination.Dispose();
            }

            if (m_destinationAsynchronize != null)
            {
                m_destinationAsynchronize.Dispose();
            }

            if (m_destinationReply != null)
            {
                m_destinationReply.Dispose();
            }

            if (m_messageSession != null)
            {
                m_messageSession.Close();
            }

            if (m_connection != null)
            {
                activeMQ.ReleaseConnection(m_connection);
            }
        }

        return strReturn;
    }
}
