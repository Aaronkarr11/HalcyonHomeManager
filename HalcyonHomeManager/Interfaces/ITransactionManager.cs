using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Models;

namespace HalcyonHomeManager.Interfaces
{
    public interface ITransactionManager
    {
        WorkTaskCompletedStats CalculatePercents(List<WorkTask> returnedWorkItems);
        BarGraphModelItem ComputeBarGraphSeries(IList<WorkTask> workTaskResult);
        List<LineGraphModelItem> ComputeLineGraphSeries(IList<WorkTask> workTaskResult);
        void CreateNewError(ErrorLog message);
        void CreateOrUpdateProject(Project project);
        void DeleteProject(Project project);
        void DeleteWorkTask(WorkTask workTask);
        void CreateOrUpdateWorkTask(WorkTask workTask);
        void CreateOrUpdateHouseHoldMember(HouseHoldMember member);
        void CreateOrUpdateRequestItem(RequestItems item);
		void DeleteRequestItem(RequestItems item);
        void DeleteHouseHoldMember(HouseHoldMember household);
        Task<List<ErrorLog>> GetErrorLogs();
		Task<List<RequestItems>> GetRequestItems();
		Task<List<HouseHoldMember>> GetHouseHoldMembers();
        Task<DashBoard> GetDashBoardData();
        Task<List<Project>> GetProjectList();
        Task<List<ProjectHierarchy>> GetProjectHierarchy();
        Task<List<WorkTask>> GetWorkTaskPrioritiesList();
    }
}