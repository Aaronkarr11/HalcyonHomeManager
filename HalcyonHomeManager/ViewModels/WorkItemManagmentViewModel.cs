using HalcyonHomeManager.Models;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HalcyonHomeManager.ViewModels
{
    public class WorkItemManagmentViewModel : BaseViewModel
    {
        private ProjectModel _selectedProject;
        public Command LoadWorkItemCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public ICommand GetSelectedProjectsCommand { get; private set; }

        public Command EditWorkTaskCommand { get; private set; }

        public Command EditProjectCommand { get; private set; }
        public Command NewProjectCommand { get; private set; }
        public Command NewWorkTaskCommand { get; private set; }
        public Command EditOperationCommand { get; private set; }

        public WorkItemManagmentViewModel()
        {
            ShowPicker = false;
            _selectedProject = null;
            ProjectList = new List<ProjectModel>();
            WorkItemHierarchy = new List<ProjectHierarchy>();
            DeviceFontSize = Helpers.ReturnDeviceFontSize();
            DeviceButtonWidth = Helpers.ReturnDeviceButtonWidth();

            EditWorkTaskCommand = new Command((workTask) =>
            {
                ExecuteEditWorkTaskCommand(workTask);
            });

            EditProjectCommand = new Command((project) =>
            {
                ExecuteEditProjectCommand(project);
            });


            NewWorkTaskCommand = new Command((sender) =>
            {
                ExecuteNewWorkTaskCommand(sender);
            });

            //EditOperationCommand = new Command((operation) =>
            //{
            //    ExecuteEditOperationCommand(operation);
            //});

            GetSelectedProjectsCommand = new Command((name) =>
            {
                Picker picker = (Picker)name;
                GetWorkTaskHierarchy((ProjectModel)picker.SelectedItem);
            });
        }

        async void GetWorkTaskHierarchy(ProjectModel item)
        {
            try
            {
                if (item != null)
                {
                    if (true)
                    {
                        ExecuteNewProjectCommand();
                    }
                    else
                    {
                        _selectedProject = item;
                       // var result = await _transactionServices.GetWorkItemHierarchy(DeviceInfo.Name.RemoveSpecialCharacters());
                       // WorkItemHierarchy = result.Where(e => e.PartitionKey == item.PartitionKey && e.RowKey == item.RowKey).ToList();

                        if (String.IsNullOrEmpty(SelectedProject))
                        {
                            SelectedProject = "Selected Project ";
                        }
                        else
                        {
                            SelectedProject = "Selected Project: " + item.Title;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "GetWorkTaskHierarchy");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {
            try
            {
                var workTask = (WorkTaskModel)sender;
                WorkTaskModel workTaskModel = new WorkTaskModel
                {
                    Title = workTask.Title,
                    Assignment = workTask.Assignment,
                    Risk = workTask.Risk ?? "3 - Low",
                    SendSMS = workTask.SendSMS,
                    RowKey = workTask.RowKey,
                    State = workTask.State ?? "New",
                    PartitionKey = workTask.PartitionKey,
                    Effort = workTask.Effort == 0 ? 1 : workTask.Effort,
                    ParentPartitionKey = workTask.ParentPartitionKey,
                    ParentRowKey = workTask.ParentRowKey,
                    Priority = workTask.Priority == 0 ? 1 : workTask.Priority,
                    StartDate = workTask.StartDate,
                    TargetDate = workTask.TargetDate,
                    Description = workTask.Description,
                    Completed = 0
                };
                var result = JsonConvert.SerializeObject(workTaskModel);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", workTaskModel }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteEditWorkTaskCommand");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteNewWorkTaskCommand(object sender)
        {
            try
            {
                var parentProject = (ProjectHierarchy)sender;

                WorkTaskModel workTaskModel = new WorkTaskModel
                {
                    ParentPartitionKey = parentProject.PartitionKey,
                    ParentRowKey = parentProject.RowKey,
                    State = "New",
                    Risk = "3 - Low",
                    SendSMS = false,
                    Effort = 1,
                    Priority = 4,
                    StartDate = DateTime.Now,
                    TargetDate = DateTime.Now,
                    Completed = 0
                };
                var result = JsonConvert.SerializeObject(workTaskModel);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", workTaskModel }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteNewWorkTaskCommand");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }


        async void ExecuteNewProjectCommand()
        {
            _selectedProject = null;
            try
            {
                ProjectModel projectModel = new ProjectModel
                {
                    StartDate = DateTime.Now,
                    TargetDate = DateTime.Now,
                    Severity = "4 - Low",
                    State = "New",
                    LocationCategory = "Whole House",
                    Priority = 1,
                    Completed = 0
                };

                var navigationParameter = new Dictionary<string, object>
                    {
                            { "Project", projectModel }
                    };
                await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteNewProjectCommand");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditProjectCommand(object sender)
        {
            try
            {
                var prog = (ProjectHierarchy)sender;
                ProjectModel projectModel = new ProjectModel
                {
                    Title = prog.Title,
                    Severity = prog.Severity ?? "4 - Low",
                    State = prog.State,
                    LocationCategory = prog.LocationCategory ?? "Whole House",
                    Priority = prog.Priority == 0 ? 1 : prog.Priority,
                    StartDate = prog.StartDate,
                    TargetDate = prog.TargetDate,
                    Description = prog.Description,
                    Completed = 0
                };
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "Project", projectModel }
                    };
                await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteEditProjectCommand");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        public async void OnAppearing()
        {
            try
            {
                List<ProjectModel> projList = new List<ProjectModel>();
                //  List<ProjectModel> projList = await _transactionServices.GetProjectList(DeviceInfo.Name.RemoveSpecialCharacters());
                IsBusy = true;
                if (_selectedProject != null)
                {
                    ProjectList = projList.OrderBy(p => p.ConvertedDateTimeStamp).ToList();
                    PickerTitle = _selectedProject.Title;
                    GetWorkTaskHierarchy(_selectedProject);
                }
                else
                {

                    ProjectList = projList.OrderBy(p => p.ConvertedDateTimeStamp).ToList();
                    if (ProjectList.Count() > 1)
                    {
                        _selectedProject = ProjectList.Where(i => i.ConvertedDateTimeStamp != 0).LastOrDefault();
                        PickerTitle = _selectedProject.Title;
                        GetWorkTaskHierarchy(_selectedProject);
                    }
                }

                if (String.IsNullOrEmpty(SelectedProject))
                {
                    SelectedProject = "Selected Project ";
                }
                else
                {
                    SelectedProject = "Selected Project: " + _selectedProject?.Title;
                }
                ShowPicker = true;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                ErrorLogModel error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "OnAppearing");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private string _projectTitle;
        public string ProjectTitle
        {
            get => _projectTitle;
            set => SetProperty(ref _projectTitle, value);
        }

        private string _pickerTitle;
        public string PickerTitle
        {
            get => _pickerTitle;
            set => SetProperty(ref _pickerTitle, value);
        }


        private List<ProjectModel> _projectList;
        public List<ProjectModel> ProjectList
        {
            get => _projectList;
            set => SetProperty(ref _projectList, value);
        }


        private List<ProjectHierarchy> _workItemHierarchy;
        public List<ProjectHierarchy> WorkItemHierarchy
        {
            get => _workItemHierarchy;
            set => SetProperty(ref _workItemHierarchy, value);
        }

        private bool _showPicker;
        public bool ShowPicker
        {
            get => _showPicker;
            set => SetProperty(ref _showPicker, value);
        }

        private double _deviceButtonWidth;
        public double DeviceButtonWidth
        {
            get => _deviceButtonWidth;
            set => SetProperty(ref _deviceButtonWidth, value);
        }

        private string _retainedSelectedProject;
        public string SelectedProject
        {
            get => _retainedSelectedProject;
            set => SetProperty(ref _retainedSelectedProject, value);
        }


    }
}
