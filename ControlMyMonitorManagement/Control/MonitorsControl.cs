using System.Windows.Controls;
using System.Windows;
using CMM.Library.ViewModel;
using CMM.Library.Base;
using CMM.Library.Method;
using System.Windows.Data;
using System;

namespace CMM.Management.Control
{
    /// <summary>
    /// 全部螢幕
    /// </summary>
    internal class MonitorsControl : System.Windows.Controls.Control
    {
        public readonly static DependencyProperty MonitorsProperty;
        private StackPanel _sp;

        static MonitorsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MonitorsControl), new FrameworkPropertyMetadata(typeof(MonitorsControl)));

            MonitorsProperty = DependencyProperty.Register(
                "Monitors", 
                typeof(ObservableRangeCollection<XMonitor>), 
                typeof(MonitorsControl),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnMonitorsChangedCallback));
        }

        public override void OnApplyTemplate()
        {
            _sp = Template.FindName("sp", this) as StackPanel;
        }

        public ObservableRangeCollection<XMonitor> Monitors
        {
            get => (ObservableRangeCollection<XMonitor>)GetValue(MonitorsProperty);
            set => SetValue(MonitorsProperty, value);
        }

        static void OnMonitorsChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var me = sender as MonitorsControl;
            if (me != null)
            {
                me.OnMonitorsChanged((ObservableRangeCollection<XMonitor>)args.NewValue);
            }
        }

        public virtual void OnMonitorsChanged(ObservableRangeCollection<XMonitor> value)
        {
            foreach (var mon in value)
            {
                var monCtrl = new MonCtrl();
                monCtrl.Mon = mon;

                _sp.Children.Add(monCtrl);
            }
        }
    }
}
