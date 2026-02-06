using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HalcyonHomeManager.ViewModels
{
    [QueryProperty(nameof(WorkTask), nameof(WorkTask))]
    public class WorkTaskViewModel : BaseViewModel
    {
        private ITransactionManager _transactionServices;
        public WorkTaskViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;
            HouseHoldMembers = new List<string>();
            DeviceFontSize = Helpers.ReturnDeviceFontSize();
            CancelCommand = new Command(OnCancel);


            SaveCommand = new Command((obj) =>
            {
                OnSave(obj);
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });

            CompleteCommand = new Command((obj) =>
            {
                OnComplete(obj);
            });


            ReselectStateColorCommand = new Command((name) =>
            {
                Picker picker = (Picker)name;
                StateColor = Helpers.SetStateColor(picker.SelectedItem.ToString());
            });

            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            BuildDropDownLists();
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
                LoadItemId(value);
            }
        }

        public async void LoadItemId(WorkTask rawWorkTask)
        {
            try
            {
                HouseHoldMembers = await GetHouseHold();
                SelectedWorkTask = rawWorkTask;
                if (System.String.IsNullOrEmpty(SelectedWorkTask.Assignment))
                {
                    SelectedWorkTask.Assignment = "N/A";
                }
                else
                {
                    SelectedWorkTask.Assignment = SelectedWorkTask.Assignment;
                }

                if (SelectedWorkTask.ID == 0)
                {
                    Name = $"Create a New Work Task";
                    ShowDeleteButton = false;
                }
                else
                {
                    Name = $"Edit WorkTask: {SelectedWorkTask.Title}";
                    ShowDeleteButton = true;
                }

                StateColor = Helpers.SetStateColor(SelectedWorkTask.State);


            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "LoadItemId");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private WorkTask _selectedWorkTask;
        public WorkTask SelectedWorkTask
        {
            get => _selectedWorkTask;
            set => SetProperty(ref _selectedWorkTask, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private List<int> _priorityList;
        public List<int> PriorityList
        {
            get => _priorityList;
            set => SetProperty(ref _priorityList, value);
        }

        private List<string> _riskList;
        public List<string> RiskList
        {
            get => _riskList;
            set => SetProperty(ref _riskList, value);
        }

        private Microsoft.Maui.Graphics.Color _stateColor;
        public Microsoft.Maui.Graphics.Color StateColor
        {
            get => _stateColor;
            set => SetProperty(ref _stateColor, value);
        }

        private List<int> _effortList;
        public List<int> EffortList
        {
            get => _effortList;
            set => SetProperty(ref _effortList, value);
        }

        private List<string> _stateList;
        public List<string> StateList
        {
            get => _stateList;
            set => SetProperty(ref _stateList, value);
        }

        private List<string> _houseHoldMembers;
        public List<string> HouseHoldMembers
        {
            get => _houseHoldMembers;
            set => SetProperty(ref _houseHoldMembers, value);
        }

        private List<HouseHoldMember> _houseHoldMembersList;
        public List<HouseHoldMember> HouseHoldMembersList
        {
            get => _houseHoldMembersList;
            set => SetProperty(ref _houseHoldMembersList, value);
        }

        private bool _showDeleteButton;
        public bool ShowDeleteButton
        {
            get => _showDeleteButton;
            set => SetProperty(ref _showDeleteButton, value);
        }



        public async Task<List<string>> GetHouseHold()
        {
            List<string> newList = new List<string>();

            try
            {
                List<HouseHoldMember> RawHouseHoldMembersList;
                RawHouseHoldMembersList = await _transactionServices.GetHouseHoldMembers();
                HouseHoldMembersList = RawHouseHoldMembersList.ToList();
                if (RawHouseHoldMembersList.Count() != 0)
                {
                    foreach (var member in RawHouseHoldMembersList)
                    {
                        newList.Add(member.Name.Trim());
                    }
                }
                newList.Add("N/A");
                return newList.Order().ToList();
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "GetHouseHold");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                return newList;
            }
        }




        public void BuildDropDownLists()
        {

            StateList = new List<string>
            {
                "New",
                "In Progress",
                "Done"
            };


            PriorityList = new List<int>
            { 1, 2, 3, 4};

            EffortList = new List<int>
            { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            RiskList = new List<string>
            {
                "1 - High",
                "2 - Medium",
                "3 - Low"
            };

        }

        public Command SaveCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }
        public Command CompleteCommand { get; }

        public Command ReselectStateColorCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                        WorkTask workTask = rawWorkTaskViewModel.SelectedWorkTask;
                        workTask.Completed = 0;
                        workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        _transactionServices.CreateOrUpdateWorkTask(workTask);
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnDelete");
                        _transactionServices.CreateNewError(error);
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }
            }));
        }

        private void OnComplete(object obj)
        {
            App._alertSvc.ShowConfirmation("Warning", "Are you sure you want to complete this worktask?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                        WorkTask workTask = rawWorkTaskViewModel.SelectedWorkTask;
                        workTask.Completed = 1;
                        workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                        _transactionServices.CreateOrUpdateWorkTask(workTask);
                        await Shell.Current.GoToAsync("..");
                    }
                    catch (Exception ex)
                    {
                        ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnComplete");
                        _transactionServices.CreateNewError(error);
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }

            }));
        }


        private async void OnSave(object obj)
        {
            try
            {
                WorkTaskViewModel rawWorkTaskViewModel = (WorkTaskViewModel)obj;
                var bb = rawWorkTaskViewModel.HouseHoldMembers;

                WorkTask workTask = rawWorkTaskViewModel.SelectedWorkTask;
                workTask.Assignment = System.String.IsNullOrEmpty(workTask.Assignment) ? "N/A" : workTask.Assignment;

                string selName = string.Empty;
                if (workTask.Assignment == "N/A")
                {
                    selName = "N/A";
                }
                else
                {
                    //var pog = bb[Convert.ToInt32(workTask.Assignment.Trim())];
                    selName = bb.Where(p => p == workTask.Assignment.Trim()).FirstOrDefault();
                    if (string.IsNullOrEmpty(selName))
                    {
                        selName = "N/A";
                    }
                }

                if (HouseHoldMembersList.Count != 0)
                {
                    if (selName != null)
                    {
                        workTask.Assignment = selName;
                    }

                }
                workTask.Completed = 0;
                workTask.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
                _transactionServices.CreateOrUpdateWorkTask(workTask);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "WorkTaskViewModel", "OnSave");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
