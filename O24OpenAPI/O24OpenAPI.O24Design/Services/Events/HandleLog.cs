//using Newtonsoft.Json;
//using O24OpenAPI.Core.Infrastructure;
//using O24OpenAPI.Sample.Domain;
//using O24OpenAPI.Sample.Services.CommandHandler;
//using O24OpenAPI.Sample.Services.Interfaces;
//using O24OpenAPI.Web.Framework.Domain.Logging;
//using O24OpenAPI.Web.Framework.Services.Events;

//namespace O24OpenAPI.Sample.Services.Events
//{
//    public class HandleLog : IConsumer<LogEvent>
//    {
//        private static Type GetTypeCommand(string logType)
//        {
//            return logType switch
//            {
//                "HTTP_LOG" => typeof(HttpLog),
//                _ => typeof(ServiceLog),
//            };
//        }

//        public async Task HandleEvent(LogEvent eventMessage)
//        {
//            try
//            {
//                var type = GetTypeCommand(eventMessage.LogType);
//                var logCommand = Activator.CreateInstance(
//                    typeof(LogCommand<>).MakeGenericType(type),
//                    eventMessage.TextData
//                );
//                var handlerType = typeof(LogCommandHandler<>).MakeGenericType(type);
//                var handler = EngineContext.Current.Resolve(handlerType);

//                var handleMethod = handlerType.GetMethod("Handle");
//                if (handleMethod != null)
//                {
//                    await (Task)handleMethod.Invoke(handler, new object[] { logCommand });
//                }
//            }
//            catch (Exception ex)
//            {
//                var logE = new ServiceLog()
//                {
//                    LogLevelId = 40,
//                    ServiceId = "LOG",
//                    ShortMessage = ex.Message,
//                    FullMessage = ex.StackTrace,
//                    Data = JsonConvert.SerializeObject(eventMessage),
//                };
//                await EngineContext.Current.Resolve<ILogService<ServiceLog>>().AddAsync(logE);
//            }
//        }
//    }
//}
