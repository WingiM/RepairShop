using CommunityToolkit.Mvvm.ComponentModel;
using RepairShop.Attributes;
using RepairShop.ViewModels.Base;

namespace RepairShop.ViewModels;

[Route(Route = Routes.Request)]
public partial class RequestPageViewModel : BaseViewModel
{
    public static string SaveButtonTooltipIfNotEditable => ValidationErrorMessages.CannotEditRepairRequest;

    private readonly INavigationService _navigationService;
    private readonly IRequestService _requestService;

    [ObservableProperty]
    private bool _isCreate;

    [ObservableProperty]
    private bool _isEditable;

    public RequestPageViewModel(INavigationService navigationService,
        IRequestService requestService)
    {
        ViewModelTitle = "Запрос на ремонт";
        _navigationService = navigationService;
        _requestService = requestService;
    }

    public override NavigationResult OnNavigatedTo(NavigationArgs args)
    {
        var isCreate = args.Parameters.GetValue<bool>("isCreate");
        if (isCreate)
        {
            IsCreate = true;
            return base.OnNavigatedTo(args);
        }

        var requestId = args.Parameters.GetValue<int>("id");

        var result = _requestService.GetRequestById(requestId);
        if (result.IsFaulted)
        {
            result.IfFail(PushErrorToSnackbar);
            return base.OnNavigatedTo(args);
        }

        RepairRequest? request = null;
        result.IfSucc(req =>
        {
            if (req is null)
            {
                return;
            }

            request = req;
            ViewModelTitle += $" № {req.Id}";
            IsEditable = req.MasterId != default;
        });

        if (request is null)
        {
            return new NavigationResult { IsSuccess = false, Message = "Указанный запрос на ремонт не был найден в системе", NavigationArgs = args };
        }

        return base.OnNavigatedTo(args);
    }
}
