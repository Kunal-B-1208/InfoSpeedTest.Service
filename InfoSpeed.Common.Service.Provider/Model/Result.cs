using Common.Service.Provider.Enum;

namespace InfoSpeed.Common.Service.Provider.Model
{
    public struct Result
    {

        public Result(Status status, string message, object data)
        {
            this.Status = status;
            this.Message = message;
            this.Data = data;
        }

        public Result(Status status, string message)
        {
            this.Status = status;
            this.Message = message;
            this.Data = null;
        }

        public Result(Status status)
        {
            this.Status = status;
            this.Message = string.Empty;
            this.Data = null;
        }

        public Status Status { get; set; } 
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
