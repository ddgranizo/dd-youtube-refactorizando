using System;
using System.Text;

namespace Refactorizando.Shared.Data.Models.Responses
{
    public enum ServiceResponseType
    {
        Undefined = 0,
        Ok = 1,
        Error = 2,
        Warning = 3,
        Exception = 4
    }

    public class ServiceResponse
    {
        protected const string DefaultOkCode = "OKI";
        protected const string DefaultErrorCode = "ERR";
        protected const string DefaultWarningCode = "WRN";
        protected const string DefaultExceptionCode = "EXC";
        protected const string DefaultUndefinedCode = "UND";

        public ServiceResponseType Type { get; set; } = ServiceResponseType.Undefined;
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Code { get; set; }
        public int StatusCode { get; set; }
        public ServiceResponse() { }

        private ServiceResponse SetStatus(
            ServiceResponseType type,
            string message,
            string stackTrace,
            string code = DefaultUndefinedCode)
        {
            Type = type;
            Message = message;
            StackTrace = stackTrace;
            Code = code;
            return this;
        }

        public ServiceResponse Exception(Exception exception, string code = DefaultExceptionCode)
        {
            return SetStatus(type: ServiceResponseType.Exception,
                             message: GetLastExceptionMessage(exception),
                             stackTrace: new StringBuilder()
                                            .AppendLine($"[{exception.GetType().ToString()}] :: [{exception.Message}]")
                                            .AppendLine($"Inner Exception : [{exception.InnerException?.Message ?? "***"}]")
                                            .AppendLine($"Stack Trace:[{exception.StackTrace ?? "***"}]")
                                            .ToString(),
                            code: code);
        }

        public ServiceResponse Warning(string message, string code = DefaultWarningCode)
        {
            return SetStatus(type: ServiceResponseType.Warning,
                             message: message,
                             stackTrace: null,
                             code: code);
        }

        public ServiceResponse Error(string message, string code = DefaultErrorCode)
        {
            return SetStatus(type: ServiceResponseType.Error,
                             message: message,
                             stackTrace: null,
                             code: code);
        }

        public ServiceResponse Ok(string message = null, string code = DefaultOkCode)
        {
            return SetStatus(type: ServiceResponseType.Ok,
                             message: message,
                             stackTrace: null,
                             code: code);
        }


        private string GetLastExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
            {
                return GetLastExceptionMessage(ex.InnerException);
            }
            return ex.Message;
        }

        public virtual bool IsOkResponse() =>
            Type == ServiceResponseType.Ok;
    }
}