using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InfoSpeed.Common.Service.Provider.Model;
using Common.Service.Provider.Enum;
using Common.Service.Provider.Interfaces;

namespace InfoSpeedTest.Controllers
{
    [Route("api/DataLog")]
    [ApiController]
    public class DataLogController : ControllerBase
    {
        private readonly IDataManager dataManager;
        public DataLogController(IDataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        [Route("Summary")]
        [HttpGet]
        public Result GetDataLogSummaryCount()
        {
            try
            {
                var result = dataManager.GetSummary();
                return new Result(Status.Success, "Summary Details Fetched", result);
            }
            catch (Exception ex)
            {
                return new Result(Status.Failure, "Error occured while fetchibg summary");
            }
        }

        [Route("Details")]
        [HttpGet]
        public Result GetDataLogDetailsById(string id)
        {
            try
            {
                var result = dataManager.GetLogsbyId(id);
                return new Result(Status.Success, "Summary Details Fetched", result);
            }
            catch (Exception ex)
            {
                return new Result(Status.Failure, "Error occured while fetchibg data log details");
            }
        }
    }
}
