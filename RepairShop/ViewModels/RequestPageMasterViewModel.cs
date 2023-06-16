using System.Collections.ObjectModel;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.RequestForMaster)]
public partial class RequestPageMasterViewModel : BaseViewModel
{
    public static string SaveButtonTooltipIfNotEditable => ValidationErrorMessages.CannotEditRepairRequest;
    public static string TitleTooltip => "Короткое название для запроса";
    public static string DescriptionTooltip => "Подробное описание проблемы";

    private readonly INavigationService<BaseViewModel> _navigationService;
    private readonly IRequestService _requestService;
    private readonly AuthorizedUserStore _authorizedUserStore;

    private RepairRequest? _request;

    [ObservableProperty] private int _id;

    [ObservableProperty] private string _title = string.Empty;

    [ObservableProperty] private string _description = string.Empty;

    [ObservableProperty] private RequestStatus _status = null!;

    [ObservableProperty] private string? _clientName;

    [ObservableProperty] private ObservableCollection<StatusHistory> _statusHistory = null!;
    
    [ObservableProperty] private bool _isTaken;
    
    [ObservableProperty] private bool _isArchive;

    [ObservableProperty] private ObservableCollection<RequestStatus> _statuses;

    [ObservableProperty] private RequestStatus _selectedStatus = null!;

    public RelayCommand ChangeStatusCommand { get; set; }
    public RelayCommand TakeRequestCommand { get; set; }

    public RequestPageMasterViewModel(INavigationService<BaseViewModel> navigationService,
        IRequestService requestService,
        AuthorizedUserStore authorizedUserStore,
        ApplicationContext context)
    {
        ViewModelTitle = "Запрос на ремонт";
        _navigationService = navigationService;
        _requestService = requestService;
        _authorizedUserStore = authorizedUserStore;
        _statuses = new ObservableCollection<RequestStatus>(context.RequestStatuses
            .Where(
                x => !new[] { (int)RequestStatuses.AwaitsConfirmation }.Contains(x.Id))
            .OrderBy(x => x.Id).ToList());

        ChangeStatusCommand =
            new RelayCommand(() => ChangeRequestStatus(), () => IsTaken);
        TakeRequestCommand =
            new RelayCommand(() => TakeRequest(), () => !IsTaken);
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        var requestId = args.Parameters.GetValue<int>("id");

        var result = _requestService.GetRequestById(requestId);
        if (result.IsFaulted)
        {
            result.IfFail(PushErrorToSnackbar);
            return base.OnNavigatedTo(args);
        }

        result.IfSucc(req =>
        {
            if (req is null)
            {
                return;
            }

            _request = req;
            ViewModelTitle += $" № {req.Id}";
            Id = req.Id;
            Title = req.ShortName;
            Description = req.Description;
            StatusHistory = new ObservableCollection<StatusHistory>(req.StatusHistories);
            Status = req.ActualStatus!;
            SelectedStatus = Statuses.FirstOrDefault(x => x.Id == Status.Id)
                             ?? Statuses.First(x => x.Id == (int)RequestStatuses.Repairing);
            ClientName = req.Client.Login;
            IsTaken = Status.Id != (int)RequestStatuses.AwaitsConfirmation;
            IsArchive = Status.Id == (int)RequestStatuses.Finished;
        });

        if (_request is null)
        {
            return new NavigationResult
            {
                IsSuccess = false, Message = "Указанный запрос на ремонт не был найден в системе", NavigationArgs = args
            };
        }

        return base.OnNavigatedTo(args);
    }

    private void ChangeRequestStatus()
    {
        if (!_authorizedUserStore.IsAuthorized) return;
        var res = _requestService.ChangeStatus(new ChangeRequestStatusDto
            { RequestId = Id, Status = (RequestStatuses)SelectedStatus.Id });
        res.IfSucc(_ => _navigationService.PopAndNavigate(Routes.RequestForMaster, new DynamicDictionary(("id", Id))));
        res.IfFail(PushErrorToSnackbar);
    }

    private void TakeRequest()
    {
        if (!_authorizedUserStore.IsAuthorized) return;
        var res = _requestService.AssignMaster(new AssignMasterToRequestDto
            { MasterId = _authorizedUserStore.AuthorizedUser!.Id, RequestId = Id });
        res.IfSucc(_ => _navigationService.PopAndNavigate(Routes.RequestForMaster, new DynamicDictionary(("id", Id))));
        res.IfFail(PushErrorToSnackbar);
    }
}