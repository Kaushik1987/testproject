using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AwsWebApi.Models
{
    public class GeneralResponse<T>
    {
        private string msg;

        public string Message
        {
            get
            {
                // In case of error no need to set error msg 
                if (Error && Exception != null && (string.IsNullOrEmpty(msg)))
                {
                    return Exception.Message;
                }
                return msg;
            }
            set => msg = value;
        }

        public T Data { get; set; }
        public ResultType Result { get; set; }

        public Exception Exception { get; set; }
        // public string Message { get; set; }
        public bool Error { get; set; }

        public List<KeyValuePair<string, string>> ExtraData { get; set; }
    }

    public enum ResultType
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Error = 4
    }
}