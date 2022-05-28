using Common.Service.Provider.Enum;

namespace Common.Service.Provider.Model
{
    public struct LogData
    {
        public INFORMATION_TYPE INFORMATION_TYPE { get; set; }
        public string Time { get; set; }
        public string ID { get; set; }
        public string ApiId { get; set; }
        public string Data { get; set; }
        public string Duration { get; set; }
    }
}
