using System.Windows;
using System.Windows.Controls;

namespace RepairShop.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigated += MainFrame_Navigated;
    }

    private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
    {
        var res = (e.Content as Page);
        if (res == null) return;

        if (!MainFrame.NavigationService.CanGoBack) GoBackButton.Visibility = Visibility.Collapsed;
        else GoBackButton.Visibility = Visibility.Visible;
        TitleTb.Text = res.Title;
        Title = res.Title;
    }

    private void GoBackButton_Click(object sender, RoutedEventArgs e)
    {
        if (!MainFrame.NavigationService.CanGoBack) return;
        MainFrame.NavigationService.GoBack();
    }
}
