using CMM.Library.Method;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

    private void Border_MouseLeave(object sender, MouseEventArgs e)
    {

    }

    private async void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        var toggle = sender as Button;
        var tag = toggle?.Tag.ToString();
        var content = toggle?.Content as string;
        if (content == "Sleep")
        { 
            await CMMCommand.Sleep(tag);
            toggle!.Content = "PowerOn";
        }
        else
        {
            await CMMCommand.PowerOn(tag);
            toggle!.Content = "Sleep";
        }
    }
}
