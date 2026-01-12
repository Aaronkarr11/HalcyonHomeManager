using HalcyonHomeManager.Entities;

namespace HalcyonHomeManager.ViewModels
{
    //[QueryProperty(nameof(RequestItem), nameof(RequestItem))]
    public class WMScheduleViewModel : BaseViewModel
    {

        //public Command LoadItemsCommand { get; }


        public WMScheduleViewModel()
        {
            //  LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }



        //async void OnItemChecked(object obj)
        //{
        //    try
        //    {
        //        RequestItemsModel item = (RequestItemsModel)obj;

        //        RequestItemsTableTemplate request = new RequestItemsTableTemplate();
        //        request.PartitionKey = item.PartitionKey;
        //        request.RowKey = item.RowKey;
        //        request.DesiredDate = item.DesiredDate;
        //        request.IsFulfilled = 1;
        //        request.DeviceName = DeviceInfo.Name.RemoveSpecialCharacters();
        //        await _transactionServices.CreateRequestItem(request);
        //        await ExecuteLoadItemsCommand();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLog error = Helpers.ReturnErrorMessage(ex, "ItemRequestViewModel", "OnItemChecked");
        //        App._alertSvc.ShowAlert("Exception!", $"{ex.Message}");
        //    }
        //}



        public async Task OnAppearing()
        {
            //RequestItems = await _transactionServices.GetRequestItems(DeviceInfo.Name.RemoveSpecialCharacters());
            IsBusy = true;
        }


        private List<RequestItems> _requestItems;
        public List<RequestItems> RequestItems
        {
            get => _requestItems;
            set => SetProperty(ref _requestItems, value);
        }



    }
}
