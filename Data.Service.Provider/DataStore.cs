using Common.Service.Provider.Enum;
using Common.Service.Provider.Interfaces;
using Common.Service.Provider.Model;
using System.Collections.Generic;

namespace Data.Service.Provider
{
    public class DataStore : IDataStore
    {
        public IDictionary<string, IDictionary<INFORMATION_TYPE, LogData>> InformationTypeLogData { get; }
        public IDictionary<string, int> ApiIdCounts { get; }

        public DataStore()
        {
            InformationTypeLogData = new Dictionary<string, IDictionary<INFORMATION_TYPE, LogData>>();
            ApiIdCounts = new Dictionary<string, int>();
        }

        public void AddUpdateInformationTypeLogData(LogData logData)
        {
            IDictionary<INFORMATION_TYPE, LogData> informationTypeLogData;

            lock (InformationTypeLogData)
            {
                InformationTypeLogData.TryGetValue(logData.ID, out informationTypeLogData);
            }

            if (informationTypeLogData != null)
            {
                lock (informationTypeLogData)
                {
                    if (!informationTypeLogData.TryGetValue(logData.INFORMATION_TYPE, out _))
                    {
                        informationTypeLogData.Add(logData.INFORMATION_TYPE, logData);
                    }
                }
            }
            else
            {
                informationTypeLogData = new Dictionary<INFORMATION_TYPE, LogData>();
                lock (informationTypeLogData)
                {
                    informationTypeLogData.Add(logData.INFORMATION_TYPE, logData);
                }

                lock (InformationTypeLogData)
                {
                    InformationTypeLogData.Add(logData.ID, informationTypeLogData);
                }
            }


            if (logData.INFORMATION_TYPE == INFORMATION_TYPE.REQUEST)
            {
                lock (ApiIdCounts)
                {
                    if (ApiIdCounts.TryGetValue(logData.ApiId, out int count))
                    {
                        ApiIdCounts[logData.ApiId] = count + 1;
                    }
                    else
                    {
                        ApiIdCounts.Add(logData.ApiId, 1);
                    }
                } 
            }
        }
    }
}
