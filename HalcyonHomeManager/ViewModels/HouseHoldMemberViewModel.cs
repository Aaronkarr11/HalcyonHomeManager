using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using Newtonsoft.Json;

namespace HalcyonHomeManager.ViewModels
{
    [QueryProperty(nameof(Member), nameof(Member))]
    public class HouseHoldMemberViewModel : BaseViewModel
    {
        private ITransactionManager _transactionServices;

        public HouseHoldMemberViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;
            CancelCommand = new Command(OnCancel);

            SaveCommand = new Command((obj) =>
            {
                OnSave(obj);
            });


            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

        }

        private HouseHoldMember _member;
        public HouseHoldMember Member
        {
            get
            {
                return _member;
            }
            set
            {
                _member = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(HouseHoldMember mem)
        {
            try
            {
                if (mem.PhoneNumber == null)
                {
                    mem.PhoneNumber = "";
                }

                SelectedHouseHoldMember = mem;
                if (mem.ID == 0)
                {
                    PageName = $"Create a New HouseHold Member";
                    ShowDeleteButton = false;
                }
                else
                {
                    PageName = $"Edit HouseHold Member: {SelectedHouseHoldMember.Name}";
                    ShowDeleteButton = true;
                }
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "LoadItemId");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        private HouseHoldMember _selectedHouseHoldMember;
        public HouseHoldMember SelectedHouseHoldMember
        {
            get => _selectedHouseHoldMember;
            set => SetProperty(ref _selectedHouseHoldMember, value);
        }

        private string _pagename;
        public string PageName
        {
            get => _pagename;
            set => SetProperty(ref _pagename, value);
        }

        private bool _showDeleteButton;
        public bool ShowDeleteButton
        {
            get => _showDeleteButton;
            set => SetProperty(ref _showDeleteButton, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave(object obj)
        {

            try
            {
                HouseHoldMemberViewModel rawHouseHoldViewModel = (HouseHoldMemberViewModel)obj;
                HouseHoldMember houseHold = rawHouseHoldViewModel.SelectedHouseHoldMember;

                
                if (!Helpers.IsEmailValid(houseHold))
                {
                    App._alertSvc.ShowAlert("Warning!", "Provided email must valid");
                }
                else
                {
                    if (!Helpers.IsPhoneValid(houseHold))
                    {
                        App._alertSvc.ShowAlert("Warning!", "Phone must be valid and only contain 10 digits! e.g. (000)-000-0000)");
                    }
                    else
                    {
                        _transactionServices.CreateOrUpdateHouseHoldMember(houseHold);
                        await Shell.Current.GoToAsync("..");
                    }
                }




            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "HouseHoldMemberViewModel", "OnSave");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
