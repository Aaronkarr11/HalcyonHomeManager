using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using Newtonsoft.Json;
using System.Windows.Input;

namespace HalcyonHomeManager.ViewModels
{
    public class WorkItemManagmentViewModel : BaseViewModel
    {
        private ITransactionManager _transactionServices;

        private Project _selectedProject;
        public Command LoadWorkItemCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItems> ItemTapped { get; }

        public ICommand GetSelectedProjectsCommand { get; private set; }

        public Command EditWorkTaskCommand { get; private set; }

        public Command EditProjectCommand { get; private set; }
        public Command NewProjectCommand { get; private set; }
        public Command NewWorkTaskCommand { get; private set; }
        public Command EditOperationCommand { get; private set; }

        public WorkItemManagmentViewModel(ITransactionManager transactionServices)
        {
            ShowPicker = false;
            _transactionServices = transactionServices;
            _selectedProject = null;
            ProjectList = new List<Project>();
            ProjectHierarchy = new List<ProjectHierarchy>();
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
                GetWorkTaskHierarchy((Project)picker.SelectedItem);
            });
        }

        async void GetWorkTaskHierarchy(Project item)
        {
            try
            {
                if (item != null)
                {
                    if (item.ID == 0)
                    {
                        ExecuteNewProjectCommand();
                    }
                    else
                    {
                        _selectedProject = item;
                        var result = await _transactionServices.GetProjectHierarchy();
                        if (result == null)
                        {
                            throw new Exception("Could not build work item hierarchy!");
                        }
                        ProjectHierarchy = result.Where(e => e.ProjectID == item.ID).ToList();

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
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "GetWorkTaskHierarchy");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {
            try
            {
                var workTask = (WorkTask)sender;
                WorkTask WorkTask = new WorkTask
                {
                    ID = workTask.ID,
                    ProjectReferenceID = workTask.ProjectReferenceID,
                    Title = workTask.Title,
                    Assignment = workTask.Assignment,
                    Risk = workTask.Risk ?? "3 - Low",
                    SendSMS = workTask.SendSMS,
                    State = workTask.State ?? "New",
                    Effort = workTask.Effort == 0 ? 1 : workTask.Effort,
                    Priority = workTask.Priority == 0 ? 1 : workTask.Priority,
                    StartDate = workTask.StartDate,
                    TargetDate = workTask.TargetDate,
                    Description = workTask.Description,
                    Completed = 0
                };
                var result = JsonConvert.SerializeObject(WorkTask);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", WorkTask }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteEditWorkTaskCommand");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteNewWorkTaskCommand(object sender)
        {
            try
            {
                var parentProject = (ProjectHierarchy)sender;

                WorkTask WorkTask = new WorkTask
                {
                    ProjectReferenceID = parentProject.ProjectID,
                    State = "New",
                    Risk = "3 - Low",
                    SendSMS = false,
                    Effort = 1,
                    Priority = 4,
                    StartDate = DateTime.Now,
                    TargetDate = DateTime.Now,
                    Completed = 0
                };
                var result = JsonConvert.SerializeObject(WorkTask);
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", WorkTask }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteNewWorkTaskCommand");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }


        async void ExecuteNewProjectCommand()
        {
            _selectedProject = null;
            try
            {
                Project Project = new Project
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
                            { "Project", Project }
                    };
                await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteNewProjectCommand");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditProjectCommand(object sender)
        {
            try
            {
                var prog = (ProjectHierarchy)sender;
                Project Project = new Project
                {
                    ID = prog.ProjectID,
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
                            { "Project", Project }
                    };
                await Shell.Current.GoToAsync($"ProjectPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "ExecuteEditProjectCommand");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        public async void OnAppearing()
        {
            try
            {
                //List<Project> projList = new List<Project>();
                List<Project> projList = await _transactionServices.GetProjectList();
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
                        _selectedProject = ProjectList.Where(i => i?.ConvertedDateTimeStamp != 0).LastOrDefault();
                        if (_selectedProject == null)
                        {
                            _selectedProject = ProjectList.Where(h => h.Title != "Create New").FirstOrDefault();
                        }
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
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkItemManagmentViewModel", "OnAppearing");
                _transactionServices.CreateNewError(error);
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


        private List<Project> _projectList;
        public List<Project> ProjectList
        {
            get => _projectList;
            set => SetProperty(ref _projectList, value);
        }


        private List<ProjectHierarchy> _projectHierarchy;
        public List<ProjectHierarchy> ProjectHierarchy
        {
            get => _projectHierarchy;
            set => SetProperty(ref _projectHierarchy, value);
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
