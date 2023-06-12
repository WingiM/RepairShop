namespace RepairShop.Navigation;

public class NavigationArgs
{
    public required string Destination { get; init; }

    public NavigationMode NavigationMode { get; init; } = NavigationMode.New;
    public DynamicDictionary Parameters { get; init; } = new();
}