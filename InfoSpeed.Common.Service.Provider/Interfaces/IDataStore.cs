using Common.Service.Provider.Enum;
using Common.Service.Provider.Model;
using System.Collections.Generic;

namespace Common.Service.Provider.Interfaces
{
    public interface IDataStore
    {
        IDictionary<string, IDictionary<INFORMATION_TYPE, LogData>> InformationTypeLogData { get; }
        IDictionary<string, int> ApiIdCounts { get; }

        void AddUpdateInformationTypeLogData(LogData logData);
    }
}
