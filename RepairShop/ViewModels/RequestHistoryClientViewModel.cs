using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.Data.Models;
using RepairShop.Stores;
using RepairShop.ViewModels.Base;
using System.Collections.ObjectModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RepairShop.ViewModels;

[Route(Route = Routes.ClientHistory)]
public partial class RequestHistoryClientViewModel : BaseViewModel
{
    private readonly AuthorizedUserStore _authorizedUserStore;
    private readonly INavigationService<BaseViewModel> _navigationService;
    private readonly IRequestService _requestService;

    [ObservableProperty]
    private ObservableCollection<RepairRequest> _closedRequests = null!;

    public RelayCommand<int> GoToRequestCommand { get; set; }

    public RequestHistoryClientViewModel(INavigationService<BaseViewModel> navigationService,
        IRequestService requestService,
        AuthorizedUserStore authorizedUserStore)
    {
        ViewModelTitle = "Завершенные заявки";
        _navigationService = navigationService;
        _requestService = requestService;

        GoToRequestCommand = new RelayCommand<int>(x => OpenRequest(x, false), _ => true);
        _authorizedUserStore = authorizedUserStore;
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        GetClientRequests();
        return base.OnNavigatedTo(args);
    }


    private void GetClientRequests()
    {
        var result = _requestService.ListArchiveForClient(_authorizedUserStore.AuthorizedUser!.Id);
        result.IfSucc(x =>
        {
            ClosedRequests = new ObservableCollection<RepairRequest>(x);
        });
        result.IfFail(PushErrorToSnackbar);
    }

    private void OpenRequest(int id, bool isCreate)
    {
        _navigationService.Navigate(Routes.Request,
            parameters: new DynamicDictionary((nameof(id), id), (nameof(isCreate), isCreate)));
    }
}
