namespace RepairShop.Navigation;

public interface INavigatable
{
    public NavigationResult OnNavigatedTo(NavigationArgs args);
}