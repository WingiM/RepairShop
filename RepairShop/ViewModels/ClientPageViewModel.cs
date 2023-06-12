using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.ClientPage)]
public partial class ClientPageViewModel : BaseViewModel
{
    private readonly AuthorizedUserStore _authorizedUserStore;
    private readonly IRequestService _requestService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private ObservableCollection<RepairRequest> _repairRequests = null!;

    public ICommand CreateRequestCommand { get; set; }
    public ICommand SeeRequestHistoryCommand { get; set; }
    public RelayCommand<int> GoToRequestCommand { get; set; }

    public ClientPageViewModel(INavigationService navigationService, 
        AuthorizedUserStore authorizedUserStore, 
        IRequestService requestService)
    {
        ViewModelTitle = "Главная";
        _navigationService = navigationService;
        _authorizedUserStore = authorizedUserStore;
        _requestService = requestService;

        CreateRequestCommand = new RelayCommand(Console.WriteLine, () => true);
        SeeRequestHistoryCommand = new RelayCommand(Console.WriteLine, () => true);
        GoToRequestCommand = new RelayCommand<int>(OpenRequest, _ => true);
    }

    public override void OnNavigatedTo(NavigationArgs args)
    {
        GetClientRequests();
    }

    private void OpenRequest(int id)
    {

    }

    private void GetClientRequests()
    {
        var result = _requestService.ListForClient(_authorizedUserStore.AuthorizedUser!.Id);
        result.IfSucc(x => RepairRequests = new ObservableCollection<RepairRequest>(x));
        result.IfFail(PushErrorToSnackbar);
    }
}