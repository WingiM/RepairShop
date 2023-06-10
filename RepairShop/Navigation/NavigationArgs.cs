namespace RepairShop.Navigation;

public class NavigationArgs
{
    private readonly Dictionary<string, object>? _parameters;

    public NavigationArgs(Dictionary<string, object>? parameters)
    {
        _parameters = parameters;
    }

    public NavigationArgs(params KeyValuePair<string, object>[] parameters)
    {
        _parameters = new Dictionary<string, object>(parameters);
    }

    public T? TryGetParameterValue<T>(string key)
    {
        if (_parameters is null || !_parameters.TryGetValue(key, out var value))
            return default;

        if (value is T result)
            return result;
        return default;
    }
}
