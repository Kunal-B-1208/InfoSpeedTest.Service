using Common.Service.Provider.Enum;
using Common.Service.Provider.Interfaces;
using Common.Service.Provider.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Service.Provider
{
    public class DataManager : IDataManager
    {
        private readonly IDataStore dataStore;
        public DataManager(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public dynamic GetLogsbyId(string Id)
        {
            if (dataStore.InformationTypeLogData.TryGetValue(Id, out IDictionary<INFORMATION_TYPE, LogData> data))
            {
                var requestLogData = data.Where(x => x.Key == INFORMATION_TYPE.REQUEST).First().Value;
                var responseLogData = data.Where(x => x.Key == INFORMATION_TYPE.RESPONSE).First().Value;

                return new
                {
                    RequestTime = requestLogData.Time,
                    ResponseTime = responseLogData.Time,
                    Request = requestLogData.Data,
                    Response = responseLogData.Data,
                    Duration = responseLogData.Duration
                };
            }

            return null;
        }

        public dynamic GetSummary()
        {
            return dataStore.ApiIdCounts.Select((x) => new
            {
                ApiId = x.Key,
                Count = x.Value
            }).ToList();
        }

        public void LoadLogDataFromFile(string[] filePaths)
        {
            Parallel.ForEach(filePaths, filePath =>
             {
                 ReadAndProcessDataFromFile(filePath);
             });
        }

        private void ReadAndProcessDataFromFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                foreach (var line in File.ReadLines(filepath))
                {
                    var data = line.Split('|', 2);

                    if (data.Count() > 1)
                    {
                        INFORMATION_TYPE infoType = INFORMATION_TYPE.NA;
                        var jsonObject = JObject.Parse(data[1].Substring(0, data[1].Length - 1));
                        var informationType = jsonObject["INFORMATION_TYPE"];
                        var ID = jsonObject["ID"];
                        var ApiType = jsonObject["ApiType"];
                        var rawData = string.Empty;
                        var apiId = string.Empty;
                        var duration = string.Empty;

                        if (informationType != null && ApiType != null)
                        {
                            if (informationType.ToString() == "REQUEST")
                            {
                                infoType = INFORMATION_TYPE.REQUEST;
                                rawData = jsonObject["RawRequest"].ToString();
                                apiId = jsonObject["RawRequest"]["ApiId"].ToString();
                            }
                            else if (informationType.ToString() == "RESPONSE")
                            {
                                infoType = INFORMATION_TYPE.RESPONSE;
                                rawData = jsonObject["Response"].ToString();
                                duration = jsonObject["Duration"].ToString();
                            }


                            if (infoType != INFORMATION_TYPE.NA && ID != null)
                            {
                                var logData = new LogData
                                {
                                    Time = data[0],
                                    INFORMATION_TYPE = infoType,
                                    ID = ID.ToString(),
                                    ApiId = apiId,
                                    Data = rawData,
                                    Duration = duration
                                };

                                dataStore.AddUpdateInformationTypeLogData(logData);
                            }
                        }
                    }
                }
            }
        }

    }
}
