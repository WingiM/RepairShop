using RepairShop.ViewModels;

namespace RepairShop.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == System.Windows.Input.MouseButton.XButton1 && _viewModel.GoBackCommand.CanExecute(null))
        {
            _viewModel.GoBackCommand.Execute(null);
        }
    }
}
