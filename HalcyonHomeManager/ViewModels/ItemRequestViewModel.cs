using HalcyonHomeManager.Entities;
using HalcyonHomeManager.Models;
using HalcyonHomeManager.Views;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Input;


namespace HalcyonHomeManager.ViewModels
{
    //[QueryProperty(nameof(RequestItem), nameof(RequestItem))]
    public class ItemRequestViewModel : BaseViewModel
    {

        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }

        public Command OnRefreshCommand { get; }

        public Command<RequestItemResponse> ItemTapped { get; }

        public ICommand CompleteCommand { get; private set; }

        public ItemRequestViewModel()
        {

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnRefreshCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(OnAddItem);

            //ItemTapped = new Command<RequestItemResponse>(OnItemChecked);

            CompleteCommand = new Command((obj) =>
            {
                OnItemChecked(obj);
            });

        }

        async void OnItemChecked(object obj)
        {
            try
            {
                RequestItems item = (RequestItems)obj;
                RequestItems request = new RequestItems();

                request.DesiredDate = item.DesiredDate;
                request.IsFulfilled = 1;
                request.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
               // await _transactionServices.CreateRequestItem(request);
                await ExecuteLoadItemsCommand();
            }
            catch (Exception ex)
            {
                ErrorLog error = Helpers.ReturnErrorMessage(ex, "ItemRequestViewModel", "OnItemChecked");
                App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
              //  RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnAppearing()
        {
           // RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }


        private List<RequestItems> _requestItems;
        public List<RequestItems> RequestItems
        {
            get => _requestItems;
            set => SetProperty(ref _requestItems, value);
        }

        //private string _cardTitle;

        //public string CardTitle
        //{
        //    get { return _cardTitle; }
        //    set { _cardTitle = value; }
        //}


    }
}
