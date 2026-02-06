using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using Newtonsoft.Json;

namespace HalcyonHomeManager.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private ITransactionManager _transactionServices;

        public NewItemViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;
            RequestedDate = DateTime.Now;
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(_name);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _description;
        public DateTime RequestedDate
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            try
            {
                RequestItems requestItemRequest = new RequestItems();
                requestItemRequest.DesiredDate = RequestedDate;
                requestItemRequest.Title = Name;
                requestItemRequest.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();

                _transactionServices.CreateOrUpdateRequestItem(requestItemRequest);

                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "NewItemViewModel", "OnSave");
                _transactionServices.CreateNewError(error);
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }
    }
}
