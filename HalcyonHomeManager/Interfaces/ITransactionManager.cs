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
        void CreateProject(Project project);
        void DeleteProject(Project project);
        void DeleteWorkTask(WorkTask workTask);
        void CreateWorkTask(WorkTask workTask);
        Task<List<HouseHoldMember>> GetHouseHoldList();
        Task<DashBoard> GetDashBoardData();
        Task<List<Project>> GetProjectList();
        Task<List<ProjectHierarchy>> GetWorkItemHierarchy();
        Task<List<WorkTask>> GetWorkTaskPrioritiesList();
    }
}