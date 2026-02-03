using HalcyonHomeManager.DataLayer;
using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.Models;
using System.Globalization;

namespace HalcyonHomeManager.BusinessLogic
{
    public class TransactionManager : ITransactionManager
    {
        WorkTaskDatabase _workTaskDatabase;
        ProjectDatabase _projectDatabase;
        ErrorLogDatabase _errorLogDatabase;
        public TransactionManager(WorkTaskDatabase workTaskDatabase, ProjectDatabase projectDatabase, ErrorLogDatabase errorLogDatabase)
        {
            _workTaskDatabase = workTaskDatabase;
            _projectDatabase = projectDatabase;
            _errorLogDatabase = errorLogDatabase;
        }


        public async Task<DashBoard> GetDashBoardData()
        {
            try
            {
                WorkTaskCompletedStats percentages = new WorkTaskCompletedStats();
                BarGraphModelItem barGraphModelItem = new BarGraphModelItem();
                List<LineGraphModelItem> lineGraphModel = new List<LineGraphModelItem>();

                var workTaskResultAsync = _workTaskDatabase.GetWorkTasksAsync();
                List<WorkTask> workTaskResult = await workTaskResultAsync;

                DateTime todayOfCurrentMonth = Convert.ToDateTime(DateTime.Now.LastDayInThisMonth());
                DateTime todayOfLastMonth = Convert.ToDateTime(DateTime.Now.AddMonths(-1).LastDayInThisMonth());

                List<WorkTask> workTaskList = new List<WorkTask>();

                // newResults = workTaskResult.Where(o => o.Timestamp >= todayOfLastMonth & o.Timestamp <= todayOfCurrentMonth).ToList();
                foreach (var workTask in workTaskResult.Where(i => (i.TimeStamp.ToString("yyyy")) == DateTime.Now.Year.ToString()))
                {
                    WorkTask workTaskModel = new WorkTask();
                    workTaskModel.Name = workTask.Assignment;
                    workTaskModel.Completed = workTask.Completed;
                    workTaskList.Add(workTaskModel);
                }
                percentages = CalculatePercents(workTaskList);
                lineGraphModel = ComputeLineGraphSeries(workTaskResult);
                barGraphModelItem = ComputeBarGraphSeries(workTaskResult);

                DashBoard dashBoard = new DashBoard();
                dashBoard.lineGraphModel = lineGraphModel;
                dashBoard.percentageData = percentages;
                dashBoard.barGraphData = barGraphModelItem;

                return dashBoard;
            }
            catch (Exception ex)
            {
                return new DashBoard();
            }
        }


