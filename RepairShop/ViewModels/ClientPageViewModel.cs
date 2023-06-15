﻿using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
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
    [ObservableProperty] private ObservableCollection<RequestStatus> _statuses = null!;

    private RequestStatus _selectedStatusFilter;

    public RequestStatus SelectedStatusFilter
    {
        get => _selectedStatusFilter;
        set
        {
            _selectedStatusFilter = value;
            GetClientRequests();
        }
    }

    public ICommand CreateRequestCommand { get; set; }
    public ICommand SeeRequestHistoryCommand { get; set; }
    public RelayCommand<int> GoToRequestCommand { get; set; }

    public ClientPageViewModel(INavigationService<BaseViewModel> navigationService,
        AuthorizedUserStore authorizedUserStore,
        IRequestService requestService,
        ApplicationContext context)
    {
        ViewModelTitle = "Главная";
        _navigationService = navigationService;
        _authorizedUserStore = authorizedUserStore;
        _requestService = requestService;

        var statuses = context.RequestStatuses
            .Where(x => x.Id != (int)RequestStatuses.Finished)
            .OrderBy(x => x.Id)
            .ToList();
        var newStatus = new RequestStatus { Id = 0, Name = "Любой" };
        statuses.Add(newStatus);
        _selectedStatusFilter = newStatus;

        Statuses = new ObservableCollection<RequestStatus>(statuses);
        CreateRequestCommand = new RelayCommand(() => OpenRequest(default, true), () => true);
        SeeRequestHistoryCommand =
            new RelayCommand(() => _navigationService.Navigate(Routes.ClientHistory), () => true);
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
        result.IfSucc(x =>
        {
            Func<RepairRequest, bool> where = _selectedStatusFilter.Id == 0
                ? _ => true
                : z => z.ActualStatus!.Id == _selectedStatusFilter.Id;
            RepairRequests = new ObservableCollection<RepairRequest>(x.Where(where).ToList());
        });
        result.IfFail(PushErrorToSnackbar);
    }
}