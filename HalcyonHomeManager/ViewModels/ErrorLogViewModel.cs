using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Interfaces;
using Newtonsoft.Json;

namespace HalcyonHomeManager.ViewModels
{
    public class ErrorLogViewModel: BaseViewModel
    {


        public Command ExecuteNewMemberCommand { get; }

        private ITransactionManager _transactionServices;

        public ErrorLogViewModel(ITransactionManager transactionServices)
        {
            _transactionServices = transactionServices;
            ExecuteNewMemberCommand = new Command(() =>
            {
                ExecuteNewMember();
            });

        }
    


        public async Task OnAppearing()
        {
            IsBusy = true;

            try
            {
                ErrorLogList = await _transactionServices.GetErrorLogs();

                if (ErrorLogList.Count == 0)
                {
                    ErrorPageTitle = "No Errors Found or Logged!";
                }
                else
                {
                    ErrorPageTitle = $"Showing The First {ErrorLogList.Count} Errors";
                }
            }
            catch (Exception ex)
            {
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        public async void ExecuteNewMember()
        {
            await Shell.Current.GoToAsync("..");
        }


        private List<ErrorLog> _errorLogList;
        public List<ErrorLog> ErrorLogList
        {
            get => _errorLogList;
            set => SetProperty(ref _errorLogList, value);
        }



        private string _errorPageTitle;
        public string ErrorPageTitle
        {
            get => _errorPageTitle;
            set => SetProperty(ref _errorPageTitle, value);
        }



    }
}