        public async Task<List<ProjectHierarchy>> GetWorkItemHierarchy()
        {
            try
            {
                var workTaskResultAsync = _workTaskDatabase.GetWorkTasksAsync();
                List<WorkTask> workTaskResult = await workTaskResultAsync;

                var projectResultAsync = _projectDatabase.GetProjectsAsync();
                List<Project> projectResult = await projectResultAsync;

                List<ProjectHierarchy> ProjectList = new List<ProjectHierarchy>();
                List<WorkTask> WorkTaskList = new List<WorkTask>();


                foreach (var project in projectResult)
                {
                    ProjectHierarchy projectHierarchy = new ProjectHierarchy();

                    projectHierarchy.WorkTaskHierarchy = new List<WorkTask>();
                    projectHierarchy.Description = project.Description;
                    projectHierarchy.LocationCategory = project.LocationCategory;
                    projectHierarchy.Priority = project.Priority;
                    projectHierarchy.Severity = project.Severity;
                    projectHierarchy.StartDate = project.StartDate;
                    projectHierarchy.TargetDate = project.TargetDate;
                    projectHierarchy.CreatedDate = project.CreatedDate;
                    projectHierarchy.DisplayStartDate = Convert.ToDateTime(project.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    projectHierarchy.DisplayTargetDate = Convert.ToDateTime(project.TargetDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    projectHierarchy.State = project.State;
                    projectHierarchy.ConvertedDateTimeStamp = project.ConvertedDateTimeStamp;
                    projectHierarchy.Title = project.Title;
                    projectHierarchy.Completed = project.Completed;
                    projectHierarchy.DeviceName = project.DeviceName;

                    ProjectList.Add(projectHierarchy);
                    foreach (var worktask in workTaskResult.Where(t => t.ProjectReferenceID == project.ID && t.Completed != 1))
                    {
                        WorkTask workTask = new WorkTask();
                        workTask.ID = worktask.ID;
                        workTask.ProjectReferenceID = worktask.ProjectReferenceID;
                        workTask.Assignment = worktask.Assignment;
                        workTask.Description = worktask.Description;
                        workTask.Effort = worktask.Effort;
                        workTask.Priority = worktask.Priority;
                        workTask.Risk = worktask.Risk;
                        workTask.SendSMS = worktask.SendSMS;
                        workTask.DisplayStartDate = Convert.ToDateTime(worktask.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        workTask.DisplayTargetDate = Convert.ToDateTime(worktask.TargetDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                        workTask.StartDate = worktask.StartDate;
                        workTask.State = worktask.State;
                        workTask.StateColor = Helpers.SetStateColor(worktask.State);
                        workTask.TargetDate = worktask.TargetDate;
                        workTask.Title = worktask.Title;
                        workTask.Completed = worktask.Completed;
                        workTask.DeviceName = worktask.DeviceName;

                        projectHierarchy.WorkTaskHierarchy.Add(workTask);
                    }
                }

                return ProjectList;
            }
            catch (Exception ex)
            {
                return new List<ProjectHierarchy>();
            }
        }

        public async void CreateNewError(string message)
        {
            try
            {
                ErrorLog errorLog = new ErrorLog();
                errorLog.Message = message;
                await _errorLogDatabase.SaveErrorLogAsync(errorLog);
            }
            catch (Exception ex)
            {

            }
        }


        public async Task<List<Project>> GetProjectList()
        {
            try
            {
                var projectResultAsync = _projectDatabase.GetProjectsAsync();
                List<Project> projectResult = await projectResultAsync;

                Project emptyProject = new Project();
                emptyProject.Title = "Create New";
                emptyProject.ID = 0;
                projectResult.Add(emptyProject);
                return projectResult;
            }
            catch (Exception ex)
            {
                List<Project> emptyProjectList = new List<Project>();
                Project model = new Project();
                model.Title = "Create New";
                model.ID = 0;
                emptyProjectList.Add(model);
                return emptyProjectList;
            }
        }


        public async Task<List<WorkTask>> GetWorkTaskPrioritiesList()
        {
            try
            {
                var workTaskResultAsync = _workTaskDatabase.GetWorkTasksAsync();
                List<WorkTask> workTaskResult = await workTaskResultAsync;

                List<WorkTask> WorkTaskList = new List<WorkTask>();

                foreach (var worktask in workTaskResult.Where(t => t.Completed != 1))
                {
                    WorkTask workTask = new WorkTask();

                    workTask.Assignment = worktask.Assignment;
                    workTask.Description = worktask.Description;
                    workTask.Effort = worktask.Effort;
                    workTask.ID = worktask.ID;
                    workTask.ProjectReferenceID = worktask.ProjectReferenceID;
                    workTask.Priority = worktask.Priority;
                    workTask.Risk = worktask.Risk;
                    workTask.SendSMS = worktask.SendSMS;
                    workTask.StartDate = worktask.StartDate;
                    workTask.State = worktask.State;
                    workTask.TargetDate = worktask.TargetDate;
                    workTask.DisplayStartDate = Convert.ToDateTime(worktask.StartDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    workTask.DisplayTargetDate = Convert.ToDateTime(worktask.TargetDate).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    workTask.Title = worktask.Title;
                    workTask.Completed = worktask.Completed;
                    workTask.DeviceName = worktask.DeviceName;

                    WorkTaskList.Add(workTask);
                }

                return WorkTaskList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public WorkTaskCompletedStats CalculatePercents(List<WorkTask> returnedWorkItems)
        {
            WorkTaskCompletedStats assignmentPercent = new WorkTaskCompletedStats();

            try
            {
                int TotalCounts = returnedWorkItems.Count();
                int TotalCountCompleted = returnedWorkItems.Where(t => t.Completed == 1).Count();
                double completedPercent = Math.Round(((Convert.ToDouble(TotalCountCompleted) / Convert.ToDouble(TotalCounts)) * 100), 0);
                double unCompletedPercent = Convert.ToDouble((100 - completedPercent));
                assignmentPercent.percentCompleted = completedPercent;
                assignmentPercent.percentUnCompleted = unCompletedPercent;
                assignmentPercent.unCompletedCount = TotalCounts;
                assignmentPercent.completedCount = TotalCountCompleted;
                return assignmentPercent;
            }
            catch (Exception)
            {
                return assignmentPercent;
            }

        }

        public List<LineGraphModelItem> ComputeLineGraphSeries(IList<WorkTask> workTaskResult)
        {
            List<string> MonthList = new List<string>();
            MonthList.Add("Jan");
            MonthList.Add("Feb");
            MonthList.Add("Mar");
            MonthList.Add("Apr");
            MonthList.Add("May");
            MonthList.Add("Jun");
            MonthList.Add("Jul");
            MonthList.Add("Aug");
            MonthList.Add("Sep");
            MonthList.Add("Oct");
            MonthList.Add("Nov");
            MonthList.Add("Dec");

            List<WorkTask> WorkTaskList = new List<WorkTask>();

            List<LineGraphModelItem> graphModel = new List<LineGraphModelItem>();
            try
            {
                foreach (var month in MonthList)
                {
                    LineGraphModelItem graphModelItem = new LineGraphModelItem();
                    int counter = 0;
                    foreach (var task in workTaskResult.Where(o => o.Completed == 1).Where(i => (i.TimeStamp.ToString("yyyy")) == DateTime.Now.Year.ToString()).Where(p => p.TimeStamp.ToString("m").Substring(0, 3) == month))
                    {
                        counter++;
                    }
                    graphModelItem.TotalCompleted = counter;
                    graphModelItem.Name = month;
                    graphModel.Add(graphModelItem);
                }
                return graphModel;
            }
            catch (Exception)
            {

                return graphModel;
            }


        }

        public BarGraphModelItem ComputeBarGraphSeries(IList<WorkTask> workTaskResult)
        {
            BarGraphModelItem barGraphModelItem = new BarGraphModelItem();

            try
            {
                List<WorkTask> TotalCountCompleted = workTaskResult.Where(o => o.Completed == 1).Where(i => (i.TimeStamp.ToString("yyyy")) == DateTime.Now.Year.ToString()).ToList();

                barGraphModelItem.CompletedCountForLastMonth = TotalCountCompleted.Where(i => i.TimeStamp.ToString("MMMM") == DateTime.Now.AddMonths(-1).ToString("MMMM")).Count();
                barGraphModelItem.CompletedCountForCurrentMonth = TotalCountCompleted.Where(i => i.TimeStamp.ToString("MMMM") == DateTime.Now.ToString("MMMM")).Count();
                barGraphModelItem.CurrentMonth = DateTime.Now.ToString("MMMM");
                barGraphModelItem.LastMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");
                return barGraphModelItem;
            }
            catch (Exception)
            {
                return barGraphModelItem;
            }
        }


    }
}
