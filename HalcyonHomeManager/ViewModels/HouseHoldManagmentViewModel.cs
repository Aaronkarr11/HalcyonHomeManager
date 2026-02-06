using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using HalcyonHomeManager.Models;
using Newtonsoft.Json;


namespace HalcyonHomeManager.ViewModels
{
    public class HouseHoldManagmentViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; }
        public Command AddMemberCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItems> ItemTapped { get; }

        public Command DeleteCommand { get; }
        public Command EditCommand { get; }
        public Command ExecuteNewMemberCommand { get; }

        private ITransactionManager _transactionServices;

        public HouseHoldManagmentViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());


            ExecuteNewMemberCommand = new Command(() =>
            {
                ExecuteNewMember();
            });

            EditCommand = new Command((sender) =>
            {
                ExecuteEditHouseHoldCommand(sender);
            });

            DeleteCommand = new Command((obj) =>
            {
                OnDelete(obj);
            });


        }

        async void ExecuteNewMember()
        {
            HouseHoldMember member = new HouseHoldMember();
            var navigationParameter = new Dictionary<string, object>
                    {
                            { "Member", member }
                    };
            await Shell.Current.GoToAsync($"HouseHoldMemberPage", navigationParameter);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {
                HouseHoldList = await _transactionServices.GetHouseHoldMembers();
                if (HouseHoldList.Count() == 0)
                {
                    ShowMessage = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "ExecuteLoadItemsCommand");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
            IsBusy = true;
            ShowMessage = false;

            try
            {
                HouseHoldList = await _transactionServices.GetHouseHoldMembers();
                if (HouseHoldList.Count() == 0 || HouseHoldList == null)
                {
                    ShowMessage = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "OnAppearing");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async void ExecuteEditHouseHoldCommand(object sender)
        {
            try
            {
                var houseHold = (HouseHoldMember)sender;
                HouseHoldMember sentHouseHold = new HouseHoldMember
                {
                    ID = houseHold.ID,
                    Name = houseHold.Name,
                    Email = houseHold.Email,
                    PhoneNumber = houseHold.PhoneNumber.RemoveSpecialCharacters()
                };
                var navigationParameter = new Dictionary<string, object>
                    {
                            { "Member", sentHouseHold }
                    };
                await Shell.Current.GoToAsync($"HouseHoldMemberPage", navigationParameter);
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "ExecuteEditHouseHoldCommand");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private void OnDelete(object obj)
        {

            App._alertSvc.ShowConfirmation("Error", "Are you sure you want to delete?", (async result =>
            {
                if (result)
                {
                    try
                    {
                        HouseHoldMember member = (HouseHoldMember)obj;
                        _transactionServices.DeleteHouseHoldMember(member);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldManagmentViewModel", "OnDelete");
                        _transactionServices.CreateNewError(error);
                        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
                    }
                }

            }));
        }


        private List<HouseHoldMember> _houseHoldList;
        public List<HouseHoldMember> HouseHoldList
        {
            get => _houseHoldList;
            set => SetProperty(ref _houseHoldList, value);
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
        }



        private bool _showMessage;
        public bool ShowMessage
        {
            get => _showMessage;
            set => SetProperty(ref _showMessage, value);
        }


    }
}
