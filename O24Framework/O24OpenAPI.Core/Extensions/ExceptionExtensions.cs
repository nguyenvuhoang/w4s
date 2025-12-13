using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace O24OpenAPI.Core.Extensions;

/// <summary>
/// The exception extensions class
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Debugs the message using the specified ex
    /// </summary>
    /// <param name="ex">The ex</param>
    /// <param name="frameNumber">The frame number</param>
    /// <returns>The string</returns>
    public static string DebugMessage(this Exception ex, int frameNumber = -1)
    {
        if (!(ex is O24OpenAPIException))
        {
            return ex.ToString();
        }

        StackTrace stackTrace = new StackTrace(ex, true);
        if (frameNumber == -1)
        {
            frameNumber = stackTrace.FrameCount - 1;
        }

        StackFrame frame = stackTrace.GetFrame(frameNumber);
        int fileLineNumber = frame.GetFileLineNumber();
        string fullName = frame.GetMethod().ReflectedType.FullName;
        frame.GetMethod().GetMethodContextName();
        DefaultInterpolatedStringHandler interpolatedStringHandler =
            new DefaultInterpolatedStringHandler(12, 4);
        interpolatedStringHandler.AppendFormatted(ex.GetType().Name);
        interpolatedStringHandler.AppendLiteral(": ");
        interpolatedStringHandler.AppendFormatted(ex.Message);
        interpolatedStringHandler.AppendLiteral("\n");
        interpolatedStringHandler.AppendFormatted(fullName);
        interpolatedStringHandler.AppendLiteral(" at line ");
        interpolatedStringHandler.AppendFormatted<int>(fileLineNumber);
        return interpolatedStringHandler.ToStringAndClear();
    }

    /// <summary>
    /// Gets the method context name using the specified method
    /// </summary>
    /// <param name="method">The method</param>
    /// <returns>The string</returns>
    public static string GetMethodContextName(this MethodBase method)
    {
        if (!method.DeclaringType.GetInterfaces().Any<Type>(i => i == typeof(IAsyncStateMachine)))
        {
            return method.Name;
        }

        Type generatedType = method.DeclaringType;
        return generatedType
            .DeclaringType.GetMethods(
                BindingFlags.DeclaredOnly
                    | BindingFlags.Instance
                    | BindingFlags.Static
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
            )
            .Single<MethodInfo>(m =>
                m.GetCustomAttribute<AsyncStateMachineAttribute>()?.StateMachineType
                == generatedType
            )
            .Name;
    }
}
