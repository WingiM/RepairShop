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
    private readonly INavigationService<BaseViewModel> _navigationService;

    [ObservableProperty] private ObservableCollection<RepairRequest> _repairRequests = null!;

    public ICommand CreateRequestCommand { get; set; }
    public ICommand SeeRequestHistoryCommand { get; set; }
    public RelayCommand<int> GoToRequestCommand { get; set; }

    public ClientPageViewModel(INavigationService<BaseViewModel> navigationService,
        AuthorizedUserStore authorizedUserStore,
        IRequestService requestService)
    {
        ViewModelTitle = "Главная";
        _navigationService = navigationService;
        _authorizedUserStore = authorizedUserStore;
        _requestService = requestService;

        CreateRequestCommand = new RelayCommand(() => OpenRequest(default, true), () => true);
        SeeRequestHistoryCommand = new RelayCommand(Console.WriteLine, () => true);
        GoToRequestCommand = new RelayCommand<int>(x => OpenRequest(x, false), _ => true);
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        GetClientRequests();
        return base.OnNavigatedTo(args);
    }

    private void OpenRequest(int id, bool isCreate)
    {
        _navigationService.Navigate(Routes.Request,
            parameters: new DynamicDictionary((nameof(id), id), (nameof(isCreate), isCreate)));
    }

    private void GetClientRequests()
    {
        var result = _requestService.ListForClient(_authorizedUserStore.AuthorizedUser!.Id);
        result.IfSucc(x => RepairRequests = new ObservableCollection<RepairRequest>(x));
        result.IfFail(PushErrorToSnackbar);
    }
}