using HalcyonHomeManager.Entities;
using Newtonsoft.Json;

namespace HalcyonHomeManager.ViewModels
{
    [QueryProperty(nameof(WorkTask), nameof(WorkTask))]
    public class WorkTaskPrioritiesViewModel : BaseViewModel
    {
        private WorkTask _selectedWorkTask;

        public Command EditWorkTaskCommand { get; private set; }

        public WorkTaskPrioritiesViewModel()
        {
            DeviceFontSize = Helpers.ReturnDeviceFontSize();

            EditWorkTaskCommand = new Command((workTask) =>
            {
                ExecuteEditWorkTaskCommand(workTask);
            });
        }

        private WorkTask _workTask;
        public WorkTask WorkTask
        {
            get
            {
                return _workTask;
            }
            set
            {
                _workTask = value;
                ExecuteEditWorkTaskCommand(value);
            }
        }


        public async void OnAppearing()
        {
          IsBusy = true;
            try
            {
               // WorkTaskList = await _transactionServices.GetWorkTaskPrioritiesList(DeviceInfo.Name.RemoveSpecialCharacters());
                IsBusy = false;
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskPrioritiesViewModel", "OnAppearing");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }

        }

        async void ExecuteEditWorkTaskCommand(object sender)
        {
            var workTask = (WorkTask)sender;
            try
            {
                WorkTask WorkTask = new WorkTask
                {
                    Title = workTask.Title,
                    Assignment = workTask?.Assignment.Trim(),
                    Risk = workTask?.Risk ?? "3 - Low",
                    SendSMS = workTask.SendSMS,
                    State = workTask?.State,
                    Effort = workTask?.Effort == 0 ? 1 : workTask.Effort,
                    Priority = workTask?.Priority == 0 ? 1 : workTask.Priority,
                    StartDate = workTask.StartDate,
                    TargetDate = workTask.TargetDate,
                    Description = workTask?.Description,
                    Completed = 0
                };
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "WorkTask", WorkTask }
                    };
                await Shell.Current.GoToAsync($"WorkTaskPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskPrioritiesViewModel", "OnAppearing");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private List<WorkTask> _workTaskList;
        public List<WorkTask> WorkTaskList
        {
            get => _workTaskList;
            set => SetProperty(ref _workTaskList, value);
        }
    }
}
