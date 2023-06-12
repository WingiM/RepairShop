using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;
using System.Collections.ObjectModel;

namespace RepairShop.ViewModels;

[Route(Route = Routes.Request)]
public partial class RequestPageViewModel : BaseViewModel
{
    public static string SaveButtonTooltipIfNotEditable => ValidationErrorMessages.CannotEditRepairRequest;
    public static string TitleTooltip => "Короткое название для запроса";
    public static string DescriptionTooltip => "Подробное описание проблемы";

    private readonly INavigationService<BaseViewModel> _navigationService;
    private readonly IRequestService _requestService;
    private readonly AuthorizedUserStore _authorizedUserStore;

    private RepairRequest? _request;

    [ObservableProperty] private bool _isEdit = true;

    [ObservableProperty] private bool _isEditable;

    [ObservableProperty] private int _id;

    [ObservableProperty] private string _title = string.Empty;

    [ObservableProperty] private string _description = string.Empty;

    [ObservableProperty] private RequestStatus _status = null!;

    [ObservableProperty] private string? _masterName;

    [ObservableProperty] private ObservableCollection<StatusHistory> _statusHistory = null!;

    public RelayCommand SaveCommand { get; set; }

    public RequestPageViewModel(INavigationService<BaseViewModel> navigationService,
        IRequestService requestService,
        AuthorizedUserStore authorizedUserStore)
    {
        ViewModelTitle = "Запрос на ремонт";
        _navigationService = navigationService;
        _requestService = requestService;
        _authorizedUserStore = authorizedUserStore;

        SaveCommand = new RelayCommand(() => SaveRequest(), () => IsEditable);
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        var isCreate = args.Parameters.GetValue<bool>("isCreate");
        if (isCreate)
        {
            IsEdit = false;
            IsEditable = true;
            return base.OnNavigatedTo(args);
        }

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
            MasterName = req.Master?.Login;
            IsEditable = !IsEdit || req.MasterId == default;
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

    private void SaveRequest()
    {
        if (!_authorizedUserStore.IsAuthorized) return;
        if (IsEdit)
        {
            var updateDto = new UpdateRepairRequestDto()
                { Description = Description, ShortName = Title, RequestId = Id };
            var res = _requestService.Update(updateDto);
            res.IfFail(PushErrorToSnackbar);
            res.IfSucc(_ => WriteToSnackBar("Изменения сохранены"));
        }
        else
        {
            var createDto = new CreateRepairRequestDto()
                { Description = Description, ShortName = Title, ClientId = _authorizedUserStore.AuthorizedUser!.Id };
            var res = _requestService.Create(createDto);
            res.IfFail(PushErrorToSnackbar);
            res.IfSucc(_ => _navigationService.GoBack());
        }
    }
}