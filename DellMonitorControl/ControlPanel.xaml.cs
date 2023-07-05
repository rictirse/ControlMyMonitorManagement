using CMM.Library.Method;
using CMM.Library.ViewModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DellMonitorControl;

/// <summary>
/// Interaction logic for ControlPanel.xaml
/// </summary>
public partial class ControlPanel : UserControl
{
    public ControlPanel()
    {
        InitializeComponent();
    }

    public async Task Refresh()
    {
        await CMMCommand.ScanMonitor();
        var monitors = await CMMCommand.ReadMonitorsData();
        sp.Children.Clear();

        foreach (var m in monitors)
        {
            var status = await CMMCommand.GetMonPowerStatus(m.SerialNumber);
            var ctrl = CreatControl(m, status);
            sp.Children.Add(ctrl);
        }
    }

    private StackPanel CreatControl(XMonitor monitorModel, string powerStatus)
    {
        var _sp = new StackPanel();

        _sp.Orientation = Orientation.Vertical;
        _sp.Margin = new Thickness(10, 5, 5, 0);

        var tb = new TextBlock
        {
            Text = $"{monitorModel.MonitorName}({monitorModel.SerialNumber})",
            HorizontalAlignment = HorizontalAlignment.Left,
            Style = (Style)FindResource("LableStyle")
        };

        var btn = new Button
        {
            Tag = monitorModel.SerialNumber,
            Content = powerStatus,
            Style = (Style)FindResource("TextButtonStyle")
        };

        btn.Click += async (s, e) => await ToggleButton_Checked(s, e);

        _sp.Children.Add(tb);
        _sp.Children.Add(btn);

        return _sp;
    }

    private void Border_MouseLeave(object sender, MouseEventArgs e)
    {
        
    }

    private async Task ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        var tag = btn?.Tag.ToString();
        var content = btn?.Content as string;
        if (content == "Sleep")
        { 
            await CMMCommand.PowerOn(tag);
        }
        else
        {
            await CMMCommand.Sleep(tag);
        }

        await Task.Delay(1000);
        btn!.Content = await CMMCommand.GetMonPowerStatus(tag);
    }
}
