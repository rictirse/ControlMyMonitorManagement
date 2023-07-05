using System.Threading.Tasks;
using System.Windows;

namespace DellMonitorControl;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.Hide();
        taskbar.TrayPopupOpen += async (s, e) => await Taskbar_TrayPopupOpen(s, e);
    }

    private async Task Taskbar_TrayPopupOpen(object sender, RoutedEventArgs e)
    {
        await comtrolPanel.Refresh();
    }
}
