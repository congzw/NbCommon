using System;
using System.Diagnostics;
using System.Text;

namespace NbPilot.Common
{
    /// <summary>
    /// StackTrace Logger
    /// </summary>
    public static class StackTraceLogger
    {
        /// <summary>
        /// StackTrace Logger
        /// </summary>
        /// <returns></returns>
        public static string CurrentStackCustomizedLog()
        {
            var stackLog = new StringBuilder();
            var currentStack = new StackTrace(true);

            for (var x = 0; x < currentStack.FrameCount; ++x)
            {
                var methodCall = currentStack.GetFrame(x);
                if (IsMethodToBeIncluded(methodCall))
                {
                    stackLog.AppendLine(MethodCallLog(methodCall));
                }
            }
            return stackLog.ToString();
        }

        // This method is used to keep Logger methods out of the returned log
        // (the methods actually included in a StackTrace
        // depend on compiler optimizations).
        private static bool IsMethodToBeIncluded(StackFrame stackMethod)
        {
            var method = stackMethod.GetMethod();
            return method.DeclaringType != typeof(StackTraceLogger);
        }

        // Instead of visiting each field of stackFrame,
        // the StackFrame.ToString() method could be used, 
        // but the returned text would not include the class name.
        private static string MethodCallLog(StackFrame methodCall)
        {
            StringBuilder methodCallLog = new StringBuilder();

            var method = methodCall.GetMethod();
            methodCallLog.Append(method.DeclaringType);
            methodCallLog.Append(".");
            methodCallLog.Append(methodCall.GetMethod().Name);

            var methodParameters = method.GetParameters();
            methodCallLog.Append("(");
            for (Int32 x = 0; x < methodParameters.Length; ++x)
            {
                if (x > 0)
                    methodCallLog.Append(", ");
                var methodParameter = methodParameters[x];
                methodCallLog.Append(methodParameter.ParameterType.Name);
                methodCallLog.Append(" ");
                methodCallLog.Append(methodParameter.Name);
            }
            methodCallLog.Append(")");

            var sourceFileName = methodCall.GetFileName();
            if (!string.IsNullOrEmpty(sourceFileName))
            {
                methodCallLog.Append(" in ");
                methodCallLog.Append(sourceFileName);
                methodCallLog.Append(": line ");
                methodCallLog.Append(methodCall.GetFileLineNumber().ToString());
            }

            return methodCallLog.ToString();
        }
    }
}
