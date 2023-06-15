using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.MasterHistory)]
public partial class RequestHistoryMasterViewModel : BaseViewModel
{
    private readonly AuthorizedUserStore _authorizedUserStore;
    private readonly INavigationService<BaseViewModel> _navigationService;
    private readonly IRequestService _requestService;

    [ObservableProperty]
    private ObservableCollection<RepairRequest> _closedRequests = null!;

    public RelayCommand<int> GoToRequestCommand { get; set; }

    public RequestHistoryMasterViewModel(INavigationService<BaseViewModel> navigationService,
        IRequestService requestService,
        AuthorizedUserStore authorizedUserStore)
    {
        ViewModelTitle = "Завершенные заявки";
        _navigationService = navigationService;
        _requestService = requestService;

        GoToRequestCommand = new RelayCommand<int>(x => OpenRequest(x), _ => true);
        _authorizedUserStore = authorizedUserStore;
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        GetMasterRequests();
        return base.OnNavigatedTo(args);
    }


    private void GetMasterRequests()
    {
        var result = _requestService.ListArchiveForMaster(_authorizedUserStore.AuthorizedUser!.Id);
        result.IfSucc(x =>
        {
            ClosedRequests = new ObservableCollection<RepairRequest>(x);
        });
        result.IfFail(PushErrorToSnackbar);
    }

    private void OpenRequest(int id)
    {
        _navigationService.Navigate(Routes.RequestForMaster,
            parameters: new DynamicDictionary((nameof(id), id)));
    }
}