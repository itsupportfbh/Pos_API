using UNITYPOS_API.Entities.Reports;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IReportRuntimeService
    {
        public Task<object> ExecuteAsync(ReportExecutionRequest request);
    }
}
