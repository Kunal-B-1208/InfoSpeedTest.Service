using Common.Service.Provider.Model;
using System.Collections.Generic;

namespace Common.Service.Provider.Interfaces
{
    public interface IDataManager
    {
        void LoadLogDataFromFile(string[] filePaths);
        dynamic GetSummary();
        dynamic GetLogsbyId(string Id);
    }
}
